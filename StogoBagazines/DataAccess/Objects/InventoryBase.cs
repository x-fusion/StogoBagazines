using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Objects
{
    /// <summary>
    /// Base class of warehouse items
    /// </summary>
    public class InventoryBase
    {
        /// <summary>
        /// Identification key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Item's title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Count of particular item in warehouse
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Price for renting purposes
        /// </summary>
        public decimal RentPrice => decimal.Multiply(MonetaryValue, MonetaryValue / 10);
        /// <summary>
        /// Revenue generated during rentals
        /// </summary>
        public decimal Revenue { get; set; }
        /// <summary>
        /// Days spent at rent
        /// </summary>
        public int TotalRentDuration { get; set; }
        /// <summary>
        /// Price used for selling purposes
        /// </summary>
        public decimal MonetaryValue { get; set; }
        /// <summary>
        /// Constructor for local object
        /// </summary>
        /// <param name="title">Title of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="value">Monetary value of item</param>
        public InventoryBase(string title, int amount, decimal value)
        {
            Title = title;
            Amount = amount;
            MonetaryValue = value;
            Revenue = 0;
            TotalRentDuration = 0;
        }
        /// <summary>
        /// Constructor for repository object
        /// </summary>
        /// <param name="id">Item's identification key in repository</param>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="totalRevenue">Item's revenue generated during rentals</param>
        /// <param name="totalRentDuration">Item's days spent at rent</param>
        /// <param name="value">Monetary value of item</param>
        public InventoryBase(int id, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value)
        {
            Id = id;
            Title = title;
            Amount = amount;
            MonetaryValue = value;
            Revenue = totalRevenue;
            TotalRentDuration = totalRentDuration;
        }
        /// <summary>
        /// Updates object with rent information
        /// </summary>
        /// <param name="amount">Amount of items is going to be rented</param>
        /// <param name="duration">Amount of days items going to spent in rent</param>
        /// <returns>If such rent is possible, local object is updated on True</returns>
        public bool Rent(int amount, int duration)
        {
            if(Amount - amount < 0)
            {
                return false;
            }
            Amount -= amount;
            TotalRentDuration += amount * duration;
            return true;
        }
    }
}
