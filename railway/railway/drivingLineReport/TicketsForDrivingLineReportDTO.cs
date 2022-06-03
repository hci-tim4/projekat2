using System;

namespace railway.drivingLineReport
{
    public class TicketsForDrivingLineReportDTO
    {
        public int TicketId { get; set; }
        public double Price { get; set; }
        public int numberOfTickets { get; set; }
        
        public DateTime DateOfDepature { get; set; }
    }
}