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
        public UsersController(ILogger<UsersController> logger, JwtOptions jwtOptions, Database database, TokenValidationParameters tokenValidationParameters)
        {
            userService = new UserService(jwtOptions, database, tokenValidationParameters);
            tokenService = new RefreshTokenService(database);
            this.logger = logger;
            this.database = database;
            this.jwtOptions = jwtOptions;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Authenticate([FromBody]AuthenticateRequest request)
        {
            User user = userService.Authetificate(request.Email, request.Password);
            if (user == null)
            {
                return BadRequest(new AuthResponse
                {
                    Message = "Provided credentials were incorrect!"
                });
            }
            return Ok(userService.GenerateAuthentificationResultForUser(user));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody]User user)
        {
            if (userService.Exists(user.Email))
            {
                return BadRequest(new Response { Message = "Such email is already registered" });
            }
            user.Role = Role.User;
            object id = userService.Create(user);
            if (id != (object)-1)
            {
                return Ok(new Response { 
                    Message = "User was successfully created",
                    Payload = id
                });
            }
            return BadRequest(new Response { 
                Message = "Registration couldn't succeed...",
                Payload = user
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = Role.SysAdmin)]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> users = userService.ReadAll().ToList();
            if (users.Count > 0)
            {
                return Ok(users);
            }
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Refresh([FromBody]RefreshTokenRequest request)
        {
            AuthResponse response = userService.RefreshToken(request.Token, request.RefreshToken);
            if(response.Message != "Authenticated")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}