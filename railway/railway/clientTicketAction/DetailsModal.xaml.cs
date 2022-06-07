using railway.database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using railway.model;
using System.Linq;
using railway.clientTimetable;

namespace railway.clientTicketAction
{
    /// <summary>
    /// Interaction logic for DetailsModal.xaml
    /// </summary>
    public partial class DetailsModal : UserControl
    {
        List<DetailDrivinglineDTO> detailLine = new List<DetailDrivinglineDTO>();
        public DetailsModal()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        private bool _hideRequest = false;
        private bool _result = false;
        private UIElement _parent;

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }



        public bool ShowHandlerDialog(int tour, int fromStationId, int arrivalId, int drivingLineId)
        {
            showDrivingLineDetails(tour, drivingLineId);
            using (var db = new RailwayContext())
            {
                List<StationSchedule> ss = (from stationSchedules in db.stationsSchedules
                                            where stationSchedules.DrivingLineId == drivingLineId
                                            orderby stationSchedules.SerialNumber
                                            select stationSchedules).ToList();



                page.Content = new map.Map(ss, fromStationId, arrivalId);
            }
            Visibility = Visibility.Visible;

            _parent.IsEnabled = false;

            _hideRequest = false;
            while (!_hideRequest)
            {
                // HACK: Stop the thread if the application is about to close
                if (this.Dispatcher.HasShutdownStarted ||
                    this.Dispatcher.HasShutdownFinished)
                {
                    break;
                }

                // HACK: Simulate "DoEvents"
                this.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Background,
                    new ThreadStart(delegate { }));
                Thread.Sleep(20);
            }

            return _result;
        }

        private void showDrivingLineDetails(int tour, int drivingLine)
        {
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

        private void HideHandlerDialog()
        {
            _hideRequest = true;
            Visibility = Visibility.Hidden;
            _parent.IsEnabled = true;
        }

        

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _result = false;
            HideHandlerDialog();
        }
    
}
}
