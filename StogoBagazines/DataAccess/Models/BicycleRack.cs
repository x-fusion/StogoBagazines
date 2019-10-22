using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing Crossbar object
    /// </summary>
    public class BicycleRack : InventoryBase
    {
        /// <summary>
        /// Limit of how many bikes rack can hold
        /// </summary>
        public int BikeLimit { get; set; }
        /// <summary>
        /// Weight limit (kg)
        /// </summary>
        public double LiftPower { get; set; }
        /// <summary>
        /// Rack assertion type
        /// </summary>
        public AssertionType Assertion { get; set; }
        /// <summary>
        /// Local custructor with custom descriptors
        /// </summary>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="value">Monetary value of item</param>
        /// <param name="limit">Maximum bicycle amount supported</param>
        /// <param name="power">Maximum weight supported</param>
        /// <param name="assertion">Way of how rack can be asserted</param>
        public BicycleRack(string title, int amount, decimal value, int limit, double power, AssertionType assertion) : base(title, amount, value)
        {
            BikeLimit = limit;
            LiftPower = power;
            Assertion = assertion;
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
        /// <param name="limit">Maximum bicycle amount supported</param>
        /// <param name="power">Maximum weight supported</param>
        /// <param name="assertion">Way of how rack can be asserted</param>
        public BicycleRack(int id, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value, int limit, double power, AssertionType assertion) : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
            BikeLimit = limit;
            LiftPower = power;
            Assertion = assertion;
        }
        /// <summary>
        /// Ways of how rack can be asserted
        /// </summary>
        public enum AssertionType
        {
            Roof,
            Hook,
            Other
        }
    }
}
