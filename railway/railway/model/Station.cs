using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace railway.model
{
    public class Station
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<StationSchedule> StationSchedules { get; set; }

        public double Latitude { get; set;}
        public double Longitude { get; set;}



        //podaci za mapu
        // longitude, latitude
    }
}
