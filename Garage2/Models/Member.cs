using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage2.Models
{
    public class Member
    {
        public int Id { get; set; }

        [DisplayName("Social security number")]
        [RegularExpression(@"^(?:19|[2-9][0-9]){0,1}(?:[0-9]{2})(?!0229|0230|0231|0431|0631|0931|1131)(?:(?:0[1-9])|(?:1[0-2]))(?:(?:0[1-9])|(?:1[0-9])|(?:2[0-9])|(?:3[01]))[-+](?!0000)(?:[0-9]{4})$",
        ErrorMessage = "Invalid format (yymmdd-xxxx)")]
        [Required(ErrorMessage = "Please enter a social security number")]
        [Remote("DoesUserExist", "Members", ErrorMessage = "Social security number already exists")]
        public string SocialSecurityNumber { get; set; }

        [DisplayName("First name")]
        [Required(ErrorMessage = "Please enter a first name")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Please enter a last name")]
        [StringLength(25)]
        public string LastName { get; set; }

        [DisplayName("Date of birth")]
        [Required(ErrorMessage = "Please enter a date of birth")]
        public DateTime DateOfBirth { get; set; }


        public int Age { get { return (int)(DateTime.Now.Subtract(DateOfBirth).TotalDays / 365); } }
        [DisplayName("Full name")]
        public string FullName { get { return FirstName + " " + LastName; } }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}