using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace railway.model
{
    public class Train
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public String Color { get; set; }
        public bool Deleted { get; set; }

        public virtual List<Seat> Seats {get;set;}

        public virtual List<DrivingLine> DrivingLines { get; set; }
       // public bool Deleted { get; set; }
    }
}
