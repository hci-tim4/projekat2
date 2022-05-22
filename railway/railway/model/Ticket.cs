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

        //[ForeignKey("Schedule")]
        //public int ScheduleId { get; set; }
        //public virtual Schedule Schedule { get; set; }
        public TicketType TicketType { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        //[ForeignKey("FromStationSchedule")]
        public int FromStationScheduleId { get; set; }
        //public StationSchedule FromStationSchedule { get; set; }

        //[ForeignKey("UntilStationSchedule")]
        public int UntilStationScheduleId { get; set; }
        //public StationSchedule UntilStationSchedule { get; set; }

        //public virtual List<TicketSeats> Seats { get; set; }

        public int ScheduleId { get; set; }

        public double Price { get; set; }

    }
}
