using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public class ScheduleStationFreePlace
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StationSchedule")]
        public int StationScheduleId { get; set; }
        public virtual StationSchedule StationSchedule { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        public int FreePlace { get; set; }

    }
}
