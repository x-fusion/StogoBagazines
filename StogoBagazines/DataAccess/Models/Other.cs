using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    /// <summary>
    /// Class type representing general purpose object
    /// </summary>
    public class Other : InventoryBase, IValidatableObject
    {
        /// <summary>
        /// Local custructor
        /// </summary>
        /// <param name="title">Tite of item</param>
        /// <param name="amount">Count of items</param>
        /// <param name="value">Monetary value of item</param>
        public Other(string title, int amount, decimal value) : base(title, amount, value)
        {
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
        public Other(int id, string title, int amount, decimal totalRevenue, int totalRentDuration, decimal value) : base(id, title, amount, totalRevenue, totalRentDuration, value)
        {
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
    }
}
