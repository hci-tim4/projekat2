using railway.clientTimetable;
using System;
using System.Collections.Generic;
using System.Text;

namespace railway.managerSchedule
{
    public class DrivingLineDTO
    {
        public int drivingLineId { get; set; }
        public string Name { get; set; }
        public List<string> Days { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday  { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }


    //    public string trafficDays { get; set; }
        public List<DetailDrivinglineDTO> schedule {get;set;}

        public DrivingLineDTO(int id, string name, List<string> days, List<DetailDrivinglineDTO> schedule) {
            this.Monday = false;
            this.Tuesday = false;
            this.Wednesday = false;
            this.Thursday = false;
            this.Friday = false;
            this.Saturday = false;
            this.Sunday = false;


            this.drivingLineId = id;
            this.Name = name;
            this.Days = days;
            this.schedule = schedule;

            for (int i = 0; i<this.Days.Count; i++) {
                switch (days[i])
                {
                    case "Monday":
                        this.Monday = true;
                        break;

                    case "Tuesday":
                        this.Tuesday = true;
                        break;
                    case "Wednesday":
                        this.Wednesday = true;
                        break;
                    case "Thursday":
                        this.Thursday = true;
                        break;
                    case "Friday":
                        this.Friday = true;
                        break;
                    case "Saturday":
                        this.Saturday = true;
                        break;
                    case "Sunday":
                        this.Sunday = true;
                        break;
                    default:
                        break;
                }

            }

        }
    }
}
