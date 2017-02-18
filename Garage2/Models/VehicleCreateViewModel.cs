using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class VehicleCreateViewModel
    {
        public FreeSpaces Spaces { get; set; }
        public Vehicle Vehicle { get; set; }
        public System.Web.Mvc.SelectList VehicleTypes { get; set; }
        public System.Web.Mvc.SelectList Owners { get; set; }
    }
}