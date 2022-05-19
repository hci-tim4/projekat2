using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace railway.model
{
    public class SeatType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        //public virtual List<Seat> Seats { get; set; }
    }
}
