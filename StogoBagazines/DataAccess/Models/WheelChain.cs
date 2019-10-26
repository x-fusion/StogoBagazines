using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing WheelChain object
    /// </summary>
    public class WheelChain : InventoryBase, IValidatableObject
    {
        /// <summary>
        /// Database reference to parent entry
        /// </summary>
        public int InventoryId { get; set; }
        /// <summary>
        /// Tire dimensions written in plain text
        /// </summary>
        [StringLength(255, ErrorMessage = "Length has to be between 0 and 255 symbols", MinimumLength = 0)]
        [DataType(DataType.Text, ErrorMessage = "Invalid Tire dimensions provided")]
        public string TireDimensions { get; set; }
        /// <summary>
        /// Chain thickness in mm's
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Not valid double value")]
        public double ChainThickness { get; set; }
        /// <summary>
        /// On which vehicle type wheelchain is applicable
        /// </summary>
        [Required(ErrorMessage = "Vehicle type is manditory field")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VehicleType Type { get; set; }
        /// <summary>
        /// Constructor used in serialization
        /// </summary>
        public WheelChain() { }
        /// <summary>
        /// Local custructor with custom descriptors
        /// </summary>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="value">Monetary value of item</param>
        /// <param name="dimensions">Dimensions of wheel tire in text</param>
        /// <param name="thickness">Thickness of chain</param>
        /// <param name="vehicleType">Vehicle type on which chain is applicable</param>
        public WheelChain(string title, int amount, decimal value, string dimensions, double thickness, VehicleType vehicleType) : base(title, amount, value)
        {
            TireDimensions = dimensions;
            ChainThickness = thickness;
            Type = vehicleType;
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
        /// <param name="dimensions">Dimensions of wheel tire in text</param>
        /// <param name="thickness">Thickness of chain</param>
        /// <param name="vehicleType">Vehicle type on which chain is applicable</param>
        public WheelChain(int id, int inventoryId, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value, string dimensions, double thickness, VehicleType vehicleType) : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
            TireDimensions = dimensions;
            ChainThickness = thickness;
            Type = vehicleType;
        }
        /// <summary>
        /// Custom property and object level validation
        /// </summary>
        /// <param name="validationContext">Properties and their values</param>
        /// <returns>Fields and their's errors</returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();
            return results;
        }
        /// <summary>
        /// Vehicle types
        /// </summary>
        public enum VehicleType
        {
            Car,
            SUV,
            Other
        }
    }
}
 