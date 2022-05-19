using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
 //   public enum SeatType { }
    public class Seat
    {   
        [Key]
        public int Id { get; set; }

        [ForeignKey("SeatType")]
        public int SeatTypeId { get; set; }
        public virtual SeatType SeatType { get; set; }

        [ForeignKey("Train")]
        public int TrainId { get; set; }
        public virtual Train Train { get; set; }

        public int Row { get; set; }
        public int Col { get; set; }

    }
}
