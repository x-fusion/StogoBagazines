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
    public class CrossbarRepository : Repository, IRepository<Crossbar>
    {
        public CrossbarRepository(Database database) : base(database)
        {

        }
        /// <summary>
        /// Creates and inserts entry in database
        /// </summary>
        /// <param name="dataObject">Entry reference</param>
        /// <returns>Id of entry which was created in database</returns>
        public object Create(Crossbar dataObject)
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
                        CommandText = $"INSERT INTO Crossbar({nameof(Crossbar.InventoryId)}) VALUES(@{nameof(Crossbar.InventoryId)});"
                    };
                    sqlCommand.Prepare();
                    sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.InventoryId)}", dataObject.InventoryId);
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
        public Crossbar Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT Crossbar.{nameof(Crossbar.InventoryId)}, Crossbar.{nameof(Crossbar.Id)}, Inventory.{nameof(Crossbar.Title)}, " +
                $"Inventory.{nameof(Crossbar.Amount)}, Inventory.{nameof(Crossbar.Revenue)}, " +
                $"Inventory.{nameof(Crossbar.TotalRentDuration)}, Inventory.{nameof(Crossbar.MonetaryValue)} " +
                $"FROM Inventory INNER JOIN Crossbar ON " +
                $"Inventory.{nameof(Crossbar.Id)} = Crossbar.{nameof(Crossbar.InventoryId)} " +
                $"WHERE Crossbar.{nameof(Crossbar.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new Crossbar(reader.GetInt32($"{nameof(Crossbar.Id)}"), reader.GetInt32($"{nameof(Crossbar.InventoryId)}"),
                        reader.GetString($"{nameof(Crossbar.Title)}"),
                        reader.GetInt32($"{nameof(Crossbar.Amount)}"), reader.GetDecimal($"{nameof(Crossbar.Revenue)}"),
                        reader.GetInt32($"{nameof(Crossbar.TotalRentDuration)}"), reader.GetDecimal($"{nameof(Crossbar.MonetaryValue)}"));
                }
                return null;
            }
        }
        /// <summary>
        /// Returns all entries from entity
        /// </summary>
        /// <returns>List of entries</returns>
        public IList<Crossbar> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT Crossbar.{nameof(Crossbar.InventoryId)}, Crossbar.{nameof(Crossbar.Id)}, Inventory.{nameof(Crossbar.Title)}, " +
                $"Inventory.{nameof(Crossbar.Amount)}, Inventory.{nameof(Crossbar.Revenue)}, " +
                $"Inventory.{nameof(Crossbar.TotalRentDuration)}, Inventory.{nameof(Crossbar.MonetaryValue)} " +
                $"FROM Inventory INNER JOIN Crossbar ON " +
                $"Inventory.{nameof(Crossbar.Id)} = Crossbar.{nameof(Crossbar.InventoryId)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<Crossbar> items = new List<Crossbar>();
                while (reader.Read())
                {
                    items.Add(new Crossbar
                    {
                        InventoryId = reader.GetInt32($"{nameof(Crossbar.InventoryId)}"),
                        Id = reader.GetInt32($"{nameof(Crossbar.Id)}"),
                        Title = reader.GetString($"{nameof(Crossbar.Title)}"),
                        Amount = reader.GetInt32($"{nameof(Crossbar.Amount)}"),
                        Revenue = reader.GetDecimal($"{nameof(Crossbar.Revenue)}"),
                        TotalRentDuration = reader.GetInt32($"{nameof(Crossbar.TotalRentDuration)}"),
                        MonetaryValue = reader.GetDecimal($"{nameof(Crossbar.MonetaryValue)}")
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
        public bool Update(object id, Crossbar updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE Inventory INNER JOIN Crossbar ON Crossbar.{nameof(Crossbar.InventoryId)} = Inventory.{nameof(Crossbar.Id)} " +
                $"SET {nameof(Crossbar.Title)}=@{nameof(Crossbar.Title)},{nameof(Crossbar.Amount)}=@{nameof(Crossbar.Amount)}," +
                $"{nameof(Crossbar.Revenue)}=@{nameof(Crossbar.Revenue)},{nameof(Crossbar.TotalRentDuration)}=@{nameof(Crossbar.TotalRentDuration)}," +
                $"{nameof(Crossbar.MonetaryValue)}=@{nameof(Crossbar.MonetaryValue)} " +
                $"WHERE Inventory.{nameof(Crossbar.Id)} = @{nameof(Crossbar.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.Title)}", updatedDataObject.Title);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.Amount)}", updatedDataObject.Amount);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.Revenue)}", updatedDataObject.Revenue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.TotalRentDuration)}", updatedDataObject.TotalRentDuration);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.MonetaryValue)}", updatedDataObject.MonetaryValue);
                sqlCommand.Parameters.AddWithValue($"@{nameof(Crossbar.Id)}", id);
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
