using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
 //   public enum SeatType { }
    public class Seat
    {   
        public int Id { get; set; }
        public SeatType SeatType { get; set; }
        public Train Train { get; set; }


    }
}
