using StogoBagazines.DataAccess.Interfaces;
using StogoBagazines.DataAccess.Models;
using StogoBagazines.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StogoBagazines.DataAccess;
using MySql.Data.MySqlClient;

namespace StogoBagazines.Services
{
    public class RefreshTokenService : Repository, IRepository<RefreshToken>
    {
        public RefreshTokenService(Database database) : base(database)
        {

        }
        public object Create(RefreshToken dataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"INSERT INTO RefreshToken({nameof(RefreshToken.JwtId)},{nameof(RefreshToken.CreationDate)},{nameof(RefreshToken.ExpirationDate)},{nameof(RefreshToken.Used)}," +
                $"{nameof(RefreshToken.Invalidated)},{nameof(RefreshToken.Token)},{nameof(RefreshToken.UserId)}) VALUES(@{nameof(RefreshToken.JwtId)},@{nameof(RefreshToken.CreationDate)},@{nameof(RefreshToken.ExpirationDate)}," +
                $"@{nameof(RefreshToken.Used)},@{nameof(RefreshToken.Invalidated)},@{nameof(RefreshToken.Token)},@{nameof(RefreshToken.UserId)});"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.JwtId)}", dataObject.JwtId);
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.CreationDate)}", dataObject.CreationDate);
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.ExpirationDate)}", dataObject.ExpirationDate);
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Used)}", dataObject.Used);
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Invalidated)}", dataObject.Invalidated);
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Token)}", dataObject.Token);
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.UserId)}", dataObject.UserId);
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

        public bool Delete(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"DELETE FROM RefreshToken WHERE {nameof(RefreshToken.Id)}=@{nameof(RefreshToken.Id)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Id)}", id);
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

        public bool Exists(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT * FROM RefreshToken WHERE {nameof(RefreshToken.Id)}=@{nameof(RefreshToken.Id)};"
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

        public RefreshToken Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT RefreshToken.{nameof(RefreshToken.Id)}, RefreshToken.{nameof(RefreshToken.JwtId)}, " +
                $"RefreshToken.{nameof(RefreshToken.CreationDate)}, RefreshToken.{nameof(RefreshToken.ExpirationDate)}, " +
                $"RefreshToken.{nameof(RefreshToken.Used)}, RefreshToken.{nameof(RefreshToken.Invalidated)}, " +
                $"RefreshToken.{nameof(RefreshToken.Token)}, RefreshToken.{nameof(RefreshToken.UserId)} " +
                $"FROM RefreshToken WHERE RefreshToken.{nameof(RefreshToken.Id)} = @{nameof(RefreshToken.Id)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Id)}", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new RefreshToken
                    { 
                        Id = reader.GetInt32($"{nameof(RefreshToken.Id)}"),
                        JwtId = reader.GetString($"{nameof(RefreshToken.JwtId)}"),
                        CreationDate = reader.GetDateTime($"{nameof(RefreshToken.CreationDate)}"),
                        ExpirationDate = reader.GetDateTime($"{nameof(RefreshToken.ExpirationDate)}"),
                        Used = reader.GetBoolean($"{nameof(RefreshToken.Used)}"),
                        Invalidated = reader.GetBoolean($"{nameof(RefreshToken.Invalidated)}"),
                        Token = reader.GetString($"{nameof(RefreshToken.Token)}"),
                        UserId = reader.GetInt32($"{nameof(RefreshToken.Invalidated)}")
                    };
                }
                return null;
            }
        }

        public RefreshToken ReadByUserId(int id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT RefreshToken.{nameof(RefreshToken.Id)}, RefreshToken.{nameof(RefreshToken.JwtId)}, " +
                $"RefreshToken.{nameof(RefreshToken.CreationDate)}, RefreshToken.{nameof(RefreshToken.ExpirationDate)}, " +
                $"RefreshToken.{nameof(RefreshToken.Used)}, RefreshToken.{nameof(RefreshToken.Invalidated)}, " +
                $"RefreshToken.{nameof(RefreshToken.Token)}, RefreshToken.{nameof(RefreshToken.UserId)} " +
                $"FROM RefreshToken WHERE RefreshToken.{nameof(RefreshToken.UserId)} = @{nameof(RefreshToken.UserId)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.UserId)}", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new RefreshToken
                    {
                        Id = reader.GetInt32($"{nameof(RefreshToken.Id)}"),
                        JwtId = reader.GetString($"{nameof(RefreshToken.JwtId)}"),
                        CreationDate = reader.GetDateTime($"{nameof(RefreshToken.CreationDate)}"),
                        ExpirationDate = reader.GetDateTime($"{nameof(RefreshToken.ExpirationDate)}"),
                        Used = reader.GetBoolean($"{nameof(RefreshToken.Used)}"),
                        Invalidated = reader.GetBoolean($"{nameof(RefreshToken.Invalidated)}"),
                        Token = reader.GetString($"{nameof(RefreshToken.Token)}"),
                        UserId = reader.GetInt32($"{nameof(RefreshToken.Invalidated)}")
                    };
                }
                return null;
            }
        }

        public RefreshToken Read(string jti)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT RefreshToken.{nameof(RefreshToken.Id)}, RefreshToken.{nameof(RefreshToken.JwtId)}, " +
                $"RefreshToken.{nameof(RefreshToken.CreationDate)}, RefreshToken.{nameof(RefreshToken.ExpirationDate)}, " +
                $"RefreshToken.{nameof(RefreshToken.Used)}, RefreshToken.{nameof(RefreshToken.Invalidated)}, " +
                $"RefreshToken.{nameof(RefreshToken.Token)}, RefreshToken.{nameof(RefreshToken.UserId)} " +
                $"FROM RefreshToken WHERE RefreshToken.{nameof(RefreshToken.JwtId)} = @JwtId;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.JwtId)}", jti);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new RefreshToken
                    {
                        Id = reader.GetInt32($"{nameof(RefreshToken.Id)}"),
                        JwtId = reader.GetString($"{nameof(RefreshToken.JwtId)}"),
                        CreationDate = reader.GetDateTime($"{nameof(RefreshToken.CreationDate)}"),
                        ExpirationDate = reader.GetDateTime($"{nameof(RefreshToken.ExpirationDate)}"),
                        Used = reader.GetBoolean($"{nameof(RefreshToken.Used)}"),
                        Invalidated = reader.GetBoolean($"{nameof(RefreshToken.Invalidated)}"),
                        Token = reader.GetString($"{nameof(RefreshToken.Token)}"),
                        UserId = reader.GetInt32($"{nameof(RefreshToken.UserId)}")
                    };
                }
                return null;
            }
        }

        public IList<RefreshToken> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT * FROM {nameof(RefreshToken)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<RefreshToken> tokens = new List<RefreshToken>();
                while (reader.Read())
                {
                    tokens.Add(new RefreshToken
                    {
                        Id = reader.GetInt32($"{nameof(RefreshToken.Id)}"),
                        JwtId = reader.GetString($"{nameof(RefreshToken.JwtId)}"),
                        CreationDate = reader.GetDateTime($"{nameof(RefreshToken.CreationDate)}"),
                        ExpirationDate = reader.GetDateTime($"{nameof(RefreshToken.ExpirationDate)}"),
                        Used = reader.GetBoolean($"{nameof(RefreshToken.Used)}"),
                        Invalidated = reader.GetBoolean($"{nameof(RefreshToken.Invalidated)}"),
                        Token = reader.GetString($"{nameof(RefreshToken.Token)}"),
                        UserId = reader.GetInt32($"{nameof(RefreshToken.Invalidated)}")
                    });
                }
                return tokens;
            }
        }

        public bool Update(object id, RefreshToken updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE {nameof(RefreshToken)} SET {nameof(RefreshToken.JwtId)}=@{nameof(RefreshToken.JwtId)},{nameof(RefreshToken.CreationDate)}=@{nameof(RefreshToken.CreationDate)}," +
                $"{nameof(RefreshToken.ExpirationDate)}=@{nameof(RefreshToken.ExpirationDate)},{nameof(RefreshToken.Used)}=@{nameof(RefreshToken.Used)},{nameof(RefreshToken.Invalidated)}=@{nameof(RefreshToken.Invalidated)} " +
                $"{nameof(RefreshToken.Token)}=@{nameof(RefreshToken.Token)} WHERE {nameof(RefreshToken)}.{nameof(RefreshToken.Id)} = @{nameof(RefreshToken.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.JwtId)}", updatedDataObject.JwtId);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.CreationDate)}", updatedDataObject.CreationDate);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.ExpirationDate)}", updatedDataObject.ExpirationDate);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Used)}", updatedDataObject.Used);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Invalidated)}", updatedDataObject.Invalidated);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Token)}", updatedDataObject.Token);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Id)}", id);
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

        public bool UpdateByUserId(int id, object tokenId, RefreshToken updatedDataObject)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE {nameof(RefreshToken)} SET {nameof(RefreshToken.JwtId)}=@{nameof(RefreshToken.JwtId)},{nameof(RefreshToken.CreationDate)}=@{nameof(RefreshToken.CreationDate)}," +
                $"{nameof(RefreshToken.ExpirationDate)}=@{nameof(RefreshToken.ExpirationDate)},{nameof(RefreshToken.Used)}=@{nameof(RefreshToken.Used)},{nameof(RefreshToken.Invalidated)}=@{nameof(RefreshToken.Invalidated)} " +
                $"{nameof(RefreshToken.Token)}=@{nameof(RefreshToken.Token)} WHERE {nameof(RefreshToken)}.{nameof(RefreshToken.UserId)} = @{nameof(RefreshToken.UserId)} AND {nameof(RefreshToken)}.{nameof(RefreshToken.Id)} = @{nameof(RefreshToken.Id)};"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.JwtId)}", updatedDataObject.JwtId);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.CreationDate)}", updatedDataObject.CreationDate);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.ExpirationDate)}", updatedDataObject.ExpirationDate);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Used)}", updatedDataObject.Used);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Invalidated)}", updatedDataObject.Invalidated);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Token)}", updatedDataObject.Token);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.UserId)}", id);
                sqlCommand.Parameters.AddWithValue($"@{nameof(RefreshToken.Id)}", tokenId);
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
