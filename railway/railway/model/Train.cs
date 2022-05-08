using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
    public class Train
    {
        public int Id { get; set; }
        public List<Seat> Seats {get;set;}
       // public bool Deleted { get; set; }
    }
}
