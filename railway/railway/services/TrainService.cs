using railway.database;
using railway.dto.trains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using railway.model;

namespace railway.services
{
    public class TrainService
    {
        public static List<TrainDTO> getTrains()
        {
            List<TrainDTO> allDTO = new List<TrainDTO>();
            using(var db = new RailwayContext())
            {
                List<Train> allTrains = (from t in db.trains
                                         select t).ToList();

                foreach(Train t in allTrains)
                {
                    TrainDTO dto = new TrainDTO { Color = t.Color, Name = t.Name, Id=t.Id};
                    getSeats(t,dto);
                    getSeatPrices(t, dto);
                    List<string> drivingLines = getDrivingLines(t);
                    dto.drivingLineName = drivingLines;
                    allDTO.Add(dto);
                }
            }
            return allDTO;
        }

        public static void getSeats(Train t, TrainDTO dto)
        {

            dto.col = getColNum(t.Id);
            if(dto.col == 0)
            {
                dto.numberREGULAR = 0;
                dto.numberBUSINESS = 0;
                dto.numberVIP = 0;
            }
            else
            {
                dto.numberREGULAR = getRegNum(t.Id) / dto.col;
                dto.numberBUSINESS = getBusNum(t.Id) / dto.col;
                dto.numberVIP = getVipNum(t.Id) / dto.col;

            }
          


            
        }

        private static int getColNum(int id)
        {
            var db = new RailwayContext();
            var seats = from s in db.seats
                        where s.TrainId == id & s.Row == 1
                        select s;
            return seats.Count();
        }


        private static int getRegNum(int id)
        {
            var db = new RailwayContext();
            var seats = from s in db.seats
                        where s.TrainId == id & s.SeatTypeId == 3
                        select s;
            return seats.Count();
        }

        private static int getVipNum(int id)
        {
            var db = new RailwayContext();
            var seats = from s in db.seats
                        where s.TrainId == id & s.SeatTypeId == 1
                        select s;
            return seats.Count();
        }

        private static int getBusNum(int id)
        {
            var db = new RailwayContext();
            var seats = from s in db.seats
                        where s.TrainId == id & s.SeatTypeId == 2
                        select s;
            return seats.Count();
        }


        public static void getSeatPrices(Train t, TrainDTO dto)
        {
            List<Seat> seats = t.Seats;
            foreach (Seat s in seats)
            {
                if(s.SeatType.Name == "VIP")
                {
                    dto.priceVIP = s.SeatType.Price;
                }
                else if(s.SeatType.Name == "REGULAR")
                {
                    dto.priceREGULAR = s.SeatType.Price;
                }
                else
                {
                    dto.priceBUSINESS = s.SeatType.Price;
                }
            }
        }

        public static List<string> getDrivingLines(Train t)
        {
            using(var db = new RailwayContext())
            {
                List<string> drivingLines = (from dl in db.drivingLines
                                             where dl.TrainId == t.Id
                                             select dl.Name).ToList();
                return drivingLines;
            }
        }
    }
}
