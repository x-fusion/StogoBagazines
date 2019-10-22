using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StogoBagazines.DataAccess.Interfaces;
using StogoBagazines.DataAccess.Models;

namespace StogoBagazines.DataAccess.Repositories
{
    /// <summary>
    /// BicycleRack objects repository
    /// </summary>
    public class BicycleRackRepository : IRepository<BicycleRack>
    {
        public Database Database { get; private set; }
        public BicycleRackRepository(Database database)
        {
            Database = database;
        }
        public object Create(BicycleRack dataObject)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object id)
        {
            throw new NotImplementedException();
        }

        public BicycleRack Read(object id)
        {
            throw new NotImplementedException();
        }

        public IList<BicycleRack> ReadAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(object id, BicycleRack updatedDataObject)
        {
            throw new NotImplementedException();
        }
    }
}
