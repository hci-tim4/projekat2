using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public enum Days { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday}
    public class Schedule
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("DrivingLine")]
        public int DrivingLineId { get; set; }
        public virtual DrivingLine DrivingLine { get; set; }


        public DateTime DepatureDate { get; set; }    // samo date je bitno


        public virtual List<Ticket> Tickets { get; set; }

        //public virtual List<ScheduleStationFreePlace> ScheduleStationFreePlaces { get; set; }
    }
}
