using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public VehicleType Type { get; set; }
        
        [DisplayName("Registration number")]
        public string RegNumber { get; set; }
        
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        [DisplayName("Number of wheels")]
        public int Wheels { get; set; }


    }
}