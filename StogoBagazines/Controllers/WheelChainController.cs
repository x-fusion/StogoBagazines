using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StogoBagazines.DataAccess;
using StogoBagazines.DataAccess.Repositories;
using StogoBagazines.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StogoBagazines.ApiRequests;

namespace StogoBagazines.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class WheelChainController : ControllerBase
    {
        /// <summary>
        /// Logging object
        /// </summary>
        private readonly ILogger<WheelChainController> logger;
        /// <summary>
        /// Database object
        /// </summary>
        private readonly Database database;
        /// <summary>
        /// Repository object
        /// </summary>
        private readonly WheelChainRepository repository;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="database">Dependency injection database object</param>
        public WheelChainController(ILogger<WheelChainController> logger, Database database)
        {
            this.logger = logger;
            this.database = database;
            repository = new WheelChainRepository(database);
        }
        // GET: api/WheelChain
        /// <summary>
        /// Returns all available WheelChain objects
        /// </summary>
        /// <returns>Enumerable array</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<WheelChain>> Get()
        {
            List<WheelChain> results = repository.ReadAll().ToList();
            if (results.Count > 0)
            {
                return Ok(results);
            }
            return NoContent();
        }

        // GET: api/WheelChain/5
        /// <summary>
        /// Returns particular WheelChain object based on identity key
        /// </summary>
        /// <param name="id">Identification key</param>
        /// <returns>WheelChain object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<WheelChain> Get(int id)
        {
            WheelChain result = repository.Read(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new Response
            {
                Message = $"Object doesn't exist",
                Payload = id
            });
        }

        // POST: api/WheelChain
        /// <summary>
        /// Inserts new Bicycle Rack item into database set
        /// </summary>
        /// <param name="value">Bicycle Rack item serialised in request body</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Post([FromBody] WheelChain value)
        {
            object id = repository.Create(value);
            value.Id = int.Parse(id.ToString());
            return Ok(new Response
            {
                Message = "Succesfully submitted",
                Payload = value
            });
        }

        // PUT: api/WheelChain/5
        /// <summary>
        /// Updates existing WheelChain entry in database
        /// </summary>
        /// <param name="id">Object to update</param>
        /// <param name="value">Updated object</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody] WheelChain value)
        {
            if (repository.Exists(id))
            {
                if (repository.Update(id, value))
                {
                    return Ok(new Response
                    {
                        Message = "Succesfully updated",
                        Payload = value
                    });
                }
            }
            return NotFound(new Response
            {
                Message = $"Object doesn't exist",
                Payload = id
            });
        }

        // DELETE: api/WheelChain/5
        /// <summary>
        /// Deletes existing WheelChain entry in database
        /// </summary>
        /// <param name="id">WheelChain entry to be deleted reference</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if (repository.Exists(id))
            {
                if (repository.Delete(id))
                {
                    return Ok(new Response
                    {
                        Message = "Succesfully deleted",
                        Payload = id
                    });
                }
            }
            return NotFound(new Response
            {
                Message = $"Object doesn't exist",
                Payload = id
            });
        }
    }
}
