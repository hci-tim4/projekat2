using System;
using System.Collections.Generic;
using System.Text;
using railway.model;

namespace railway.client
{
    public class TicketDTO
    {
        public String fromStation { get; set; }
        public String untilStation { get; set; }

        public TimeSpan depatureTime { get; set; }
        public TimeSpan arrivalTime { get; set; }

        public DateTime Date { get; set; }
    }
}
