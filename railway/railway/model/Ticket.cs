using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public enum TicketType {Reserved, Bought }
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public TicketType TicketType { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int FromStationScheduleId { get; set; }
        public int UntilStationScheduleId { get; set; }

        public int ScheduleId { get; set; }

        public double Price { get; set; }

        public List<TicketSeats> ticketSeats;

        public int Tour { get; set; }
    }
}
