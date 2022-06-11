using railway.database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using railway.model;

namespace railway.clientTimetable
{
    /// <summary>
    /// Interaction logic for DetailsTimetable.xaml
    /// </summary>
    public partial class DetailsTimetable : Window
    {
        List<DetailDrivinglineDTO> detailLine = new List<DetailDrivinglineDTO>();
        //private List<StationSchedule> stationSchedules;
        int drivingLineId;
        private Frame parentFrame;
        private Page parentPage;

        public DetailsTimetable(int tour, int fromStationId, int arrivalId, Frame parentFrame, Page timetable, int drivingLineId)
        {
            InitializeComponent();
            showDrivingLineDetails(tour, drivingLineId);
            using (var db = new RailwayContext())
            {
                List<StationSchedule> ss = (from stationSchedules in db.stationsSchedules
                    where stationSchedules.DrivingLineId == drivingLineId
                    orderby stationSchedules.SerialNumber
                    select stationSchedules).ToList();
                
                

                page.Content = new map.Map(ss, fromStationId, arrivalId);
            }
            this.parentFrame = parentFrame;
            this.parentPage = timetable;
        }

        private void showDrivingLineDetails(int tour, int drivingLine) {
            detailLine = FindDetails(tour, drivingLine);
            detailGrid.ItemsSource = detailLine;

        }
        private List<DetailDrivinglineDTO> FindDetails(int tour, int drivingLineId)
        {

            List<DetailDrivinglineDTO> detailDrivinglineDTOs = new List<DetailDrivinglineDTO>();

            using (var db = new RailwayContext())
            {
            /*    int drivingLineId =
                (from station in db.stationsSchedules
                where station.Tour == tour
                select station.DrivingLineId).FirstOrDefault();

                this.drivingLineId = drivingLineId;   */

                var stations =
                    (from stationSchedule in db.stationsSchedules
                    join station in db.stations
                    on stationSchedule.StationId equals station.Id
                    where stationSchedule.DrivingLineId == drivingLineId && stationSchedule.Tour == tour
                     select new
                    {
                        StationName = station.Name,
                        ArrivalTime = stationSchedule.ArrivalTime,
                        DepartureTime = stationSchedule.DepartureTime
                    }).ToList();
                  


                foreach (var s in stations)
                {
                    DetailDrivinglineDTO dto = new DetailDrivinglineDTO
                    {
                        StationName = s.StationName,
                        ArrivalTime = s.ArrivalTime,
                        DepartureTime = s.DepartureTime
                    };

                    detailDrivinglineDTOs.Add(dto);
                   
                    
                }
                return detailDrivinglineDTOs;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.parentFrame.Content = parentPage;
        }
    
    }
}
