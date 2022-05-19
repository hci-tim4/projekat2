using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public class TrafficDays
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DrivingLine")]
        public int DrivingLineId { get; set; }
        public virtual DrivingLine DrivingLine { get; set; }

        //[ForeignKey("Day")]
        //public int DayId { get; set; }
        public Days Day { get; set; }
    }
}
