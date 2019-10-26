using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing Bicycle rack object
    /// </summary>
    public class BicycleRack : InventoryBase, IValidatableObject
    {
        /// <summary>
        /// Database reference to parent entry
        /// </summary>
        public int InventoryId { get; set; }
        /// <summary>
        /// Limit of how many bikes rack can hold
        /// </summary>
        [Required(ErrorMessage = "Bike limit is manditory field")]
        [Range(1, 4, ErrorMessage = "Bike limit is between 1 and 4")]
        public int BikeLimit { get; set; }
        /// <summary>
        /// Weight limit (kg)
        /// </summary>
        [Required(ErrorMessage = "Lift power is manditory field")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid lift power provided")]
        public double LiftPower { get; set; }
        /// <summary>
        /// Rack assertion type
        /// </summary>
        [Required(ErrorMessage = "Assertion type is manditory field")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AssertionType Assertion { get; set; }
        /// <summary>
        /// Constructor used in serialization
        /// </summary>
        public BicycleRack() { }
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
        /// <param name="inventoryId">Item's base object identification key in repository</param>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="totalRevenue">Item's revenue generated during rentals</param>
        /// <param name="totalRentDuration">Item's days spent at rent</param>
        /// <param name="value">Monetary value of item</param>
        /// <param name="limit">Maximum bicycle amount supported</param>
        /// <param name="power">Maximum weight supported</param>
        /// <param name="assertion">Way of how rack can be asserted</param>
        public BicycleRack(int id, int inventoryId, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value, int limit, double power, AssertionType assertion) : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
            BikeLimit = limit;
            LiftPower = power;
            Assertion = assertion;
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
            if(LiftPower == 0)
            {
                members.Add(nameof(LiftPower));
                results.Add(new ValidationResult("Lift power cannot be 0", members));
            }
            return results;
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
