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
            CheckOutTimeStamp = DateTime.Now;
        }

        public Vehicle vehicle { get; set; }

        public double PricePerHour { get; set; } = 100;

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yy, HH:mm}")]
        [DisplayName("Check-out time")]
        public DateTime CheckOutTimeStamp { get; set; }

        [DisplayName("Total duration")]
        public double ParkingsPeriodInMin { get; set; }

        [DisplayName("Total price")]
        public double TotalPrice { get; set; }
    }
}