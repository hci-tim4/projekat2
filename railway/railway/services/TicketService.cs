using railway.database;
using railway.dto.tickets_view;
using railway.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using railway.drivingLineReport;
using railway.monthlyReport;

namespace railway.services
{
    public class TicketService
    {
        

        public static List<TicketsDTO> getTickets(int userId,TicketType ticketType)
        {
            List<TicketsDTO> dtos = new List<TicketsDTO>();
            List<Ticket> tickets = getTicketsFromDb(userId,ticketType);

            foreach (Ticket t in tickets) {
                List<Seat> seats = getTicketSeats(t.Id);
                foreach (Seat s in seats)
                {
                    TicketsDTO dto = getTicketAttribute(t);
                    setDate(t, dto);
                    dto.row = s.Row;
                    dto.col = s.Col;
                    dto.seatType = getSeatType(s.SeatTypeId);
                    dto.Price = countPriceOfTicket(t, s);
                    dtos.Add(dto);
                }
                
                
                /*
                List<Seat> seats = getSeats(t.Id);
                foreach (Seat s in seats)
                {
                    dto.col = s.Col;
                    dto.row = s.Row;
                    dto.seatType = getSeatType(s.SeatTypeId);
                    dtos.Add(dto);
                }*/
            }
            return dtos;
        }

        private static List<Seat> getTicketSeats(int tId)
        {
            using (var db = new RailwayContext())
            {
                List<Seat> res = (from ts in db.ticketSeats
                    where ts.TicketId == tId
                    select ts.Seat).ToList();
                return res;
            }
        }

        private static double countPriceOfTicket(Ticket ticket, Seat ts)
        {
            int numberOfStations = Math.Abs(ticket.FromStationScheduleId - ticket.UntilStationScheduleId);
            using (var db = new RailwayContext())
            {
                double seatTypePrice = (from s in db.seatTypes
                    where s.Id == ts.SeatTypeId
                    select s.Price).Single();
                return seatTypePrice * numberOfStations;
            }

            return numberOfStations * 100;
        }

        private static string getSeatType(int seatType)
        {
            if (seatType == 1)
            {
                return "VIP";
            }
            else if(seatType == 2)
            {
                return "BIZNIS";
            }
            else 
            {
                return "REDOVNA";
            }

        }


        public static List<TicketsDTO> getReservedTickets()
        {
            return new List<TicketsDTO>();
        }

        public static List<Ticket> getTicketsFromDb(int userId, TicketType ticketType)
        {
            using(var db = new RailwayContext())
            {
                List<Ticket> tickets = (from t in db.tickets
                    where t.UserId == userId && t.TicketType == ticketType
                                        select t).ToList();
                return tickets;
            }
        }

        public static TicketsDTO getTicketAttribute(Ticket ticket)
        {
            
            TicketsDTO dto = new TicketsDTO();
            setStationScheduleAttr(ticket, dto);
            //dto.Price = countTicketPrice(dto);//ticket.Price;//count price
            return dto;
        }

        public static void setStationScheduleAttr(Ticket ticket, TicketsDTO dto)
        {
            using (var db = new RailwayContext())
            {
                StationSchedule ssDeparture = (StationSchedule)(from s in db.stationsSchedules
                                   where s.Id == ticket.FromStationScheduleId
                                   select s).FirstOrDefault();
                dto.DepartureTime = (TimeSpan)ssDeparture.DepartureTime;
                dto.DepartureStationName = getStationName(ssDeparture.StationId);

                StationSchedule ssArrival = (StationSchedule)(from s in db.stationsSchedules
                                                                where s.Id == ticket.UntilStationScheduleId
                                                                select s).FirstOrDefault();
                dto.ArrivalTime= (TimeSpan)ssDeparture.ArrivalTime;
                dto.ArrivalStationName = getStationName(ssArrival.StationId);

            }
        }

