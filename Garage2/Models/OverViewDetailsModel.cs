using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class OverViewDetailsModel
    {
        public string Model { get; set; }
        public string Brand { get; set; }
        public int Size { get; set; }
        [DisplayName("Number of wheels")]
        public int Wheels { get; set; }
    }
}