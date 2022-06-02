using railway.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace railway.dto.trains
{
    public class TrainDTO
    {
        public String Name { get; set; }
        public String Color { get; set; }
        public Dictionary<string, int> typeNumberSeats { get; set; }
        public List<String> drivingLineName { get; set; }
    }
}
