using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using StogoBagazines.DataAccess.Interfaces;
using StogoBagazines.DataAccess.Models;

namespace StogoBagazines.DataAccess.Repositories
{
    /// <summary>
    /// Repository which provides access to entity manipulation in database
    /// </summary>
    public class OtherRepository : Repository, IRepository<Other>
    {
        public OtherRepository(Database database) : base(database)
        {

        }
        /// <summary>
        /// Creates and inserts entry in database
        /// </summary>
        /// <param name="dataObject">Entry reference</param>
        /// <returns>Id of entry which was created in database</returns>
        public object Create(Other dataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"INSERT INTO Inventory({nameof(InventoryBase.Title)},{nameof(InventoryBase.Amount)},{nameof(InventoryBase.Revenue)}," +
                $"{nameof(InventoryBase.TotalRentDuration)},{nameof(InventoryBase.MonetaryValue)}) VALUES(@{nameof(InventoryBase.Title)},@{nameof(InventoryBase.Amount)}," +
                $"@{nameof(InventoryBase.Revenue)},@{nameof(InventoryBase.TotalRentDuration)},@{nameof(InventoryBase.MonetaryValue)});"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(InventoryBase.Title)}", dataObject.Title);
            sqlCommand.Parameters.AddWithValue($"@{nameof(InventoryBase.Amount)}", dataObject.Amount);
            sqlCommand.Parameters.AddWithValue($"@{nameof(InventoryBase.Revenue)}", dataObject.Revenue);
            sqlCommand.Parameters.AddWithValue($"@{nameof(InventoryBase.TotalRentDuration)}", dataObject.TotalRentDuration);
            sqlCommand.Parameters.AddWithValue($"@{nameof(InventoryBase.MonetaryValue)}", dataObject.MonetaryValue);
            MySqlTransaction sqlTransaction = Database.Connection.BeginTransaction();
            object insertedObjectId = -1;
            using (Database.Connection)
            {
                if (sqlCommand.ExecuteNonQuery() == 1)
                {
                    sqlCommand.CommandText = "SELECT LAST_INSERT_ID();";
                    dataObject.InventoryId = int.Parse(sqlCommand.ExecuteScalar().ToString());
                    sqlCommand = new MySqlCommand
                    {
                        Connection = Database.Connection,
                        CommandText = $"INSERT INTO Other({nameof(Other.InventoryId)}) VALUES(@{nameof(Other.InventoryId)});"
                    };
                    sqlCommand.Prepare();
                    sqlCommand.Parameters.AddWithValue($"@{nameof(Other.InventoryId)}", dataObject.InventoryId);
                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        sqlCommand.CommandText = "SELECT LAST_INSERT_ID();";
                        insertedObjectId = sqlCommand.ExecuteScalar();
                    }
                    sqlTransaction.Commit();
                    return insertedObjectId;
                }
                else
                {
                    sqlTransaction.Rollback();
                    return insertedObjectId;
                }
            }
        }
        /// <summary>
        /// Deletes entry in database
        /// </summary>
        /// <param name="id">Id of entry to be deleted</param>
        /// <returns>If deletion was successful</returns>
        /// <returns></returns>
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
        public Other Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT Other.{nameof(Other.InventoryId)}, Other.{nameof(Other.Id)}, Inventory.{nameof(Other.Title)}, " +
                $"Inventory.{nameof(Other.Amount)}, Inventory.{nameof(Other.Revenue)}, " +
                $"Inventory.{nameof(Other.TotalRentDuration)}, Inventory.{nameof(Other.MonetaryValue)} " +
                $"FROM Inventory INNER JOIN Other ON " +
                $"Inventory.{nameof(Other.Id)} = Other.{nameof(Other.InventoryId)} " +
                $"WHERE Other.{nameof(Other.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new Other(reader.GetInt32($"{nameof(Other.Id)}"), reader.GetInt32($"{nameof(Other.InventoryId)}"),
                        reader.GetString($"{nameof(Other.Title)}"),
                        reader.GetInt32($"{nameof(Other.Amount)}"), reader.GetDecimal($"{nameof(Other.Revenue)}"),
                        reader.GetInt32($"{nameof(Other.TotalRentDuration)}"), reader.GetDecimal($"{nameof(Other.MonetaryValue)}"));
                }
                return null;
            }
        }
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        public IList<Other> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT Other.{nameof(Other.InventoryId)}, Other.{nameof(Other.Id)}, Inventory.{nameof(Other.Title)}, " +
                $"Inventory.{nameof(Other.Amount)}, Inventory.{nameof(Other.Revenue)}, " +
                $"Inventory.{nameof(Other.TotalRentDuration)}, Inventory.{nameof(Other.MonetaryValue)} " +
                $"FROM Inventory INNER JOIN Other ON " +
                $"Inventory.{nameof(Other.Id)} = Other.{nameof(Other.InventoryId)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<Other> items = new List<Other>();
                while (reader.Read())
                {
                    items.Add(new Other
                    {
                        InventoryId = reader.GetInt32($"{nameof(Other.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(Other.Id)}"),
                        Title = reader.GetString($"{nameof(Other.Title)}"),
                        Amount = reader.GetInt32($"{nameof(Other.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(Other.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(Other.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(Other.MonetaryValue)}")
                    });
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
        public bool Update(object id, Other updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE Inventory INNER JOIN Other ON Other.{nameof(Other.InventoryId)} = Inventory.{nameof(Other.Id)} " +
                $"SET {nameof(Other.Title)}=@{nameof(Other.Title)},{nameof(Other.Amount)}=@{nameof(Other.Amount)}," +
                $"{nameof(Other.Revenue)}=@{nameof(Other.Revenue)},{nameof(Other.TotalRentDuration)}=@{nameof(Other.TotalRentDuration)}," +
                $"{nameof(Other.MonetaryValue)}=@{nameof(Other.MonetaryValue)} " +
                $"WHERE Inventory.{nameof(Other.Id)} = @{nameof(Other.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(Other.Title)}", updatedDataObject.Title);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Other.Amount)}", updatedDataObject.Amount);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Other.Revenue)}", updatedDataObject.Revenue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Other.TotalRentDuration)}", updatedDataObject.TotalRentDuration);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Other.MonetaryValue)}", updatedDataObject.MonetaryValue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Other.Id)}", id);
                MySqlTransaction sqlTransaction = Database.Connection.BeginTransaction();
                if (sqlCommand.ExecuteNonQuery() > 0)
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
