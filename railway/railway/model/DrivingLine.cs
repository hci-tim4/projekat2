using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
    public class DrivingLine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StationSchedule> StationSchedule { get; set; }
        public Train Train { get; set; }


    }
}
