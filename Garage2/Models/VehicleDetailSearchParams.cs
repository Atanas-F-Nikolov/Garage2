using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class VehicleDetailSearchParams
    {
        public string RegNr { get; set; }
        public DateTime? CheckIn { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Wheels { get; set; }
        public string Owner { get; set; }
        public int? VehicleTypeId { get; set; }
    }
}