        public static void setDate(Ticket ticket, TicketsDTO dto)
        {
            using (var db = new RailwayContext())
            {
                Schedule schedule = (Schedule)(from s in db.schedules
                                               where s.Id == ticket.ScheduleId
                                               select s).FirstOrDefault();
                dto.DepatureDate = schedule.DepatureDate;
            }
        }

        public static string getStationName(int id)
        {
            using (var db = new RailwayContext())
            {
                Station station = (Station)(from st in db.stations
                                            where st.Id == id
                                            select st
                                            ).FirstOrDefault();
                return station.Name;
            }
        }

        private static List<Seat> getSeats(int ticketId)
        {
            using (var db = new RailwayContext())
            {
                List<TicketSeats> ticketsSeats = (from ts in db.ticketSeats
                                        where ts.TicketId == ticketId
                                        select ts).ToList();
                List<Seat> seatsForTicket = new List<Seat>();
                foreach (TicketSeats t in ticketsSeats)
                {
                    Seat s = (from seat in db.seats
                              where t.SeatId == seat.Id
                              select seat).FirstOrDefault();
                    seatsForTicket.Add(s);
                }
                return seatsForTicket;
            }
        }

        public List<TicketForReportDTO> getTicketsInPeriod(DateTime? fromDate, DateTime? untilDate)
        {
            using (var db = new RailwayContext())
            {
                List<TicketForReportDTO> res = (from tick in db.tickets
                    join sched in db.schedules
                        on tick.ScheduleId equals sched.Id
                        where sched.DepatureDate > fromDate && sched.DepatureDate < untilDate
                    select new TicketForReportDTO()
                    {
                        TicketId = tick.Id,
                        DrivingLineId = sched.DrivingLineId,
                        Price = tick.Price,
                        fromStationId = tick.FromStationScheduleId,
                        untilStationId = tick.UntilStationScheduleId
                        //ticketSeats = tick.ticketSeats
                    }).ToList();
                foreach (TicketForReportDTO ticketForReport in res)
                {
                    ticketForReport.ticketSeats = new List<TicketSeats>();
                    foreach (TicketSeats t in db.ticketSeats)
                    {
                        if (ticketForReport.TicketId == t.TicketId)
                        {
                            ticketForReport.ticketSeats.Add(t);
                            
                        }
                    }
                }
                foreach (TicketForReportDTO t in res)
                {
                    t.NumberOfVIPSeats = t.GetNumberOfSeatType("VIP");
                    t.NumberOfBiznisSeats = t.GetNumberOfSeatType("Biznis klasa");
                    t.NumberOfRegularSeats = t.GetNumberOfSeatType("Regularan");
                    t.PriceOfVIPSeats = t.CountPriceForSeatType("VIP");
                    t.PriceOfBiznisSeats = t.CountPriceForSeatType("Biznis klasa");
                    t.PriceOfRegularSeats = t.CountPriceForSeatType("Regularan");
                }
                return res;
            }
        }
        
        public List<TicketsForDrivingLineReportDTO> GetTicketByDrivingLine(int drivingLineId)
        {
            using (var db = new RailwayContext())
            {
                List<TicketsForDrivingLineReportDTO> res = (from tick in db.tickets
                    join sched in db.schedules
                        on tick.ScheduleId equals sched.Id
                    where sched.DrivingLineId == drivingLineId
                    select new TicketsForDrivingLineReportDTO()
                    {
                        TicketId = tick.Id,
                        Price = tick.Price,
                        DateOfDepature = sched.DepatureDate
                    }).ToList();

                foreach (TicketsForDrivingLineReportDTO ticketForReport in res)
                {
                    ticketForReport.numberOfTickets = 0;
                    foreach (TicketSeats t in db.ticketSeats)
                    {
                        if (ticketForReport.TicketId == t.TicketId)
                        {
                            ticketForReport.numberOfTickets++;
                        }
                    }
                
                }

                return res;
            }
        }
        
    }
}
