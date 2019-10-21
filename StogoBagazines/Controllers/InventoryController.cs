using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StogoBagazines.DataAccess;
using StogoBagazines.DataAccess.Repositories;
using StogoBagazines.DataAccess.Objects;

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
        public IEnumerable<InventoryBase> Get()
        {
            return repository.ReadAll();
        }

        // GET: api/Inventory/5
        /// <summary>
        /// Returns particular Inventory based on identity key
        /// </summary>
        /// <param name="id">Identification key</param>
        /// <returns>Inventory object</returns>
        [HttpGet("{id}", Name = "Get")]
        public InventoryBase Get(object id)
        {
            return repository.Read(id);
        }

        // POST: api/Inventory
        /// <summary>
        /// Inserts new Inventory item into database set
        /// </summary>
        /// <param name="value">Inventory item serialised in request body</param>
        [HttpPost]
        public void Post([FromBody] InventoryBase value)
        {

        }

        // PUT: api/Test/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
