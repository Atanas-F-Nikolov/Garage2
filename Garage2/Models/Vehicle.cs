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
            CheckInTimeStamp = DateTime.Now;
        }

        public int Id { get; set; }
        public VehicleType Type { get; set; }

        [DisplayName("Parking space")]
        public int ParkingSpace { get; set; }
        public int Size { get; set; }

        [DisplayName("Registration number")]
        [Required(ErrorMessage = "Please enter a registration number!")]
        [MaxLength(10 ,ErrorMessage = "Please enter max 10 characters!")]
        [MinLength(4, ErrorMessage = "Please enter min 4 characters!")]
        [RegularExpression(@"^([a-zA-Z0-9 \-]+)$", ErrorMessage = "Only \"–\" as a special symbol is allowed!")]
        public string RegNumber { get; set; }

        [StringLength(10, ErrorMessage = "Please enter max 10 characters!")]
        [RegularExpression(@"^([a-zA-Z]+)$", ErrorMessage= "Please, enter only letters!")]
        [DisplayFormat(NullDisplayText = "[NotSet]")]
        public string Color { get; set; }

        [StringLength(16, ErrorMessage = "Please enter max 16 characters!")]
        [RegularExpression(@"^([a-zA-Z \&\""\'\-]+)$", ErrorMessage = "Please enter only letters, \"–\", \"&\", or \".")]
        [DisplayFormat(NullDisplayText = "[NotSet]")]
        public string Brand { get; set; }

        [StringLength(16, ErrorMessage = "Please enter max 16 characters!")]
        [RegularExpression(@"^([a-zA-Z0-9 \&\-]+)$", ErrorMessage = "Please enter only letters, numbers, \"–\" or \"&\".")]
        [DisplayFormat(NullDisplayText = "[NotSet]")]
        public string Model { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DisplayName("Number of wheels")]
        [Range(0, 16, ErrorMessage = "Please enter a number in the range [0, 16].")]
        public int Wheels { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yy, HH:mm}")]
        [DisplayName("Check-in time")]
        public DateTime CheckInTimeStamp { get; set; }

    }
}