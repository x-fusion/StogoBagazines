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
                    insertedObjectId = sqlCommand.ExecuteScalar();
                    sqlCommand = new MySqlCommand
                    {
                        Connection = Database.Connection,
                        CommandText = $"INSERT INTO BicycleRack({nameof(BicycleRack.InventoryId)},{nameof(BicycleRack.BikeLimit)}, {nameof(BicycleRack.LiftPower)}," +
                        $"{nameof(BicycleRack.Assertion)}) VALUES(@{nameof(BicycleRack.InventoryId)},@{nameof(BicycleRack.BikeLimit)},@{nameof(BicycleRack.LiftPower)},@{nameof(BicycleRack.Assertion)});"
                    };
                    sqlCommand.Prepare();
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

        public bool Delete(object id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object id)
        {
            throw new NotImplementedException();
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
                CommandText = $"SELECT Inventory.{nameof(InventoryBase.Title)}, Inventory.{nameof(InventoryBase.Amount)}, " +
                $"Inventory.{nameof(InventoryBase.Revenue)}, Inventory.{nameof(InventoryBase.TotalRentDuration)}, Inventory.{nameof(InventoryBase.MonetaryValue)}, " +
                $"BicycleRack.{nameof(BicycleRack.Id)}, BicycleRack.{nameof(BicycleRack.BikeLimit)}, BicycleRack.{nameof(BicycleRack.LiftPower)}," +
                $" BicycleRack.{nameof(BicycleRack.Assertion)} FROM Inventory INNER JOIN BicycleRack ON " +
                $"Inventory.{nameof(InventoryBase.Id)} = BicycleRack.{nameof(BicycleRack.InventoryId)} " +
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
                    return new BicycleRack(reader.GetInt32($"BicycleRack.{nameof(BicycleRack.Id)}"), reader.GetString($"Inventory.{nameof(InventoryBase.Title)}"),
                        reader.GetInt32($"Inventory.{nameof(InventoryBase.Amount)}"), reader.GetDecimal($"Inventory.{nameof(InventoryBase.Revenue)}"),
                        reader.GetInt32($"Inventory.{nameof(InventoryBase.TotalRentDuration)}"), reader.GetDecimal($"Inventory.{nameof(InventoryBase.MonetaryValue)}"),
                        reader.GetInt32($"BicycleRack.{nameof(BicycleRack.BikeLimit)}"), reader.GetDouble($"BicycleRack.{nameof(BicycleRack.LiftPower)}"),
                        (BicycleRack.AssertionType)Enum.Parse(typeof(BicycleRack.AssertionType), reader.GetString($"BicycleRack.{nameof(BicycleRack.Assertion)}")));
                }
                return null;
            }
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
