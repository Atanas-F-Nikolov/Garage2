using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class FreeSpaces
    {
        public int RegularSpaces { get; set; }
        public int MotorSpaces { get; set; }
        public int AllSpaces { get; set; }

        public bool IsSpacesLeft { get { return RegularSpaces > 0 || MotorSpaces > 0; } }
    }
}