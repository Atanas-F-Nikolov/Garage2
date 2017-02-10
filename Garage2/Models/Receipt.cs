using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class Receipt
    {
        public Receipt()
        {
            checkOutTimeStamp = DateTime.Now;
        }

        public Vehicle vehicle { get; set; }

        public double PricePerHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yy, HH:mm}")]
        [DisplayName("Check-out time")]
        public DateTime checkOutTimeStamp { get; set; }

        [DisplayName("Total duration")]
        public double ParkingPeriodInMin { get; set; }

        [DisplayName("Total price")]
        public double TotalPrice { get; set; }
    }
}