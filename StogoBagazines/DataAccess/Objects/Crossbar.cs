using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Objects
{
    public class Crossbar : InventoryBase
    {
        public Crossbar(string title, int amount, decimal value)
            : base(title, amount, value)
        {

        }

        public Crossbar(int id, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value)
            : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {

        }
    }
}
