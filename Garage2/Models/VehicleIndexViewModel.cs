using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage2.Models
{
    public class VehicleIndexViewModel
    {
        public int? CreatedVehicleId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public SelectList VehicleTypes { get; set; }
    }
}