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
    public class WheelChainRepository : Repository, IRepository<WheelChain>
    {
        public WheelChainRepository(Database database) : base(database)
        {

        }
        /// <summary>
        /// Creates and inserts entry in database
        /// </summary>
        /// <param name="dataObject">Entry reference</param>
        /// <returns>Id of entry which was created in database</returns>
        public object Create(WheelChain dataObject)
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
                        CommandText = $"INSERT INTO WheelChain({nameof(WheelChain.InventoryId)},{nameof(WheelChain.TireDimensions)},{nameof(WheelChain.ChainThickness)}," +
                        $"{nameof(WheelChain.Type)}) VALUES(@{nameof(WheelChain.InventoryId)},@{nameof(WheelChain.TireDimensions)},@{nameof(WheelChain.ChainThickness)},@{nameof(WheelChain.Type)});"
                    };
                    sqlCommand.Prepare();
                    sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.InventoryId)}", dataObject.InventoryId);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.TireDimensions)}", dataObject.TireDimensions);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.ChainThickness)}", dataObject.ChainThickness);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.Type)}", dataObject.Type);
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
        public WheelChain Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT WheelChain.{nameof(WheelChain.InventoryId)}, WheelChain.{nameof(WheelChain.Id)}, Inventory.{nameof(WheelChain.Title)}, " +
                $"Inventory.{nameof(WheelChain.Amount)}, Inventory.{nameof(WheelChain.Revenue)}, " +
                $"Inventory.{nameof(WheelChain.TotalRentDuration)}, Inventory.{nameof(WheelChain.MonetaryValue)}, " +
                $"WheelChain.{nameof(WheelChain.TireDimensions)}, WheelChain.{nameof(WheelChain.ChainThickness)}, " +
                $"WheelChain.{nameof(WheelChain.Type)} FROM Inventory INNER JOIN WheelChain ON " +
                $"Inventory.{nameof(WheelChain.Id)} = WheelChain.{nameof(WheelChain.InventoryId)} " +
                $"WHERE WheelChain.{nameof(WheelChain.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new WheelChain
                    {
                        InventoryId = reader.GetInt32($"{nameof(WheelChain.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(WheelChain.Id)}"),
                        Title = reader.GetString($"{nameof(WheelChain.Title)}"),
                        Amount = reader.GetInt32($"{nameof(WheelChain.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(WheelChain.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(WheelChain.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(WheelChain.MonetaryValue)}"),
                        TireDimensions = reader[$"{nameof(RoofRack.AppearenceDescription)}"] is DBNull ? string.Empty : reader.GetString($"{nameof(WheelChain.TireDimensions)}"),
                        ChainThickness = reader.GetDouble($"{nameof(WheelChain.ChainThickness)}"),
                        Type = (WheelChain.VehicleType)Enum.Parse(typeof(WheelChain.VehicleType), reader.GetString($"{nameof(WheelChain.Type)}"))
                    };
                }
                return null;
            }
        }
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        public IList<WheelChain> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT WheelChain.{nameof(WheelChain.InventoryId)}, WheelChain.{nameof(WheelChain.Id)}, Inventory.{nameof(WheelChain.Title)}, " +
                $"Inventory.{nameof(WheelChain.Amount)}, Inventory.{nameof(WheelChain.Revenue)}, " +
                $"Inventory.{nameof(WheelChain.TotalRentDuration)}, Inventory.{nameof(WheelChain.MonetaryValue)}, " +
                $"WheelChain.{nameof(WheelChain.TireDimensions)}, WheelChain.{nameof(WheelChain.ChainThickness)}, " +
                $"WheelChain.{nameof(WheelChain.Type)} FROM Inventory INNER JOIN WheelChain ON " +
                $"Inventory.{nameof(WheelChain.Id)} = WheelChain.{nameof(WheelChain.InventoryId)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<WheelChain> items = new List<WheelChain>();
                while (reader.Read())
                {

                    items.Add(new WheelChain
                    {
                        InventoryId = reader.GetInt32($"{nameof(WheelChain.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(WheelChain.Id)}"),
                        Title = reader.GetString($"{nameof(WheelChain.Title)}"),
                        Amount = reader.GetInt32($"{nameof(WheelChain.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(WheelChain.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(WheelChain.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(WheelChain.MonetaryValue)}"),
                        TireDimensions = reader[$"{nameof(RoofRack.AppearenceDescription)}"] is DBNull ? string.Empty : reader.GetString($"{nameof(WheelChain.TireDimensions)}"),
                        ChainThickness = reader.GetDouble($"{nameof(WheelChain.ChainThickness)}"),
                        Type = (WheelChain.VehicleType)Enum.Parse(typeof(WheelChain.VehicleType), reader.GetString($"{nameof(WheelChain.Type)}"))
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
        public bool Update(object id, WheelChain updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE Inventory INNER JOIN WheelChain ON WheelChain.{nameof(WheelChain.InventoryId)} = Inventory.{nameof(WheelChain.Id)} " +
                $"SET {nameof(WheelChain.Title)}=@{nameof(WheelChain.Title)},{nameof(WheelChain.Amount)}=@{nameof(WheelChain.Amount)}," +
                $"{nameof(WheelChain.Revenue)}=@{nameof(WheelChain.Revenue)},{nameof(WheelChain.TotalRentDuration)}=@{nameof(WheelChain.TotalRentDuration)}," +
                $"{nameof(WheelChain.MonetaryValue)}=@{nameof(WheelChain.MonetaryValue)},{nameof(WheelChain.TireDimensions)}=@{nameof(WheelChain.TireDimensions)}," +
                $"{nameof(WheelChain.ChainThickness)}=@{nameof(WheelChain.ChainThickness)},{nameof(WheelChain.Type)}=@{nameof(WheelChain.Type)} " +
                $"WHERE Inventory.{nameof(WheelChain.Id)} = @{nameof(WheelChain.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.Title)}", updatedDataObject.Title);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.Amount)}", updatedDataObject.Amount);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.Revenue)}", updatedDataObject.Revenue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.TotalRentDuration)}", updatedDataObject.TotalRentDuration);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.MonetaryValue)}", updatedDataObject.MonetaryValue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.TireDimensions)}", updatedDataObject.TireDimensions);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.ChainThickness)}", updatedDataObject.ChainThickness);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.Type)}", updatedDataObject.Type);
                sqlCommand.Parameters.AddWithValue($"@{nameof(WheelChain.Id)}", id);
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
