using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StogoBagazines.DataAccess;
using StogoBagazines.DataAccess.Repositories;
using StogoBagazines.DataAccess.Objects;

namespace StogoBagazines.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly Database database;

        public InventoryController(ILogger<WeatherForecastController> logger, Database database)
        {
            this.logger = logger;
            this.database = database;
        }

        [HttpGet]
        public IEnumerable<InventoryBase> Get(object id)
        {
            return null;
        }
    }
}
