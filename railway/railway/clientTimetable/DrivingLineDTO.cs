using System;
using System.Collections.Generic;
using System.Text;

namespace railway.clientTimetable
{
    public class DrivingLineDTO
    {
        public int Id { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Train { get; set; }

        public int FromStationScheduleId { get; set; }
        public int UntilStationScheduleId { get; set; }
        public int ScheduleId { get; set; }
        public int drivingLine { get; set; }

        public int Tour { get; set; }

    }
}
