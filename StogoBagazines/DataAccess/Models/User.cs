using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StogoBagazines.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is manditory field")]
        [StringLength(45, ErrorMessage = "Length has to be between 5 and 45 symbols", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Invalid First name provided")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is manditory field")]
        [StringLength(45, ErrorMessage = "Length has to be between 5 and 45 symbols", MinimumLength = 5)]
        [DataType(DataType.Text, ErrorMessage = "Invalid Last name provided")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is manditory field")]
        [StringLength(255, ErrorMessage = "Length cannot exceed 45 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email provided")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Text)]
        public string Role { get; set; }
        [field: NonSerialized]
        public string Token { get; set; }
    }
    public static class Role
    {
        public const string User = "User";
        public const string Manager = "Manager";
        public const string SysAdmin = "SysAdmin";
    }
}
