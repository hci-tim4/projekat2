using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using railway.model;

namespace railway.database
{
    public class RailwayContext : DbContext
    {
        public RailwayContext()
        {
            
        }
        
        public DbSet<DrivingLine> drivingLines { get; set; }

        public DbSet<Schedule> schedules { get; set; }

        public DbSet<Seat> seats { get; set; }

        public DbSet<SeatType> seatTypes { get; set; }

        public DbSet<Station> stations { get; set; }

        public DbSet<StationSchedule> stationsSchedules { get; set; }

        public DbSet<Ticket> tickets { get; set; }

        public DbSet<Train> trains { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<TicketSeats> ticketSeats { get; set; }

        public DbSet<TrafficDays> TrafficDays { get; set; }
    }
}
