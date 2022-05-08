using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
    public class StationSchedule
    {
        public Station Station { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }

    }
}
