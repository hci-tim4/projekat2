using System;
using System.Collections.Generic;
using System.Linq;
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
using railway.database;
using railway.model;

namespace railway.clientTimetable
{
    /// <summary>
    /// Interaction logic for Timetable.xaml
    /// </summary>
    public partial class Timetable : Page
    {
        DateTime date;
        Station departure;
        Station arrival;
        public Timetable()
        {
            InitializeComponent();
            cmbDeparture.ItemsSource = GetAllStations();
            cmbArrival.ItemsSource = GetAllStations();
        }

        private List<Station> GetAllStations() {
            using (var db = new RailwayContext()) {
                var stations = db.stations.Select(s=>s).ToList();
                return stations;
            }   
        }

        private void cmbDeparture_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.departure = (Station) (cmbDeparture.SelectedItem as Station);
          //  cmbDeparture.= selectedStation.Name;
        }

        private void cmbArrival_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) 
        {
            this.arrival = (Station) (cmbArrival.SelectedItem as Station);
         // cmbArrival.DataContext = arrivalStation.Name;

        }

        private void getInput() {
            Station departureStation = (Station)cmbDeparture.SelectedItem;
            Station arrivalStation = (Station)cmbArrival.SelectedItem;
        }

        private void btn_search(object sender, RoutedEventArgs e) 
        {
            Find(this.departure, this.arrival, this.date);
  

        }

        private List<DrivingLineDTO> Find(Station departure, Station arrival, DateTime date) 
        {
            departure = new Station()
            {
                Id = 1
            };
            //arrival.Id = 4;


            List<DrivingLineDTO> DTOs = new List<DrivingLineDTO>();
            using (var db = new RailwayContext())
            {
                var stations = 
                    (from station in db.stations
                    join stationSchedule in db.stationsSchedules
                    on station.Id equals stationSchedule.Station.Id
                    where station.Id==departure.Id
                select new
                { 
                    departureName = station.Name,
                    departureId = station.Id,
                    departureTime = stationSchedule.DepartureTime,
                    serialNumber = stationSchedule.SerialNumber,
                    DrivinglineId = stationSchedule.DrivingLineId

                }).ToList();





                /*join  DrivingLine in db.drivingLines
                on stationSchedule.DrivingLineId equals DrivingLine.Id
                join Train in db.trains
                on DrivingLine.TrainId equals Train.Id
                join schedule in db.schedules
                on DrivingLine.Id equals schedule.DrivingLineId  */
                return null;
            }
        }

        private void selectedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

    }
}
