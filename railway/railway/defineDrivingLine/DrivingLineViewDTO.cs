using System;
using System.Collections.Generic;

namespace railway.defineDrivingLine
{
    public class DrivingLineViewDTO
    {
        public int DrivingLineId { get; set; }
        public string Name { get; set; }
        public string TrainName { get; set; }
        
        public List<StationDTO> stationSchedules { get; set; }
        
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        
        public DateTime? newEndDate { get; set; }
    }
}