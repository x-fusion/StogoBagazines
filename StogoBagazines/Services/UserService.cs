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

namespace StogoBagazines.Services
{
    public interface IUserService
    {
        User Authetificate(string email, string password);
    }
    public class UserService : Repository, IUserService, IRepository<User>
    {
        private readonly JwtOptions jwtOptions;

        public UserService(JwtOptions jwtOptions, Database database) : base(database)
        {
            this.jwtOptions = jwtOptions;
        }

        public User Authetificate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public object Create(User dataObject)
        {
            dataObject.Password = EncryptionService.Encrypt(dataObject.Password, jwtOptions.Secret);
            MySqlCommand sqlCommand = new MySqlCommand
            {
                Connection = Database.Connection,
                CommandText = $"INSERT INTO User({nameof(InventoryBase.Title)},{nameof(InventoryBase.Amount)},{nameof(InventoryBase.Revenue)}," +
    $"{nameof(InventoryBase.TotalRentDuration)},{nameof(InventoryBase.MonetaryValue)}) VALUES(@{nameof(InventoryBase.Title)},@{nameof(InventoryBase.Amount)}," +
    $"@{nameof(InventoryBase.Revenue)},@{nameof(InventoryBase.TotalRentDuration)},@{nameof(InventoryBase.MonetaryValue)});"
            };
        }

        public bool Delete(object id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string email)
        {
            throw new NotImplementedException();
        }

        public User Read(object id)
        {
            throw new NotImplementedException();
        }

        public IList<User> ReadAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(object id, User updatedDataObject)
        {
            throw new NotImplementedException();
        }
    }
}
