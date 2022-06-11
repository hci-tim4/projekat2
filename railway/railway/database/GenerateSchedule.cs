using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using railway.model;
using System.Windows;

namespace railway.database
{
    public class GenerateSchedule
    {
        public GenerateSchedule() { 
        
        }

        public void Generate() {
            using (var db = new RailwayContext()) 
            {
                var info =
                    (from trafficDays in db.TrafficDays
                     join drivingLine in db.drivingLines
                     on trafficDays.DrivingLineId equals drivingLine.Id 
                     where drivingLine.startDate <= DateTime.Now
                     select new
                     {
                         drivingLineId = drivingLine.Id,
                         day = trafficDays.Day
                     }).ToList();


                foreach(var i in info) {
                    List<DateTime> dates = GetDatesForDay(i.day);
                    foreach (DateTime date in dates) {
                        Schedule schedule = new Schedule();
                        schedule.DrivingLineId = i.drivingLineId;
                        schedule.DepatureDate = date;
                        db.schedules.Add(schedule);
                        db.SaveChanges();
                    }
                    
                }
                
            }
        }

        private List<DateTime> GetDatesForDay(Days day) {
            List<DateTime> dates = new List<DateTime>();

            DateTime start = DateTime.Now.Date;
            DateTime end = start.AddDays(30);

            for (int i = 0; i < 10; i++) {
                if (start.DayOfWeek.ToString().Equals(day.ToString())) {
                    DateTime newOne = start;
                    dates.Add(newOne);
                }
                
                
                start = start.AddDays(1);
            }

            return dates;
        }
    }
}
