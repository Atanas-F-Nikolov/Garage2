using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class VehicleDetailsViewModel
    {
        public int VehicleId { get; set; }
        public DateTime ParkTime { get; set; }
        public int TypeId { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public string RegNumber { get; set; }
        public string ParkingTime { get; set; }
    }
}