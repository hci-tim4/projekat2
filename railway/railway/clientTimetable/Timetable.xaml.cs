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
        private int id = 0;
        DrivingLineDTO currentSelected;

        public Timetable(User user)
        {
            InitializeComponent();
            //parentFrame = page1;
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
                if (this.lines.Count == 0)
                {
                    this.lines.AddRange(FindWithTransfer(this.departure, this.arrival, (DateTime)this.date));
                }
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
            List<int> fromStationScheduleIds = new List<int>();
            List<int> toStationScheduleIds = new List<int>();
            int t = -1;

            using (var db = new RailwayContext())
            {
                var stations =
                    (from station in db.stations
                     join stationSchedule in db.stationsSchedules
                     on station.Id equals stationSchedule.Station.Id
                     where station.Id == departure.Id && stationSchedule.deleted == false
                     select new
                     {
                         departureName = station.Name,
                         departureId = station.Id,
                         departureTime = stationSchedule.DepartureTime,
                         serialNumber = stationSchedule.SerialNumber,
                         DrivinglineId = stationSchedule.DrivingLineId,
                         FromScheduleStationId = stationSchedule.Id,
                         Tour = stationSchedule.Tour
                     }).ToList();


                foreach (var s in stations) {

                    var drivingLineStations =
                        (from DrivingLine in db.drivingLines
                         join stat in db.stationsSchedules
                         on DrivingLine.Id equals stat.DrivingLineId
                         join train in db.trains
                         on DrivingLine.TrainId equals train.Id
                         where DrivingLine.Id == s.DrivinglineId && stat.Tour ==s.Tour && stat.deleted==false && DrivingLine.deleted == false
                         select new
                         {
                             EndStationId = stat.StationId,
                             UntilStationSchedule = stat.Id,
                             TrainName = train.Name,
                             Tour = s.Tour
                         }).ToList();

                  
                    foreach (var until in drivingLineStations) {
                        
                        if (departure.Id == until.EndStationId) {
                            correct=true;
                            t = until.Tour;
                        }
                        if ((arrival.Id == until.EndStationId) && (correct==true) && (t == until.Tour)) {
                            var datesTravell =
                                (from schedule in db.schedules
                                where schedule.DrivingLineId == s.DrivinglineId
                                select new { 
                                    scheduleId = schedule.Id,
                                    departureDate= schedule.DepatureDate }).ToList();

                            foreach(var dateTravell in datesTravell) {
                                if (dateTravell.departureDate.Equals(date) && (!fromStationScheduleIds.Contains(s.FromScheduleStationId))
                                    &&(!toStationScheduleIds.Contains(until.UntilStationSchedule)))
                                {
                                    DrivingLineDTO dto = new DrivingLineDTO
                                    {
                                        Id = ++id,
                                        Departure = s.departureName,
                                        Date = date,
                                        Time = s.departureTime,
                                        Train = until.TrainName,
                                        Arrival = arrival.Name,
                                        FromStationScheduleId = s.FromScheduleStationId,
                                        UntilStationScheduleId = until.UntilStationSchedule,
                                        ScheduleId = dateTravell.scheduleId,
                                        drivingLine = s.DrivinglineId,
                                        Tour = s.Tour
                                    };
                                    DTOs.Add(dto);
                                    fromStationScheduleIds.Add(dto.FromStationScheduleId);
                                    toStationScheduleIds.Add(dto.UntilStationScheduleId);
                                    correct = false;
                                    t = -1;
                                }
                            }
                        }
                    }
                }
            }
            return DTOs;
        }

        private List<DrivingLineDTO> FindWithTransfer(Station departure, Station arrival, DateTime date) {

            List<DrivingLineDTO> DTOs = new List<DrivingLineDTO>();

            using (var db = new RailwayContext())
            {
                var startStations =
                    (from station in db.stations
                     join stationSchedule in db.stationsSchedules
                     on station.Id equals stationSchedule.Station.Id
                     where station.Id == departure.Id
                     select new
                     {
                         departureName = station.Name,
                         departureId = station.Id,
                         departureTime = stationSchedule.DepartureTime,
                         startArrivalTime = stationSchedule.ArrivalTime,
                         serialNumber = stationSchedule.SerialNumber,
                         DrivinglineId = stationSchedule.DrivingLineId,
                         FromScheduleStationId = stationSchedule.Id,
                         Tour = stationSchedule.Tour

                     }).ToList();


                var endStations =
                    (from station in db.stations
                     join stationSchedule in db.stationsSchedules
                     on station.Id equals stationSchedule.Station.Id
                     where station.Id == arrival.Id
                     select new
                     {
                         departureName = station.Name,
                         departureId = station.Id,
                         departureTime = stationSchedule.DepartureTime,
                         endArrivalTime = stationSchedule.ArrivalTime,
                         serialNumber = stationSchedule.SerialNumber,
                         DrivinglineId = stationSchedule.DrivingLineId,
                         ToScheduleStationId = stationSchedule.Id,
                         Tour = stationSchedule.Tour

                     }).ToList();



                foreach (var start in startStations) {
                    var stationsForStartDrivinLine =             // sve stanice driving line-a sa polazistem
                         (from drivingLine in db.drivingLines
                          join stationSchedule in db.stationsSchedules
                          on drivingLine.Id equals stationSchedule.DrivingLineId
                          join station in db.stations
                          on stationSchedule.StationId equals station.Id
                          join schedule in db.schedules
                          on drivingLine.Id equals schedule.DrivingLineId
                          join train in db.trains
                          on drivingLine.TrainId equals train.Id
                          where drivingLine.Id == start.DrivinglineId && stationSchedule.Tour == start.Tour
                          select new
                          {
                              stationName = station.Name,
                              stationId = station.Id,
                              stationDepartureTime = stationSchedule.DepartureTime,
                              stationArrivalTime = stationSchedule.ArrivalTime,
                              date = schedule.DepatureDate,
                              drivingLineId = drivingLine.Id,
                              trainName = train.Name,
                              stationScheduleId = stationSchedule.Id,
                              scheduleId = schedule.Id,
                              Tour = stationSchedule.Tour

                          }).ToList();


                    foreach (var end in endStations) {          // sve stanice driving line-a sa odredistem
                        var stationsForEndDrivingLine =
                            (from drivingLine in db.drivingLines
                             join stationSchedule in db.stationsSchedules
                             on drivingLine.Id equals stationSchedule.DrivingLineId
                             join station in db.stations
                             on stationSchedule.StationId equals station.Id
                             join schedule in db.schedules
                             on drivingLine.Id equals schedule.DrivingLineId
                             join train in db.trains
                             on drivingLine.TrainId equals train.Id
                             where drivingLine.Id==end.DrivinglineId && stationSchedule.Tour == end.Tour

                             select new 
                             {
                                 stationName = station.Name,
                                 stationId = station.Id,
                                 stationDepartureTime = stationSchedule.DepartureTime,
                                 stationArrivalTime = stationSchedule.ArrivalTime,
                                 date = schedule.DepatureDate,
                                 drivingLineId = drivingLine.Id,
                                 trainName = train.Name,
                                 stationScheduleId = stationSchedule.Id,
                                 scheduleId = schedule.Id,
                                 DrivingLineId = drivingLine.Id,
                                 Tour = stationSchedule.Tour
                             

                             }).ToList();

                        foreach (var startMed in stationsForStartDrivinLine) 
                        {
                            foreach (var endMed in stationsForEndDrivingLine)
                            { 
                                    if (startMed.stationName == departure.Name) {
                                        continue;
                                    }
                            
                                    if (startMed.stationId == endMed.stationId && startMed.stationArrivalTime < endMed.stationDepartureTime &&
                                        startMed.date == endMed.date && startMed.date == date && start.DrivinglineId != endMed.DrivingLineId 
                                       )
                                    {
                                        DrivingLineDTO dto1 = new DrivingLineDTO
                                        {
                                            Departure = departure.Name,
                                            Arrival = startMed.stationName,
                                            Time = start.departureTime,
                                            Date = startMed.date,
                                            Train = startMed.trainName,

                                            FromStationScheduleId = start.FromScheduleStationId,
                                            UntilStationScheduleId = startMed.stationScheduleId,
                                            ScheduleId = startMed.scheduleId,
                                            drivingLine = start.DrivinglineId,
                                            Id = ++id,
                                            Tour = start.Tour
                                        };




                                        DrivingLineDTO dto2 = new DrivingLineDTO
                                        {
                                            Id = ++id,
                                            Departure = startMed.stationName,

                                            Time = endMed.stationDepartureTime,
                                            Train = endMed.trainName,
                                            Date = endMed.date,

                                            FromStationScheduleId = endMed.stationScheduleId,
                                            ScheduleId = endMed.scheduleId,
                                            drivingLine = endMed.DrivingLineId,
                                            Tour = endMed.Tour
                                        };

                                        DTOs.Add(dto1);
                                        DTOs.Add(dto2);

                                    }
                                    if (endMed.stationId == arrival.Id)
                                    {
                                        foreach (var dto in DTOs)
                                        {
                                            if (dto.Id == id)
                                            {
                                                dto.UntilStationScheduleId = endMed.stationScheduleId;
                                                dto.Arrival = endMed.stationName;
                                            }
                                        }
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
            //this.parentFrame.Content = new GetTicketPage(getTicketDTO, this.loggedUser, parentFrame, this);

        }
        private void btnReserve_Click(object sender, RoutedEventArgs e) { }

        private GetTicketDTO getTicketDTO(int id)
        {
            DrivingLineDTO dto = findDTOById(id);
            GetTicketDTO getTicketDTO = new GetTicketDTO
            {
                DrivingLineId = dto.drivingLine,
                FromStationScheduleId = dto.FromStationScheduleId,
                UntilStationScheduleId = dto.UntilStationScheduleId,
                ScheduleId = dto.ScheduleId
            };
            return getTicketDTO;

        }
        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            var dtoId = ((Button)sender).Tag;
            GetTicketDTO TicketDTO = getTicketDTO((int)dtoId);

        }

        private void btnDetalji_Click(object sender, RoutedEventArgs e)
        {
            var dtoId = ((Button)sender).Tag;
            DrivingLineDTO dto = findDTOById((int)dtoId);
            this.parentFrame.Content = new DetailsTimetable(dto.Tour, departure.Id, arrival.Id, parentFrame, this, dto.drivingLine);
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
