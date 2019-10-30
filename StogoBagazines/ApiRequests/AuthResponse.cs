using StogoBagazines.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.ApiRequests
{
    public class AuthResponse : Response
    {
        public string Token { get; set; }
    }
}
