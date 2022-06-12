using System;
using System.Collections.Generic;
using railway.model;

namespace railway.monthlyReport
{
    public class TicketForReportDTO
    {
        public int TicketId { get; set; }
        public int DrivingLineId { get; set; }
        public double Price { get; set; }

        public int NumberOfVIPSeats { get; set; }
        public int NumberOfBiznisSeats { get; set; }
        public int NumberOfRegularSeats { get; set; }
        
        
        public double PriceOfVIPSeats { get; set; }
        public double PriceOfBiznisSeats { get; set; }
        public double PriceOfRegularSeats { get; set; }
        
        public int fromStationId { get; set; }
        public int untilStationId { get; set; }
        
        public List<TicketSeats> ticketSeats;

        public int GetNumberOfSeatType(string name)
        {
            int res = 0;
            foreach (TicketSeats ts in ticketSeats)
            {
                if (ts.Seat.SeatType.Name == name)
                {
                    res++;
                }
            }
            return res;
        }

        public double CountPriceForSeatType(string name)
        {
            double res = 0;
            foreach (TicketSeats ts in ticketSeats)
            {
                if (ts.Seat.SeatType.Name == name)
                {
                    res += (Math.Abs(untilStationId - fromStationId) * ts.Seat.SeatType.Price);
                }
            }
            return res;
        }
    }
}