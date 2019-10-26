﻿using System;
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
    public class CrossbarController : ControllerBase
    {
        /// <summary>
        /// Logging object
        /// </summary>
        private readonly ILogger<CrossbarController> logger;
        /// <summary>
        /// Database object
        /// </summary>
        private readonly Database database;
        /// <summary>
        /// Repository object
        /// </summary>
        private readonly CrossbarRepository repository;
        /// <summary>
        /// API endpoint constructor
        /// </summary>
        /// <param name="logger">Dependency injection logging object</param>
        /// <param name="database">Dependency injection database object</param>
        public CrossbarController(ILogger<CrossbarController> logger, Database database)
        {
            this.logger = logger;
            this.database = database;
            repository = new CrossbarRepository(database);
        }
        // GET: api/Crossbar
        /// <summary>
        /// Returns all available Crossbar objects
        /// </summary>
        /// <returns>Enumerable array</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Crossbar>> Get()
        {
            List<Crossbar> results = repository.ReadAll().ToList();
            if (results.Count > 0)
            {
                return Ok(results);
            }
            return NoContent();
        }

        // GET: api/Crossbar/5
        /// <summary>
        /// Returns particular Crossbar based on identity key
        /// </summary>
        /// <param name="id">Identification key</param>
        /// <returns>Crossbar object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CrossbarController> Get(int id)
        {
            Crossbar result = repository.Read(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new KeyValuePair<string, int>("id", id));
        }

        // POST: api/Crossbar
        /// <summary>
        /// Inserts new Crossbar item into database set
        /// </summary>
        /// <param name="value">Crossbar item serialised in request body</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] Crossbar value)
        {
            return Ok(new KeyValuePair<string, string>("id", repository.Create(value).ToString()));
        }

        // PUT: api/Crossbar/5
        /// <summary>
        /// Updates existing Crossbar entry in database
        /// </summary>
        /// <param name="id">Object to update</param>
        /// <param name="value">Updated object</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody] Crossbar value)
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

        // DELETE: api/Crossbar/5
        /// <summary>
        /// Deletes existing Crossbar entry in database
        /// </summary>
        /// <param name="id">Crossbar entry to be deleted reference</param>
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
