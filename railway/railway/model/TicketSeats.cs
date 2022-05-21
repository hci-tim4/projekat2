using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public class TicketSeats
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Seat")]
        public int SeatId { get; set; }
        public virtual Seat Seat { get; set; }


        //[ForeignKey("Ticket")]
        public int TicketId { get; set; }
        //public virtual Ticket Ticket { get; set; }
    }
}
