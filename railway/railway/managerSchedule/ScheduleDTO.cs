using System;
using System.Collections.Generic;
using System.Text;

namespace railway.managerSchedule
{
    public class ScheduleDTO
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int SerialNumber { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int DrivingLineId { get; set; }
        public int Tour { get; set; }
    }
}
