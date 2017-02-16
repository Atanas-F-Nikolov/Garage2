using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class OverViewModel
    {
        public int VehicleId { get; set; }
        public string ParkingSpace { get; set; }
        public VehicleType Type { get; set; }
        public string RegNumber { get; set; }
        public string Color { get; set; }
        public DateTime CheckInTimeStamp { get; set; }
    }
}