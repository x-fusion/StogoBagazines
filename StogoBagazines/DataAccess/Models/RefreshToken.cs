using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public int UserId { get; set; }

        public RefreshToken()
        {

        }
    }
}
