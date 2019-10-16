using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Interfaces
{
    interface IRepository<T> where T : class
    {
        T Create(T dataObject);
        T Read(object id);
        IList<T> ReadAll();
        bool Update(T updatedDataObject);
        bool Delete(object id);
        bool Exits(object id);
    }
}
