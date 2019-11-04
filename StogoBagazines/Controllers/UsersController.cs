using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StogoBagazines.ApiRequests;
using StogoBagazines.Services;
using StogoBagazines.DataAccess.Models;
using Microsoft.Extensions.Logging;
using StogoBagazines.DataAccess;
using StogoBagazines.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StogoBagazines.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace StogoBagazines.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Logging object
        /// </summary>
        private readonly ILogger<UsersController> logger;
        /// <summary>
        /// Database object
        /// </summary>
        private readonly Database database;
        /// <summary>
        /// User service object
        /// </summary>
        private readonly UserService userService;
        private readonly RefreshTokenService tokenService;
        /// <summary>
        /// Jwt Settings
        /// </summary>
        private readonly JwtOptions jwtOptions;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="jwtOptions">Dependency injection jwtOptions object</param>
        /// <param name="database">Dependency injection database object</param>
        public UsersController(ILogger<UsersController> logger, JwtOptions jwtOptions, Database database)
        {
            userService = new UserService(jwtOptions, database);
            tokenService = new RefreshTokenService(database);
            this.logger = logger;
            this.database = database;
            this.jwtOptions = jwtOptions;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateRequest request)
        {
            User user = userService.Authetificate(request.Email, request.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Provided credentials are incorrect!" });
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new AuthResponse
            {
                Message = "Authentificated",
                Token = tokenHandler.WriteToken(token),
                RefreshToken = 
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Create([FromBody]User user)
        {
            if (userService.Exists(user.Email))
            {
                return BadRequest(new Response { Message = "Such email is already registered" });
            }
            user.Role = Role.User;
            if(userService.Create(user) != (object)-1)
            {
                return Ok(new Response { Message = "User was successfully created" });
            }
            return BadRequest(new Response { Message = "Registration couldn't complete, try again." });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> users = userService.ReadAll().ToList();
            if (users.Count > 0)
            {
                return Ok(users);
            }
            return NoContent();
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Refresh([FromBody]RefreshTokenRequest request)
        {
            User user = userService.RefreshToken(request.Email, request.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Provided credentials are incorrect!" });
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new AuthResponse
            {
                Message = "Authentificated",
                Token = tokenHandler.WriteToken(token),
                RefreshToken =
            });
        }
    }
}