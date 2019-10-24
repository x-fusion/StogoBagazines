using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StogoBagazines.DataAccess.Interfaces;
using StogoBagazines.DataAccess.Models;
using MySql.Data.MySqlClient;

namespace StogoBagazines.DataAccess.Repositories
{
    /// <summary>
    /// Repository which provides access to entity manipulation in database
    /// </summary>
    public class InventoryBaseRepository : Repository, IRepository<InventoryBase>
    {
        /// <summary>
        /// Repository constructor
        /// </summary>
        /// <param name="database">Database object reference</param>
        public InventoryBaseRepository(Database database) : base(database)
        {

        }
        /// <summary>
        /// Creates and inserts entry in database
        /// </summary>
        /// <param name="dataObject">Entry reference</param>
        /// <returns>Id of entry which was created in database</returns>
        public object Create(InventoryBase dataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "INSERT INTO Inventory(Title,Amount,Revenue,TotalRentDuration,MonetaryValue) VALUES(@Title,@Amount,@Revenue,@TotalRentDuration,@MonetaryValue);"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Title", dataObject.Title);
            sqlCommand.Parameters.AddWithValue("@Amount", dataObject.Amount);
            sqlCommand.Parameters.AddWithValue("@Revenue", dataObject.Revenue);
            sqlCommand.Parameters.AddWithValue("@TotalRentDuration", dataObject.TotalRentDuration);
            sqlCommand.Parameters.AddWithValue("@MonetaryValue", dataObject.MonetaryValue);
            object insertedObjectId = -1;
            using (Database.Connection)
            {
                if (sqlCommand.ExecuteNonQuery() == 1)
                {
                    sqlCommand.CommandText = "SELECT LAST_INSERT_ID();";
                    insertedObjectId = sqlCommand.ExecuteScalar();
                    return insertedObjectId;
                }
                else
                {
                    return insertedObjectId;
                }
            }
        }
        /// <summary>
        /// Deletes entry in database
        /// </summary>
        /// <param name="id">Id of entry to be deleted</param>
        /// <returns>If deletion was successful</returns>
        public bool Delete(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "DELETE FROM Inventory WHERE Id=@Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                if (sqlCommand.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Check if entry is in database entity
        /// </summary>
        /// <param name="id">Reference of entry</param>
        /// <returns>Does exist</returns>
        public bool Exists(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "SELECT * FROM Inventory WHERE Id=@Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Returns entry from database
        /// </summary>
        /// <param name="id">Id of entry to be retrieved</param>
        /// <returns>Entry identified by id</returns>
        public InventoryBase Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "SELECT * FROM Inventory WHERE Id=@Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new InventoryBase(reader.GetInt32("Id"), reader.GetString("Title"),
                    reader.GetInt32("Amount"), reader.GetDecimal("Revenue"), reader.GetInt32("TotalRentDuration"), reader.GetDecimal("MonetaryValue"));
                }
                return null;
            }
        }
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        public IList<InventoryBase> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "SELECT * FROM Inventory;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<InventoryBase> items = new List<InventoryBase>();
                while (reader.Read())
                {
                    items.Add(new InventoryBase(reader.GetInt32("Id"), reader.GetString("Title"),
                    reader.GetInt32("Amount"), reader.GetDecimal("Revenue"), reader.GetInt32("TotalRentDuration"), reader.GetDecimal("MonetaryValue")));
                }
                return items;
            }
        }
        /// <summary>
        /// Updates entry in database entity
        /// </summary>
        /// <param name="id">Object to update id</param>
        /// <param name="updatedDataObject">Updated entry</param>
        /// <returns>If update was successful</returns>
        public bool Update(object id, InventoryBase updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "UPDATE Inventory SET Title = @Title,Amount=@Amount,Revenue=@Revenue,TotalRentDuration=@TotalRentDuration,MonetaryValue=@MonetaryValue WHERE Id=@Id;"
            };
            Database.Connection.Open();
            using(Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue("@Title", updatedDataObject.Title);
                sqlCommand.Parameters.AddWithValue("@Amount", updatedDataObject.Amount);
                sqlCommand.Parameters.AddWithValue("@Revenue", updatedDataObject.Revenue);
                sqlCommand.Parameters.AddWithValue("@TotalRentDuration", updatedDataObject.TotalRentDuration);
                sqlCommand.Parameters.AddWithValue("@MonetaryValue", updatedDataObject.MonetaryValue);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                MySqlTransaction sqlTransaction = Database.Connection.BeginTransaction();
                if (sqlCommand.ExecuteNonQuery() == 1)
                {
                    sqlTransaction.Commit();
                    return true;
                }
                sqlTransaction.Rollback();
                return false;
            }
        }
    }
}
