using System;
using System.Collections.Generic;
using System.Text;

namespace railway.clientTimetable
{
    public class DetailDrivinglineDTO
    {
        public string StationName { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }
    }
}
