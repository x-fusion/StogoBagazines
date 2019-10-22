using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Interfaces
{
    /// <summary>
    /// Defines manditory capabilities of repository
    /// </summary>
    /// <typeparam name="T">Object for which repository is meant to be used</typeparam>
    interface IRepository<T> where T : class
    {
        /// <summary>
        /// Creates an entry in database entity
        /// </summary>
        /// <param name="dataObject">Entry to be inserted</param>
        /// <returns>Inserted entry id</returns>
        object Create(T dataObject);
        /// <summary>
        /// Returns particular entry from database
        /// </summary>
        /// <param name="id">Identification key of entry to be retrieved</param>
        /// <returns>Object which is related to identification key</returns>
        T Read(object id);
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        IList<T> ReadAll();
        /// <summary>
        /// Updates particular object
        /// </summary>
        /// <param name="id">Object to update reference</param>
        /// <param name="updatedDataObject">Object which will replace old version of it</param>
        /// <returns>Updated object</returns>
        bool Update(object id, T updatedDataObject);
        /// <summary>
        /// Deletes object from database
        /// </summary>
        /// <param name="id">Identification key of object</param>
        /// <returns>Status of operation completion</returns>
        bool Delete(object id);
        /// <summary>
        /// Looks for particular entry in database entity
        /// </summary>
        /// <param name="id">Object identification key</param>
        /// <returns>Status of object existence</returns>
        bool Exists(object id);
    }
}
