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
        private readonly UserService service;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="jwtOptions">Dependency injection jwtOptions object</param>
        /// <param name="database">Dependency injection database object</param>
        public UsersController(ILogger<UsersController> logger, JwtOptions jwtOptions,Database database)
        {
            service = new UserService(jwtOptions, database);
            this.logger = logger;
            this.database = database;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateRequest request)
        {
            User user =  service.Authetificate(request.Email, request.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Provided credentials are incorrect!" });
            }
            return Ok(user);
        }

        [Authorize(Roles = Role.SysAdmin)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> users = service.ReadAll().ToList();
            if(users.Count > 0)
            {
                return Ok(users);
            }
            return NoContent();
        }
    }
}