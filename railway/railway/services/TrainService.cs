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
                    TrainDTO dto = new TrainDTO { Color = t.Color, Name = t.Name };
                    Dictionary<string, int> typeNumberSeats = getSeats(t);
                    dto.typeNumberSeats = typeNumberSeats;
                    List<string> drivingLines = getDrivingLines(t);
                    dto.drivingLineName = drivingLines;
                    allDTO.Add(dto);
                }
            }
            return allDTO;
        }

        public static Dictionary<string, int> getSeats(Train t)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            int vip = 0;
            int regular = 0;
            int bussines = 0;
            using (var db = new RailwayContext())
            {
                List<Seat> seats = (from s in db.seats
                                    where s.TrainId == t.Id
                                    select s).ToList();
                foreach (Seat s in seats)
                {
                    if (s.SeatTypeId == 1)
                        vip++;
                    else if (s.SeatTypeId == 2)
                    {
                        bussines++;
                    }
                    else
                        regular++;
                }

                dict.Add("VIP", vip);
                dict.Add("Biznis", bussines);
                dict.Add("Regularan", regular);

            }
            return dict;
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
