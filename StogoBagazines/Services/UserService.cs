using StogoBagazines.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions;
using StogoBagazines.Configuration;
using StogoBagazines.DataAccess.Interfaces;
using StogoBagazines.DataAccess;
using StogoBagazines.DataAccess.Repositories;
using MySql.Data.MySqlClient;
using StogoBagazines.ApiRequests;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StogoBagazines.Services
{
    public interface IUserService
    {
        User Authetificate(string email, string password);
    }
    public class UserService : Repository, IUserService, IRepository<User>
    {
        private readonly JwtOptions jwtOptions;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly RefreshTokenService refreshTokenService;

        public UserService(JwtOptions jwtOptions, Database database, TokenValidationParameters validationParameters) : base(database)
        {
            this.jwtOptions = jwtOptions;
            refreshTokenService = new RefreshTokenService(database);
            this.tokenValidationParameters = validationParameters;
        }

        public User Authetificate(string email, string password)
        {
            password = EncryptionService.Encrypt(password, jwtOptions.Secret);
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT User.{nameof(User.Id)},User.{nameof(User.FirstName)},User.{nameof(User.LastName)}," +
                $"User.{nameof(User.Email)},User.{nameof(User.Role)} FROM User " +
                $"WHERE User.{nameof(User.Email)} = @{nameof(User.Email)} AND User.`{nameof(User.Password)}` = @{nameof(User.Password)};"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"@{nameof(User.Email)}", email);
            sqlCommand.Parameters.AddWithValue($"@{nameof(User.Password)}", password);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {

                    return new User
                    {
                        Id = reader.GetInt32($"{nameof(User.Id)}"),
                        FirstName = reader.GetString($"{nameof(User.FirstName)}"),
                        LastName = reader.GetString($"{nameof(User.LastName)}"),
                        Email = reader.GetString($"{nameof(User.Email)}"),
                        Role = reader.GetString($"{nameof(User.Role)}"),
                    };
                }
                return null;
            }
        }

        public object Create(User dataObject)
        {
            dataObject.Password = EncryptionService.Encrypt(dataObject.Password, jwtOptions.Secret);
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"INSERT INTO User({nameof(User.FirstName)},{nameof(User.LastName)},{nameof(User.Email)}," +
                $"`{nameof(User.Password)}`,{nameof(User.Role)}) VALUES(@{nameof(User.FirstName)},@{nameof(User.LastName)}," +
                $"@{nameof(User.Email)},@{nameof(User.Password)},@{nameof(User.Role)});"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue($"{nameof(User.FirstName)}", dataObject.FirstName);
            sqlCommand.Parameters.AddWithValue($"{nameof(User.LastName)}", dataObject.LastName);
            sqlCommand.Parameters.AddWithValue($"{nameof(User.Email)}", dataObject.Email);
            sqlCommand.Parameters.AddWithValue($"{nameof(User.Password)}", dataObject.Password);
            sqlCommand.Parameters.AddWithValue($"{nameof(User.Role)}", dataObject.Role);
            using (Database.Connection)
            {
                if (sqlCommand.ExecuteNonQuery() == 1)
                {
                    sqlCommand.CommandText = "SELECT LAST_INSERT_ID();";
                    return sqlCommand.ExecuteScalar();
                }
            }
            return -1;
        }

        public bool Delete(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "DELETE FROM User WHERE Id=@Id;"
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

        public bool Exists(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "SELECT * FROM User WHERE Id=@Id;"
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

        public bool Exists(string email)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = "SELECT * FROM User WHERE Email=@Email;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Email", email);
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

        public User Read(object id)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT User.{nameof(User.Id)},User.{nameof(User.FirstName)},User.{nameof(User.LastName)}," +
                $"User.{nameof(User.Email)},User.{nameof(User.Role)} FROM User " +
                $"WHERE User.{nameof(User.Id)} = @Id;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32($"{nameof(User.Id)}"),
                        FirstName = reader.GetString($"{nameof(User.FirstName)}"),
                        LastName = reader.GetString($"{nameof(User.LastName)}"),
                        Email = reader.GetString($"{nameof(User.Email)}"),
                        Role = reader.GetString($"{nameof(User.Role)}"),
                    };
                }
                return null;
            }
        }
        public User Read(string email)
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT User.{nameof(User.Id)},User.{nameof(User.FirstName)},User.{nameof(User.LastName)}," +
                $"User.{nameof(User.Email)},User.{nameof(User.Role)} FROM User " +
                $"WHERE User.{nameof(User.Email)} = @Email;"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            sqlCommand.Parameters.AddWithValue("@Email", email);
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32($"{nameof(User.Id)}"),
                        FirstName = reader.GetString($"{nameof(User.FirstName)}"),
                        LastName = reader.GetString($"{nameof(User.LastName)}"),
                        Email = reader.GetString($"{nameof(User.Email)}"),
                        Role = reader.GetString($"{nameof(User.Role)}"),
                    };
                }
                return null;
            }
        }

        public IList<User> ReadAll()
        {
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"SELECT User.{nameof(User.Id)},User.{nameof(User.FirstName)},User.{nameof(User.LastName)}," +
                $"User.{nameof(User.Email)},User.{nameof(User.Role)} FROM User"
            };
            Database.Connection.Open();
            sqlCommand.Prepare();
            using (Database.Connection)
            {
                using MySqlDataReader reader = sqlCommand.ExecuteReader();
                List<User> items = new List<User>();
                while (reader.Read())
                {
                    items.Add(new User
                    {
                        Id = reader.GetInt32($"{nameof(User.Id)}"),
                        FirstName = reader.GetString($"{nameof(User.FirstName)}"),
                        LastName = reader.GetString($"{nameof(User.LastName)}"),
                        Email = reader.GetString($"{nameof(User.Email)}"),
                        Role = reader.GetString($"{nameof(User.Role)}"),
                    });
                }
                return items;
            }
        }

        public bool Update(object id, User updatedDataObject)
        {
            updatedDataObject.Password = EncryptionService.Encrypt(updatedDataObject.Password, jwtOptions.Secret);
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"UPDATE User SET {nameof(User.FirstName)}=@{nameof(User.LastName)},{nameof(User.LastName)}," +
                $"{nameof(User.Email)}=@{nameof(User.Email)},`{nameof(User.Password)}`=@{nameof(User.Password)}," +
                $"{nameof(User.Role)}=@{nameof(User.Role)} WHERE {nameof(User.Id)}=@{nameof(User.Id)}"
            };
            Database.Connection.Open();
            using (Database.Connection)
            {
                sqlCommand.Prepare();
                sqlCommand.Parameters.AddWithValue($"@{nameof(User.FirstName)}", updatedDataObject.FirstName);
                sqlCommand.Parameters.AddWithValue($"@{nameof(User.LastName)}", updatedDataObject.LastName);
                sqlCommand.Parameters.AddWithValue($"@{nameof(User.Email)}", updatedDataObject.Email);
                sqlCommand.Parameters.AddWithValue($"@{nameof(User.Password)}", updatedDataObject.Password);
                sqlCommand.Parameters.AddWithValue($"@{nameof(User.Role)}", updatedDataObject.Role);
                sqlCommand.Parameters.AddWithValue($"@{nameof(User.Id)}", id);
                MySqlTransaction sqlTransaction = Database.Connection.BeginTransaction();
                if (sqlCommand.ExecuteNonQuery() > 0)
                {
                    sqlTransaction.Commit();
                    return true;
                }
                sqlTransaction.Rollback();
            }
            return false;
        }
        public AuthResponse RefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthResponse
                {
                    Message = "Invalid token",
                    Payload = token
                };
            }

            var expirationDate = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            DateTime expirationDateUtc = new DateTime(DateTime.UnixEpoch.Ticks, DateTimeKind.Utc).AddSeconds(expirationDate);

            if (expirationDateUtc > DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Message = "This token hasn't expired yet.",
                    Payload = expirationDate
                };
            }
            string jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = refreshTokenService.Read(refreshToken);
            if (storedRefreshToken == null)
            {
                return new AuthResponse
                {
                    Message = "This refresh token doesn't exist",
                    Payload = refreshToken
                };
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpirationDate)
            {
                return new AuthResponse
                {
                    Message = "This refresh token has expired",
                    Payload = storedRefreshToken.ExpirationDate
                };
            }
            if (storedRefreshToken.Invalidated)
            {
                return new AuthResponse
                {
                    Message = "This refresh token has been invalidated",
                    Payload = storedRefreshToken
                };
            }
            if (storedRefreshToken.Used)
            {
                return new AuthResponse
                {
                    Message = "This refresh token has been used already",
                    Payload = storedRefreshToken
                };
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResponse
                {
                    Message = "This refresh token doesn't match this Jwt",
                    Payload = storedRefreshToken.JwtId
                };
            }

            storedRefreshToken.Used = true;
            User user = Read((object)validatedToken.Claims.Single(x => x.Type == "id").Value);
            refreshTokenService.Update(storedRefreshToken.Id, storedRefreshToken);

            return GenerateAuthentificationResultForUser(user);
        }
        public AuthResponse GenerateAuthentificationResultForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString()
            };
            refreshTokenService.Create(refreshToken);
            return new AuthResponse
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                Message = "Authenticated"
            };
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = this.tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (SecurityTokenValidationException ex)
            {
                throw ex;
            }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
