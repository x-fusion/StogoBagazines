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
    public class RoofRackController : ControllerBase
    {
        /// <summary>
        /// Logging object
        /// </summary>
        private readonly ILogger<RoofRackController> logger;
        /// <summary>
        /// Database object
        /// </summary>
        private readonly Database database;
        /// <summary>
        /// Repository object
        /// </summary>
        private readonly RoofRackRepository repository;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="database">Dependency injection database object</param>
        public RoofRackController(ILogger<RoofRackController> logger, Database database)
        {
            this.logger = logger;
            this.database = database;
            repository = new RoofRackRepository(database);
        }
        // GET: api/RoofRack
        /// <summary>
        /// Returns all available RoofRack objects
        /// </summary>
        /// <returns>Enumerable array</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<RoofRack>> Get()
        {
            List<RoofRack> results = repository.ReadAll().ToList();
            if (results.Count > 0)
            {
                return Ok(results);
            }
            return NoContent();
        }

        // GET: api/RoofRack/5
        /// <summary>
        /// Returns particular RoofRack based on identity key
        /// </summary>
        /// <param name="id">Identification key</param>
        /// <returns>Inventory object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RoofRack> Get(int id)
        {
            RoofRack result = repository.Read(id);
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

        // POST: api/RoofRack
        /// <summary>
        /// Inserts new RoofRack item into database set
        /// </summary>
        /// <param name="value">RoofRack item serialised in request body</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] RoofRack value)
        {
            object id = repository.Create(value);
            value.Id = int.Parse(id.ToString());
            return Ok(new Response
            {
                Message = "Succesfully submitted",
                Payload = value
            });
        }

        // PUT: api/RoofRack/5
        /// <summary>
        /// Updates existing RoofRack entry in database
        /// </summary>
        /// <param name="id">Object to update</param>
        /// <param name="value">Updated object</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody] RoofRack value)
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

        // DELETE: api/RoofRack/5
        /// <summary>
        /// Deletes existing RoofRack entry in database
        /// </summary>
        /// <param name="id">RoofRack entry to be deleted reference</param>
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
