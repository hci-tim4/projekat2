using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public class DrivingLine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual List<StationSchedule> StationSchedule { get; set; }

        [ForeignKey("Train")]
        public int TrainId { get; set; }
        public virtual Train Train { get; set; }

        //public virtual List<Schedule> Schedules { get; set; }

    }
}
