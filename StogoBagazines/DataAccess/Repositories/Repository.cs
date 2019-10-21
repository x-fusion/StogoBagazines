using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Repositories
{
    /// <summary>
    /// Base class of repository implementation
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Connectivity to database
        /// </summary>
        public Database Database { get; private set; }
        protected string lastInsertCmd = "SELECT LAST_INSERT_ID();";
        /// <summary>
        /// Repository constructor
        /// </summary>
        /// <param name="database">Database reference object</param>
        public Repository(Database database)
        {
            Database = database;
        }
    }
}
