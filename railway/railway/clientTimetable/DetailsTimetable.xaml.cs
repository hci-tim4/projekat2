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
    public partial class DetailsTimetable : Page
    {
        List<DetailDrivinglineDTO> detailLine = new List<DetailDrivinglineDTO>();
        //private List<StationSchedule> stationSchedules;
        int drivingLineId;

        public DetailsTimetable(int drivingLineId, int fromStationId, int arrivalId)
        {
            InitializeComponent();
            showDrivingLineDetails(drivingLineId);
            using (var db = new RailwayContext())
            {
                List<StationSchedule> ss = (from stationSchedules in db.stationsSchedules
                    where stationSchedules.DrivingLineId == drivingLineId
                    orderby stationSchedules.SerialNumber
                    select stationSchedules).ToList();
                
                

                page.Content = new map.Map(ss, fromStationId, arrivalId);
            }

        }

        private void showDrivingLineDetails(int drivingLineId) {
            this.drivingLineId = drivingLineId;
            detailLine = FindDetails(drivingLineId);
            detailGrid.ItemsSource = detailLine;

        }
        private List<DetailDrivinglineDTO> FindDetails(int drivingLineId)
        {

            List<DetailDrivinglineDTO> detailDrivinglineDTOs = new List<DetailDrivinglineDTO>();

            using (var db = new RailwayContext())
            {
                var stations =
                    (from drivingLine in db.drivingLines
                     join stationSchedule in db.stationsSchedules
                     on drivingLine.Id equals stationSchedule.DrivingLineId
                     join station in db.stations
                     on stationSchedule.StationId equals station.Id
                     where drivingLine.Id == drivingLineId
                     select new
                     {
                         StationName = station.Name,
                         ArrivalTime = stationSchedule.ArrivalTime,
                         DepartureTime = stationSchedule.DepartureTime
                     }).ToList();

                foreach (var s in stations)
                {
                    detailDrivinglineDTOs.Add(new DetailDrivinglineDTO
                    {
                        StationName = s.StationName,
                        ArrivalTime = s.ArrivalTime,
                        DepartureTime = s.DepartureTime
                    });
                }
                return detailDrivinglineDTOs;
            }
        }
    }
}
