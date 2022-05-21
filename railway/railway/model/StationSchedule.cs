using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace railway.model
{
    public class StationSchedule
    {
        [Key]
        public int Id { get; set; }
        public int SerialNumber { get; set; }


        [ForeignKey("Station")]
        public int StationId { get; set; }
        public virtual Station Station { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }

        
        [ForeignKey("DrivingLine")]
        public int DrivingLineId { get; set; }
        
        public virtual DrivingLine DrivingLine { get; set; }
        

    }
}
