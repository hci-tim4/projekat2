using System;
using System.Collections.Generic;
using System.Text;

namespace railway.dto.tickets_view
{
    public class TicketsDTO
    {
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime DepatureDate { get; set; }
        public double Price { get; set; }
        public string DepartureStationName { get; set; }
        public string ArrivalStationName { get; set; }
        public string seatType { get; set; }
        public int row { get; set; }
        public int col { get; set; }

    }
}
