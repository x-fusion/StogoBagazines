using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing RoofRack object
    /// </summary>
    public class RoofRack : InventoryBase, IValidatableObject
    {
        /// <summary>
        /// Database reference to parent entry
        /// </summary>
        public int InventoryId { get; set; }
        /// <summary>
        /// Describes roof rack way of opening
        /// </summary>
        [Required(ErrorMessage = "Way of opening is manditory field")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OpeningType Opening { get; set; }
        /// <summary>
        /// Weight limit (kg)
        /// </summary>
        [Required(ErrorMessage = "Lift power is manditory field")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid lift power provided")]
        public double LiftPower { get; set; }
        /// <summary>
        /// Indicates if roof rack has lock
        /// </summary>
        [Required(ErrorMessage = "IsLockable is manditory field")]
        public bool IsLockable { get; set; }
        /// <summary>
        /// Describes weight of roof rack itself (kg)
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Invalid weight provided")]
        public double Weight { get; set; }
        /// <summary>
        /// Describes exterior of roof rack
        /// </summary>
        [StringLength(255, ErrorMessage = "Length has to be between 0 and 255 symbols", MinimumLength = 0)]
        [DataType(DataType.Text, ErrorMessage = "Invalid Appearence description provided")]
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
        /// <param name="inventoryId">Item's base object identification key in repository</param>
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
        public RoofRack(int id, int inventoryId, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value, OpeningType openingType, double limit, bool lockable, double weight, string exteriorDescription)
            : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
            Opening = openingType;
            LiftPower = limit;
            IsLockable = lockable;
            Weight = weight;
            AppearenceDescription = exteriorDescription;
            InventoryId = inventoryId;
        }
        /// <summary>
        /// Custom property and object level validation
        /// </summary>
        /// <param name="validationContext">Properties and their values</param>
        /// <returns>Fields and their's errors</returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();
            List<string> members = new List<string>();
            if (LiftPower == 0)
            {
                members.Add(nameof(LiftPower));
                results.Add(new ValidationResult("Lift power cannot be 0", members));
            }
            return results;
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
