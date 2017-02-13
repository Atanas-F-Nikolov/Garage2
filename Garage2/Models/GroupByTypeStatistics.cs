using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class GroupByTypeStatistics
    {
        public VehicleType VehicleGroup { get; set; }
        public int VehicleCountInAGroup { get; set; }
        public int WheelsNumberInAGroup { get; set; }
        public double ParkingTimeInAGroup { get; set; }
        public double ParkingPriceInAGroup { get; set; }
    }
}