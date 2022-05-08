using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
    public class Schedule
    {
        public DrivingLine DrivingLine { get; set; }
        public DateTime DepatureDate { get; set; }    // samo date je bitno
    }
}
