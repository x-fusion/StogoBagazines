using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing RoofRack object
    /// </summary>
    public class RoofRack : InventoryBase
    {
        /// <summary>
        /// Describes roof rack way of opening
        /// </summary>
        public OpeningType Opening { get; set; }
        /// <summary>
        /// Weight limit (kg)
        /// </summary>
        public double LiftPower { get; set; }
        /// <summary>
        /// Indicates if roof rack has lock
        /// </summary>
        public bool IsLockable { get; set; }
        /// <summary>
        /// Describes weight of roof rack itself (kg)
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// Describes exterior of roof rack
        /// </summary>
        public string AppearenceDescription { get; set; }
        /// <summary>
        /// Local custructor with custom descriptors
        /// </summary>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="value">Monetary value of item</param>
        /// <param name="openingType">Roof rack opening type</param>
        /// <param name="limit">Maximum weight supported</param>
        /// <param name="lockable">Does roof rack has lock</param>
        /// <param name="weight">Weight of roof rack</param>
        /// <param name="exteriorDescription">Exterior description</param>
        public RoofRack(string title, int amount, decimal value, OpeningType openingType, double limit, bool lockable, double weight, string exteriorDescription)
            : base(title, amount, value)
        {
            Opening = openingType;
            LiftPower = limit;
            IsLockable = lockable;
            Weight = weight;
            AppearenceDescription = exteriorDescription;
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
        /// <param name="openingType">Roof rack opening type</param>
        /// <param name="limit">Maximum weight supported</param>
        /// <param name="lockable">Does roof rack has lock</param>
        /// <param name="weight">Weight of roof rack</param>
        /// <param name="exteriorDescription">Exterior description</param>
        public RoofRack(int id, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value, OpeningType openingType, double limit, bool lockable, double weight, string exteriorDescription)
            : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
            Opening = openingType;
            LiftPower = limit;
            IsLockable = lockable;
            Weight = weight;
            AppearenceDescription = exteriorDescription;
        }
        /// <summary>
        /// Ways of how Roof rack can be opened
        /// </summary>
        public enum OpeningType
        {
            TwoSided,
            OneSided,
            RemovableTop
        }
    }
}
