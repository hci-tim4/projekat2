using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using railway.client;
using railway.database;
using railway.model;

namespace railway.clientTimetable
{
    /// <summary>
    /// Interaction logic for Timetable.xaml
    /// </summary>
    public partial class Timetable : Page
    {
        DateTime? date = null;
        Station departure;
        Station arrival;
        List<DrivingLineDTO> lines = new List<DrivingLineDTO>();
        List<Station> stations = GetAllStations();
        User loggedUser;
        private Frame parentFrame;

        public Timetable(User user, Frame page1)
        {
            InitializeComponent();
            parentFrame = page1;
            cmbDeparture.ItemsSource = GetAllStations();
            cmbArrival.ItemsSource = GetAllStations();
            this.loggedUser = user;
        }

        private static List<Station> GetAllStations()
        {
            using (var db = new RailwayContext())
            {
                var stations = db.stations.Select(s => s).ToList();
                return stations;
            }
        }
        private void TextBox_TextChanged1(object sender, TextChangedEventArgs e)
        {
            var cmbx = sender as ComboBox;
            cmbx.ItemsSource = from item in stations
                               where item.Name.ToLower().Contains(cmbx.Text.ToLower())
                               select item;
            this.departure = (from item in stations
                            where item.Name.ToLower().Equals(cmbx.Text.ToLower())
                            select item).FirstOrDefault();

            cmbx.IsDropDownOpen = true;
        }

        private void TextBox_TextChanged2(object sender, TextChangedEventArgs e)
        {
            var cmbx = sender as ComboBox;
            cmbx.ItemsSource = from item in stations
                               where item.Name.ToLower().Contains(cmbx.Text.ToLower())
                               select item;

            this.arrival = (from item in stations
                           where item.Name.ToLower().Equals(cmbx.Text.ToLower())
                           select item).FirstOrDefault();

            cmbx.IsDropDownOpen = true;
        }

        private void cmbDeparture_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.departure = (Station)(cmbDeparture.SelectedItem as Station);
        }

        private void cmbArrival_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.arrival = (Station)(cmbArrival.SelectedItem as Station);
        }
     
        private void btn_search(object sender, RoutedEventArgs e)
        {

            if ((this.departure == null) || (this.arrival == null) || (this.date == null))
            {
                MessageBox.Show("Unesite sve podatke za pretragu");
            }
            else
            {
                this.lines = Find(this.departure, this.arrival, (DateTime)this.date);
                dataGrid.ItemsSource = this.lines;
            }

        }

        private List<DrivingLineDTO> Find(Station departure, Station arrival, DateTime date)
        {
          /*  departure = new Station()
            {
                Id = 1
            };
            //arrival.Id = 4;
            arrival = new Station() { Id = 11 };

            date = DateTime.ParseExact("05/29/2022", "MM/dd/yyyy", null);
          */

            List<DrivingLineDTO> DTOs = new List<DrivingLineDTO>();

            bool correct = false;
            int dtoId = 0;
            using (var db = new RailwayContext())
            {
                var stations =
                    (from station in db.stations
                     join stationSchedule in db.stationsSchedules
                     on station.Id equals stationSchedule.Station.Id
                     where station.Id == departure.Id
                     select new
                     {
                         departureName = station.Name,
                         departureId = station.Id,
                         departureTime = stationSchedule.DepartureTime,
                         serialNumber = stationSchedule.SerialNumber,
                         DrivinglineId = stationSchedule.DrivingLineId,
                         FromScheduleStationId = stationSchedule.Id

                     }).ToList();


                foreach (var s in stations) {

                    var drivingLineStations =
                        (from DrivingLine in db.drivingLines
                         join stat in db.stationsSchedules
                         on DrivingLine.Id equals stat.DrivingLineId
                         join train in db.trains
                         on DrivingLine.TrainId equals train.Id
                         where DrivingLine.Id == s.DrivinglineId
                         select new
                         {
                             EndStationId = stat.StationId,
                             UntilStationSchedule = stat.Id,
                             TrainName = train.Name
                         }).ToList();


                    foreach (var until in drivingLineStations) {
                        if (departure.Id == until.EndStationId) {
                            correct=true;
                        }
                        if ((arrival.Id == until.EndStationId) && (correct==true)) {
                            var datesTravell =
                                (from schedule in db.schedules
                                where schedule.DrivingLineId == s.DrivinglineId
                                select new { 
                                    scheduleId = schedule.Id,
                                    departureDate= schedule.DepatureDate }).ToList();

                            foreach(var dateTravell in datesTravell) {
                                if (dateTravell.departureDate.Equals(date))
                                {
                                    DrivingLineDTO dto = new DrivingLineDTO
                                    {
                                        Id = dtoId++,
                                        Departure = s.departureName,
                                        Date = date,
                                        Time = s.departureTime,
                                        Train = until.TrainName,
                                        Arrival = arrival.Name,
                                        FromStationScheduleId = s.FromScheduleStationId,
                                        UntilStationScheduleId = until.UntilStationSchedule,
                                        ScheduleId = dateTravell.scheduleId,
                                        drivingLine = s.DrivinglineId
                                    };
                                    DTOs.Add(dto);
                                    correct = false;
                                }
                            }
                        }
                    }
                }
            }
            return DTOs;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            this.date = picker.SelectedDate;
        }


        private void btnUzmi_Click(object sender, RoutedEventArgs e)
        {
            var dtoId = ((Button)sender).Tag;
            DrivingLineDTO dto = findDTOById((int)dtoId);
            GetTicketDTO getTicketDTO = new GetTicketDTO {
                DrivingLineId = dto.drivingLine, 
                FromStationScheduleId = dto.FromStationScheduleId,
                UntilStationScheduleId = dto.UntilStationScheduleId,
                ScheduleId = dto.ScheduleId
            };
            this.parentFrame.Content = new GetTicketPage(getTicketDTO, this.loggedUser);
        }

        private void btnDetalji_Click(object sender, RoutedEventArgs e)
        {
            var drivinLineId = ((Button)sender).Tag;
            this.parentFrame.Content = new DetailsTimetable((int)drivinLineId);
        }

        private DrivingLineDTO findDTOById(int dtoId) {
            foreach (DrivingLineDTO dto in this.lines) {
                if (dto.Id == dtoId) {
                    return dto;
                }
            }
            return null;
        }
    }
}
