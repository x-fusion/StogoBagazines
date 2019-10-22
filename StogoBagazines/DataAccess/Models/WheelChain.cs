using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing WheelChain object
    /// </summary>
    public class WheelChain : InventoryBase
    {
        public string TireDimensions { get; set; }
        public double ChainThickness { get; set; }
        /// <summary>
        /// Local custructor with custom descriptors
        /// </summary>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="value">Monetary value of item</param>
        /// <param name="dimensions">Dimensions of wheel tire in text</param>
        /// <param name="thickness">Thickness of chain</param>
        public WheelChain(string title, int amount, decimal value, string dimensions, double thickness) : base(title, amount, value)
        {
            TireDimensions = dimensions;
            ChainThickness = thickness;
        }
        /// <summary>
        /// Repository object constructor with custom descriptors
        /// </summary>
        /// <param name="id">Item's identification key in repository</param>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="totalRevenue">Item's revenue generated during rentals</param>
        /// <param name="totalRentDuration">Item's days spent at rent</param>
        /// <param name="value">Monetary value of item</param>
        /// <param name="dimensions">Dimensions of wheel tire in text</param>
        /// <param name="thickness">Thickness of chain</param>
        public WheelChain(int id, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value, string dimensions, double thickness) : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
            TireDimensions = dimensions;
            ChainThickness = thickness;
        }
    }
}
 