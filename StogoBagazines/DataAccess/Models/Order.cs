using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Warehouse rent object containing details about real world 
    /// </summary>
    public class Order : IValidatableObject
    {
        /// <summary>
        /// Identification key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Item's title
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer is manditory field")]
        [StringLength(255, ErrorMessage = "Length has to be between 5 and 255 symbols", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Invalid customer provided")]
        public string Customer { get; set; }
        /// <summary>
        /// Value which can be assigned and used instead of CalculatedPrice (for manually asigned price operations)
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Not valid decimal value")]
        public decimal CustomPrice { get; set; }
        /// <summary>
        /// Value which is calculated from every order item
        /// </summary>
        public decimal CalculatedPrice
        {
            get
            {
                decimal sum = 0;
                RoofRacks.ForEach(x => sum += x.Item1.RentPrice * x.Item2);
                BicycleRacks.ForEach(x => sum += x.Item1.RentPrice * x.Item2);
                Crossbars.ForEach(x => sum += x.Item1.RentPrice * x.Item2);
                Others.ForEach(x => sum += x.Item1.RentPrice * x.Item2);
                WheelChains.ForEach(x => sum += x.Item1.RentPrice * x.Item2);
                return sum;
            }
        }
        List<Tuple<RoofRack, int>> RoofRacks { get; set; }
        List<Tuple<BicycleRack, int>> BicycleRacks { get; set; }
        List<Tuple<Crossbar, int>> Crossbars { get; set; }
        List<Tuple<WheelChain, int>> WheelChains { get; set; }
        List<Tuple<Other, int>> Others { get; set; }
        /// <summary>
        /// Constructor used in serialization
        /// </summary>
        public Order()
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
