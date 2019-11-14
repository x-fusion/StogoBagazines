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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using StogoBagazines.ApiRequests;

namespace StogoBagazines.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BicycleRackController : ControllerBase
    {
        /// <summary>
        /// Logging object
        /// </summary>
        private readonly ILogger<BicycleRackController> logger;
        /// Repository object
        /// </summary>
        private readonly BicycleRackRepository repository;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="database">Dependency injection database object</param>
        public BicycleRackController(ILogger<BicycleRackController> logger, Database database)
        {
            this.logger = logger;
            repository = new BicycleRackRepository(database);
        }
        // GET: api/BicycleRack
        /// <summary>
        /// Returns all available BicycleRack objects
        /// </summary>
        /// <returns>Enumerable array</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<BicycleRack>> Get()
        {
            List<BicycleRack> results = repository.ReadAll().ToList();
            if (results.Count > 0)
            {
                return Ok(results);
            }
            return NoContent();

        }

        // GET: api/BicycleRack/5
        /// <summary>
        /// Returns particular Bicycle Rack based on identity key
        /// </summary>
        /// <param name="id">Identification key</param>
        /// <returns>Inventory object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<BicycleRack> Get(int id)
        {
            BicycleRack result = repository.Read(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new Response
            {
                Message = $"Object with {id} doesn't exist"
            });
        }

        // POST: api/BicycleRack
        /// <summary>
        /// Inserts new Bicycle Rack item into database set
        /// </summary>
        /// <param name="value">Bicycle Rack item serialised in request body</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Post([FromBody] BicycleRack value)
        {
            object id = repository.Create(value);
            value.Id = int.Parse(id.ToString());
            return Ok(new Response
            {
                Message = "Succesfully submitted",
                Payload = value
            });
        }

        // PUT: api/BicycleRack/5
        /// <summary>
        /// Updates existing Inventory entry in database
        /// </summary>
        /// <param name="id">Object to update</param>
        /// <param name="value">Updated object</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody] BicycleRack value)
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
                Message = $"Object with {id} doesn't exist"
            });
        }

        // DELETE: api/BicycleRack/5
        /// <summary>
        /// Deletes existing Bicycle Rack entry in database
        /// </summary>
        /// <param name="id">Inventory entry to be deleted reference</param>
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
                        Message = "Succesfully deleted"
                    });
                }
            }
            return NotFound(new Response
            {
                Message = $"Object with {id} doesn't exist"
            });
        }
    }
}
