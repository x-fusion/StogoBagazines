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
        private string InventoryTable => "Inventory";
        private string BicycleRackTable => "BicycleRack";
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
                CommandText = $"INSERT INTO {InventoryTable}({nameof(InventoryBase.Title)},{nameof(InventoryBase.Amount)},{nameof(InventoryBase.Revenue)}," +
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
                    sqlCommand.CommandText = lastInsertCmd;
                    insertedObjectId = sqlCommand.ExecuteScalar();
                    sqlCommand = new MySqlCommand
                    {
                        Connection = Database.Connection,
                        CommandText = $"INSERT INTO {BicycleRackTable}({nameof(BicycleRack.InventoryId)},{nameof(BicycleRack.BikeLimit)}, {nameof(BicycleRack.LiftPower)}," +
                        $"{nameof(BicycleRack.Assertion)}) VALUES(@{nameof(BicycleRack.InventoryId)},@{nameof(BicycleRack.BikeLimit)},@{nameof(BicycleRack.LiftPower)},@{nameof(BicycleRack.Assertion)});"
                    };
                    sqlCommand.Prepare();
                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        sqlCommand.CommandText = lastInsertCmd;
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
                CommandText = $"SELECT {InventoryTable}.{nameof(InventoryBase.Title)}, {InventoryTable}.{nameof(InventoryBase.Amount)}, " +
                $"{InventoryTable}.{nameof(InventoryBase.Revenue)}, {InventoryTable}.{nameof(InventoryBase.TotalRentDuration)}, {InventoryTable}.{nameof(InventoryBase.MonetaryValue)}, " +
                $"{BicycleRackTable}.{nameof(BicycleRack.Id)}, {BicycleRackTable}.{nameof(BicycleRack.BikeLimit)}, {BicycleRackTable}.{nameof(BicycleRack.LiftPower)}," +
                $" {BicycleRackTable}.{nameof(BicycleRack.Assertion)} FROM {InventoryTable} INNER JOIN {BicycleRackTable} ON " +
                $"{InventoryTable}.{nameof(InventoryBase.Id)} = {BicycleRackTable}.{nameof(BicycleRack.InventoryId)} " +
                $"WHERE {BicycleRackTable}.{nameof(BicycleRack.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new BicycleRack(reader.GetInt32($"{BicycleRackTable}.{nameof(BicycleRack.Id)}"), reader.GetString($"{InventoryTable}.{nameof(InventoryBase.Title)}"),
                        reader.GetInt32($"{InventoryTable}.{nameof(InventoryBase.Amount)}"), reader.GetDecimal($"{InventoryTable}.{nameof(InventoryBase.Revenue)}"),
                        reader.GetInt32($"{InventoryTable}.{nameof(InventoryBase.TotalRentDuration)}"), reader.GetDecimal($"{InventoryTable}.{nameof(InventoryBase.MonetaryValue)}"),
                        reader.GetInt32($"{BicycleRackTable}.{nameof(BicycleRack.BikeLimit)}"), reader.GetDouble($"{BicycleRackTable}.{nameof(BicycleRack.LiftPower)}"),
                        (BicycleRack.AssertionType)Enum.Parse(typeof(BicycleRack.AssertionType), reader.GetString($"{BicycleRackTable}.{nameof(BicycleRack.Assertion)}")));
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
