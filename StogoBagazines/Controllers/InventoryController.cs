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

namespace StogoBagazines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// Logging object
        /// </summary>
        private readonly ILogger<InventoryController> logger;
        /// <summary>
        /// Database object
        /// </summary>
        private readonly Database database;
        /// <summary>
        /// Repository object
        /// </summary>
        private readonly InventoryBaseRepository repository;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="database">Dependency injection database object</param>
        public InventoryController(ILogger<InventoryController> logger, Database database)
        {
            this.logger = logger;
            this.database = database;
            repository = new InventoryBaseRepository(database);
        }
        // GET: api/Inventory
        /// <summary>
        /// Returns all available InventoryBase objects
        /// </summary>
        /// <returns>Enumerable array</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<InventoryBase>> Get()
        {
            List<InventoryBase> results = repository.ReadAll().ToList();
            if (results.Count > 0)
            {
                return Ok(results);
            }
            return NoContent();
        }

        // GET: api/Inventory/5
        /// <summary>
        /// Returns particular Inventory based on identity key
        /// </summary>
        /// <param name="id">Identification key</param>
        /// <returns>Inventory object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<InventoryBase> Get(int id)
        {
            InventoryBase result = repository.Read(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new KeyValuePair<string, int>("id", id));
        }

        // POST: api/Inventory
        /// <summary>
        /// Inserts new Inventory item into database set
        /// </summary>
        /// <param name="value">Inventory item serialised in request body</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] InventoryBase value)
        {
            return Ok(repository.Create(value));
        }

        // PUT: api/Test/5
        /// <summary>
        /// Updates existing Inventory entry in database
        /// </summary>
        /// <param name="id">Object to update</param>
        /// <param name="value">Updated object</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody] InventoryBase value)
        {
            if (repository.Exists(id))
            {
                if (repository.Update(id, value))
                {
                    return Ok();
                }
            }
            return NotFound(new KeyValuePair<string, int>("id", id));
        }

        // DELETE: api/Inventory/5
        /// <summary>
        /// Deletes existing Inventory entry in database
        /// </summary>
        /// <param name="id">Inventory entry to be deleted reference</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if (repository.Exists(id))
            {
                if (repository.Delete(id))
                {
                    return Ok();
                }
            }
            return NotFound(new KeyValuePair<string, int>("id", id));
        }
    }
}
