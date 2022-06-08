using railway.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace railway.dto.trains
{
    public class TrainDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Color { get; set; }
        public int numberVIP { get; set; }
        public int numberREGULAR { get; set; }
        public int numberBUSINESS { get; set; }
        public double priceVIP { get; set; }
        public double priceREGULAR { get; set; }
        public double priceBUSINESS { get; set; }
        public int col { get; set; }
        public List<String> drivingLineName { get; set; }
    }
}
