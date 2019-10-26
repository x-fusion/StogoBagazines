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
    public class BicycleRackRepository : Repository, IRepository<BicycleRack>
    {
        public BicycleRackRepository(Database database) : base(database)
        {

        }
        /// <summary>
        /// Creates and inserts entry in database
        /// </summary>
        /// <param name="dataObject">Entry reference</param>
        /// <returns>Id of entry which was created in database</returns>
        public object Create(BicycleRack dataObject)
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
                        CommandText = $"INSERT INTO BicycleRack({nameof(BicycleRack.InventoryId)},{nameof(BicycleRack.BikeLimit)}, {nameof(BicycleRack.LiftPower)}," +
                        $"{nameof(BicycleRack.Assertion)}) VALUES(@{nameof(BicycleRack.InventoryId)},@{nameof(BicycleRack.BikeLimit)},@{nameof(BicycleRack.LiftPower)},@{nameof(BicycleRack.Assertion)});"
                    };
                    sqlCommand.Prepare();
                    sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.InventoryId)}", dataObject.InventoryId);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.BikeLimit)}", dataObject.BikeLimit);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.LiftPower)}", dataObject.LiftPower);
                    sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.Assertion)}", dataObject.Assertion);
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
        public BicycleRack Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT BicycleRack.{nameof(BicycleRack.InventoryId)}, BicycleRack.{nameof(BicycleRack.Id)}, Inventory.{nameof(BicycleRack.Title)}, " +
                $"Inventory.{nameof(BicycleRack.Amount)}, Inventory.{nameof(BicycleRack.Revenue)}, " +
                $"Inventory.{nameof(BicycleRack.TotalRentDuration)}, Inventory.{nameof(BicycleRack.MonetaryValue)}, " +
                $"BicycleRack.{nameof(BicycleRack.BikeLimit)}, BicycleRack.{nameof(BicycleRack.LiftPower)}, " +
                $"BicycleRack.{nameof(BicycleRack.Assertion)} FROM Inventory INNER JOIN BicycleRack ON " +
                $"Inventory.{nameof(BicycleRack.Id)} = BicycleRack.{nameof(BicycleRack.InventoryId)} " +
                $"WHERE BicycleRack.{nameof(BicycleRack.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new BicycleRack(reader.GetInt32($"{nameof(BicycleRack.Id)}"), reader.GetInt32($"{nameof(BicycleRack.InventoryId)}"),
                        reader.GetString($"{nameof(BicycleRack.Title)}"),
                        reader.GetInt32($"{nameof(BicycleRack.Amount)}"), reader.GetDecimal($"{nameof(BicycleRack.Revenue)}"),
                        reader.GetInt32($"{nameof(BicycleRack.TotalRentDuration)}"), reader.GetDecimal($"{nameof(BicycleRack.MonetaryValue)}"),
                        reader.GetInt32($"{nameof(BicycleRack.BikeLimit)}"), reader.GetDouble($"{nameof(BicycleRack.LiftPower)}"),
                        (BicycleRack.AssertionType)Enum.Parse(typeof(BicycleRack.AssertionType), reader.GetString($"{nameof(BicycleRack.Assertion)}")));
                }
                return null;
            }
        }
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        public IList<BicycleRack> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT BicycleRack.{nameof(BicycleRack.InventoryId)}, BicycleRack.{nameof(BicycleRack.Id)}, Inventory.{nameof(BicycleRack.Title)}, " +
                $"Inventory.{nameof(BicycleRack.Amount)}, Inventory.{nameof(BicycleRack.Revenue)}, " +
                $"Inventory.{nameof(BicycleRack.TotalRentDuration)}, Inventory.{nameof(BicycleRack.MonetaryValue)}, " +
                $"BicycleRack.{nameof(BicycleRack.BikeLimit)}, BicycleRack.{nameof(BicycleRack.LiftPower)}, " +
                $"BicycleRack.{nameof(BicycleRack.Assertion)} FROM Inventory INNER JOIN BicycleRack ON " +
                $"Inventory.{nameof(BicycleRack.Id)} = BicycleRack.{nameof(BicycleRack.InventoryId)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<BicycleRack> items = new List<BicycleRack>();
                while (reader.Read())
                {

                    items.Add(new BicycleRack
                    {
                        InventoryId = reader.GetInt32($"{nameof(BicycleRack.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(BicycleRack.Id)}"),
                        Title = reader.GetString($"{nameof(BicycleRack.Title)}"),
                        Amount = reader.GetInt32($"{nameof(BicycleRack.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(BicycleRack.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(BicycleRack.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(BicycleRack.MonetaryValue)}"),
                        BikeLimit = reader.GetInt32($"{nameof(BicycleRack.BikeLimit)}"),
                        LiftPower = reader.GetDouble($"{nameof(BicycleRack.LiftPower)}"),
                        Assertion = (BicycleRack.AssertionType)Enum.Parse(typeof(BicycleRack.AssertionType), reader.GetString($"{nameof(BicycleRack.Assertion)}"))
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
        public bool Update(object id, BicycleRack updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE Inventory INNER JOIN BicycleRack ON BicycleRack.{nameof(BicycleRack.InventoryId)} = Inventory.{nameof(BicycleRack.Id)} " +
                $"SET {nameof(BicycleRack.Title)}=@{nameof(BicycleRack.Title)},{nameof(BicycleRack.Amount)}=@{nameof(BicycleRack.Amount)}," +
                $"{nameof(BicycleRack.Revenue)}=@{nameof(BicycleRack.Revenue)},{nameof(BicycleRack.TotalRentDuration)}=@{nameof(BicycleRack.TotalRentDuration)}," +
                $"{nameof(BicycleRack.MonetaryValue)}=@{nameof(BicycleRack.MonetaryValue)},{nameof(BicycleRack.BikeLimit)}=@{nameof(BicycleRack.BikeLimit)}," +
                $"{nameof(BicycleRack.LiftPower)}=@{nameof(BicycleRack.LiftPower)},{nameof(BicycleRack.Assertion)}=@{nameof(BicycleRack.Assertion)} " +
                $"WHERE Inventory.{nameof(BicycleRack.Id)} = @{nameof(BicycleRack.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.Title)}", updatedDataObject.Title);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.Amount)}", updatedDataObject.Amount);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.Revenue)}", updatedDataObject.Revenue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.TotalRentDuration)}", updatedDataObject.TotalRentDuration);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.MonetaryValue)}", updatedDataObject.MonetaryValue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.BikeLimit)}", updatedDataObject.BikeLimit);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.LiftPower)}", updatedDataObject.LiftPower);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.Assertion)}", updatedDataObject.Assertion);
                sqlCommand.Parameters.AddWithValue($"@{nameof(BicycleRack.Id)}", id);
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
