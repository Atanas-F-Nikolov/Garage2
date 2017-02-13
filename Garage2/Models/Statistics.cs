using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class Statistics
    {
        public List<GroupByTypeStatistics> GroupByDiffStatistics { get; set; }

        public int TotalVehiclesNumber { get; set; }
        public int TotalWheelsNumber { get; set; }
        public double TotalParkingTime { get; set; }
        public double TotalParkingPrice { get; set; }
    }
}