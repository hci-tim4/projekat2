using System;
using System.Collections.Generic;
using System.Text;

namespace railway.client
{
    public class GetTicketDTO
    {
        public int FromStationScheduleId { get; set; }
        public int UntilStationScheduleId { get; set; }
        public int ScheduleId { get; set; }

        public int DrivingLineId { get; set; }
    }
}
