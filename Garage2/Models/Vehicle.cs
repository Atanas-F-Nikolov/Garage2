using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            TimeStamp = DateTime.Now;
        }

        public int Id { get; set; }
        public VehicleType Type { get; set; }
        
        [DisplayName("Registration number")]
        [Required(ErrorMessage = "Please enter a registration number!")]
        [MaxLength(10)]
        [MinLength(4)]
        [RegularExpression(@"^([a-zA-Z0-9 \-]+)$", ErrorMessage = "Please don't enter any special symbols!")]
        public string RegNumber { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^([a-zA-Z]+)$", ErrorMessage= "Please, enter only letters (a-z, A-Z)!")]
        public string Color { get; set; }

        [StringLength(16)]
        [RegularExpression(@"^([a-zA-Z \&\'\-]+)$", ErrorMessage = "Please enter only letters, '&', or \" !")]
        public string Brand { get; set; }

        [StringLength(16)]
        [RegularExpression(@"^([a-zA-Z0-9 \&\-]+)$", ErrorMessage = "Please enter letters, numbers, '&', or \" !")]
        public string Model { get; set; }

        [DisplayName("Number of wheels")]
        [Range(0,16)]
        public int Wheels { get; set; }

        [DisplayName("Check-in time")]
        public DateTime TimeStamp { get; set; }

    }
}