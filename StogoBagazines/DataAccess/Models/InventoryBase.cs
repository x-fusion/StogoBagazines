using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Base class of warehouse items
    /// </summary>
    public class InventoryBase : IValidatableObject
    {
        /// <summary>
        /// Identification key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Item's title
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is manditory field")]
        [StringLength(255, ErrorMessage = "Length has to be between 5 and 255 symbols", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Invalid title provided")]
        public string Title { get; set; }
        /// <summary>
        /// Count of particular item in warehouse
        /// </summary>
        [Required(ErrorMessage = "Amount is manditory field")]
        [Range(0, int.MaxValue, ErrorMessage = "Not valid integer value")]
        public int Amount { get; set; }
        /// <summary>
        /// Price for renting purposes
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Not valid decimal value")]
        public decimal RentPrice => decimal.Multiply(MonetaryValue, MonetaryValue / 10);
        /// <summary>
        /// Revenue generated during rentals
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Not valid decimal value")]
        public decimal Revenue { get; set; }
        /// <summary>
        /// Days spent at rent
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Not valid integer value")]
        public int TotalRentDuration { get; set; }
        /// <summary>
        /// Price used for selling purposes
        /// </summary>
        [Required(ErrorMessage = "Monetary value is manditory field")]
        [Range(0, double.MaxValue, ErrorMessage = "Not valid decimal value")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid value provided")]
        public decimal MonetaryValue { get; set; }
        /// <summary>
        /// Constructor used in serialization
        /// </summary>
        public InventoryBase()
        {
        }
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
            if (Amount - amount < 0)
            {
                return false;
            }
            Amount -= amount;
            TotalRentDuration += amount * duration;
            return true;
        }
        /// <summary>
        /// Custom property and object level validation
        /// </summary>
        /// <param name="validationContext">Properties and their values</param>
        /// <returns>Fields and their's errors</returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            List<string> members = new List<string>();
            if(MonetaryValue <= 0)
            {
                members.Add(nameof(MonetaryValue));
                results.Add(new ValidationResult("Monetary value cannot be 0 or lower", members));
            }
            return results;
        }
    }
}
