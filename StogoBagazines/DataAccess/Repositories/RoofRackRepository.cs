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
    public class RoofRackRepository : Repository, IRepository<RoofRack>
    {
        public RoofRackRepository(Database database) : base(database)
        {

        }
        /// <summary>
        /// Creates and inserts entry in database
        /// </summary>
        /// <param name="dataObject">Entry reference</param>
        /// <returns>Id of entry which was created in database</returns>
        public object Create(RoofRack dataObject)
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
                        CommandText = $"INSERT INTO RoofRack({nameof(RoofRack.InventoryId)},{nameof(RoofRack.Opening)},{nameof(RoofRack.LiftPower)}," +
                        $"{nameof(RoofRack.IsLockable)},{nameof(RoofRack.Weight)},{nameof(RoofRack.AppearenceDescription)}) " +
                        $"VALUES(@{nameof(RoofRack.InventoryId)},@{nameof(RoofRack.Opening)},@{nameof(RoofRack.LiftPower)},@{nameof(RoofRack.IsLockable)}," +
                        $"@{nameof(RoofRack.Weight)},@{nameof(RoofRack.AppearenceDescription)});"
                    };
                    sqlCommand.Prepare();
                    sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.InventoryId)}", dataObject.InventoryId);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Opening)}", dataObject.Opening);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.LiftPower)}", dataObject.LiftPower);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.IsLockable)}", dataObject.IsLockable);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Weight)}", dataObject.Weight);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.AppearenceDescription)}", dataObject.AppearenceDescription);
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
        public RoofRack Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT RoofRack.{nameof(RoofRack.InventoryId)}, RoofRack.{nameof(RoofRack.Id)}, Inventory.{nameof(RoofRack.Title)}, " +
                $"Inventory.{nameof(RoofRack.Amount)}, Inventory.{nameof(RoofRack.Revenue)}, " +
                $"Inventory.{nameof(RoofRack.TotalRentDuration)}, Inventory.{nameof(RoofRack.MonetaryValue)}, " +
                $"RoofRack.{nameof(RoofRack.Opening)}, RoofRack.{nameof(RoofRack.LiftPower)}, " +
                $"RoofRack.{nameof(RoofRack.IsLockable)}, RoofRack.{nameof(RoofRack.Weight)}, RoofRack.{nameof(RoofRack.AppearenceDescription)} " +
                $"FROM Inventory INNER JOIN RoofRack ON Inventory.{nameof(RoofRack.Id)} = RoofRack.{nameof(RoofRack.InventoryId)} " +
                $"WHERE RoofRack.{nameof(RoofRack.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new RoofRack
                    {
                        InventoryId = reader.GetInt32($"{nameof(RoofRack.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(RoofRack.Id)}"),
                        Title = reader.GetString($"{nameof(RoofRack.Title)}"),
                        Amount = reader.GetInt32($"{nameof(RoofRack.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(RoofRack.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(RoofRack.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(RoofRack.MonetaryValue)}"),
                        Opening = (RoofRack.OpeningType)Enum.Parse(typeof(RoofRack.OpeningType), reader.GetString($"{nameof(RoofRack.Opening)}")),
                        LiftPower = reader.GetDouble($"{nameof(RoofRack.LiftPower)}"),
                        IsLockable = reader.GetBoolean($"{nameof(RoofRack.IsLockable)}"),
                        Weight = reader.GetDouble($"{nameof(RoofRack.Weight)}"),
                        AppearenceDescription = reader[$"{nameof(RoofRack.AppearenceDescription)}"] is DBNull ? string.Empty : reader.GetString($"{nameof(RoofRack.AppearenceDescription)}")
                    };
                }
                return null;
            }
        }
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        public IList<RoofRack> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT RoofRack.{nameof(RoofRack.InventoryId)}, RoofRack.{nameof(RoofRack.Id)}, Inventory.{nameof(RoofRack.Title)}, " +
                $"Inventory.{nameof(RoofRack.Amount)}, Inventory.{nameof(RoofRack.Revenue)}, " +
                $"Inventory.{nameof(RoofRack.TotalRentDuration)}, Inventory.{nameof(RoofRack.MonetaryValue)}, " +
                $"RoofRack.{nameof(RoofRack.Opening)}, RoofRack.{nameof(RoofRack.LiftPower)}, " +
                $"RoofRack.{nameof(RoofRack.IsLockable)}, RoofRack.{nameof(RoofRack.Weight)}, RoofRack.{nameof(RoofRack.AppearenceDescription)} " +
                $"FROM Inventory INNER JOIN RoofRack ON Inventory.{nameof(RoofRack.Id)} = RoofRack.{nameof(RoofRack.InventoryId)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<RoofRack> items = new List<RoofRack>();
                while (reader.Read())
                {
                    items.Add(new RoofRack
                    {
                        InventoryId = reader.GetInt32($"{nameof(RoofRack.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(RoofRack.Id)}"),
                        Title = reader.GetString($"{nameof(RoofRack.Title)}"),
                        Amount = reader.GetInt32($"{nameof(RoofRack.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(RoofRack.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(RoofRack.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(RoofRack.MonetaryValue)}"),
                        Opening = (RoofRack.OpeningType)Enum.Parse(typeof(RoofRack.OpeningType), reader.GetString($"{nameof(RoofRack.Opening)}")),
                        LiftPower = reader.GetDouble($"{nameof(RoofRack.LiftPower)}"),
                        IsLockable = reader.GetBoolean($"{nameof(RoofRack.IsLockable)}"),
                        Weight = reader.GetDouble($"{nameof(RoofRack.Weight)}"),
                        AppearenceDescription = reader[$"{nameof(RoofRack.AppearenceDescription)}"] is DBNull ? string.Empty : reader.GetString($"{nameof(RoofRack.AppearenceDescription)}")
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
        public bool Update(object id, RoofRack updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE Inventory INNER JOIN RoofRack ON RoofRack.{nameof(RoofRack.InventoryId)} = Inventory.{nameof(RoofRack.Id)} " +
                $"SET {nameof(RoofRack.Title)}=@{nameof(RoofRack.Title)},{nameof(RoofRack.Amount)}=@{nameof(RoofRack.Amount)}," +
                $"{nameof(RoofRack.Revenue)}=@{nameof(RoofRack.Revenue)},{nameof(RoofRack.TotalRentDuration)}=@{nameof(RoofRack.TotalRentDuration)}," +
                $"{nameof(RoofRack.MonetaryValue)}=@{nameof(RoofRack.MonetaryValue)},{nameof(RoofRack.Opening)}=@{nameof(RoofRack.Opening)}," +
                $"{nameof(RoofRack.LiftPower)}=@{nameof(RoofRack.LiftPower)},{nameof(RoofRack.IsLockable)}=@{nameof(RoofRack.IsLockable)}," +
                $"{nameof(RoofRack.Weight)}=@{nameof(RoofRack.Weight)},{nameof(RoofRack.AppearenceDescription)}=@{nameof(RoofRack.AppearenceDescription)} " +
                $"WHERE Inventory.{nameof(RoofRack.Id)} = @{nameof(RoofRack.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Title)}", updatedDataObject.Title);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Amount)}", updatedDataObject.Amount);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Revenue)}", updatedDataObject.Revenue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.TotalRentDuration)}", updatedDataObject.TotalRentDuration);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.MonetaryValue)}", updatedDataObject.MonetaryValue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Opening)}", updatedDataObject.Opening);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.LiftPower)}", updatedDataObject.LiftPower);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.IsLockable)}", updatedDataObject.IsLockable);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Weight)}", updatedDataObject.Weight);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.AppearenceDescription)}", updatedDataObject.AppearenceDescription);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RoofRack.Id)}", id);
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
