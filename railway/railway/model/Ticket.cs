using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
    public enum TicketType {Reserved, Bought }
    public class Ticket
    {
        public Schedule Schedule { get; set; }
        public TicketType TicketType { get; set; }
        public User User { get; set; }
    }
}
