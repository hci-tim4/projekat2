using System;
using System.Collections.Generic;
using System.Text;

namespace railway.client
{
    class ConfirmationTicketDTO
    {
        public String fromStation { get; set; }

        public String untilStation { get; set; }

        public DateTime Date { get; set; }

        public int ticketCount { get; set; }

        public double price { get; set; }

        public TimeSpan depatureTime { get; set; }

        public TimeSpan arrivalTime { get; set; }
    }
}
