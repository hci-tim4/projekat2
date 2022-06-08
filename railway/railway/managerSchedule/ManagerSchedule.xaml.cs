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
using railway.clientTimetable;
using System.ComponentModel;
using System.Collections.ObjectModel;
using railway.model;

namespace railway.managerSchedule
{
    /// <summary>
    /// Interaction logic for ManagerSchedule.xaml
    /// </summary>
    public partial class ManagerSchedule : Page, INotifyPropertyChanged
    {
        public List<DrivingLineDTO> lines { get; set; }
        public DrivingLineDTO currentSelected { get; set; }
        public ObservableCollection<DetailDrivinglineDTO> Studenti { get;  set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ManagerSchedule()
        {
            InitializeComponent();
            lines = new List<DrivingLineDTO>();
            setAllDrivingLines();
            drivingLineDataGrid.ItemsSource = this.lines;
         
            this.DataContext =this;

        }

        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private ICollectionView _View;
        public ICollectionView View
        {
            get
            {
                return _View;
            }
            set
            {
                _View = value;
                OnPropertyChanged("View");
            }
        }

        private bool _GroupView;
        public bool GroupView
        {
            get
            {
                return _GroupView;
            }
            set
            {
                if ( View != null)
                {
                    View.GroupDescriptions.Clear();
                    if (value)
                    {
                        View.GroupDescriptions.Add(new PropertyGroupDescription("Tour"));
                    }
                    _GroupView = value;
                    OnPropertyChanged("GroupView");

                    OnPropertyChanged("View");
                }
            }
        }
        private void DrivingLineDataGrid_OnSelected(object sender, RoutedEventArgs e)
        {
            
            currentSelected = drivingLineDataGrid.SelectedItem as DrivingLineDTO;

            
            Studenti = new ObservableCollection<DetailDrivinglineDTO>(currentSelected.schedule);
            
            View = CollectionViewSource.GetDefaultView(Studenti);
            GroupView = true;
            dataGridStationSchedule.ItemsSource = Studenti;
        }

        public void setAllDrivingLines()
        {
            List<DrivingLineDTO> linesDB = new List<DrivingLineDTO>();
            using (var db = new RailwayContext())
            {
                var allDrivinLines =
                    (from drivingLine in db.drivingLines
                     join traffic in db.TrafficDays
                     on drivingLine.Id equals traffic.DrivingLineId
                     into groupDays
                     select new
                     {
                         drivingLineName = drivingLine.Name,
                         trafficDays = groupDays,
                         drivingLineId = drivingLine.Id

                     }).ToList();

                foreach (var driving in allDrivinLines) {
                    List<string> days = new List<string>();
                    foreach (var day in driving.trafficDays) {
                        days.Add(day.Day.ToString());
                    }
                    List<DetailDrivinglineDTO> schedule = getStationSchedule(driving.drivingLineId);
                    linesDB.Add(new DrivingLineDTO(driving.drivingLineId, driving.drivingLineName, days, schedule));
                }
                this.lines = linesDB;
                drivingLineDataGrid.ItemsSource = this.lines;
            }
        }

        private List<DetailDrivinglineDTO> getStationSchedule(int drivingLineId) {

            List<DetailDrivinglineDTO> detailDrivinglineDTOs = new List<DetailDrivinglineDTO>();
            using (var db = new RailwayContext())
            {
                var stations =
                    (from drivingLine in db.drivingLines
                     join stationSchedule in db.stationsSchedules
                     on drivingLine.Id equals stationSchedule.DrivingLineId
                     join station in db.stations
                     on stationSchedule.StationId equals station.Id
                     where drivingLine.Id == drivingLineId && stationSchedule.deleted == false
                     select new
                     {
                         StationName = station.Name,
                         ArrivalTime = stationSchedule.ArrivalTime,
                         DepartureTime = stationSchedule.DepartureTime,
                         Tour = stationSchedule.Tour,
                         StationScheduleId = stationSchedule.Id
                     }).ToList();

                foreach (var s in stations)
                {
                    detailDrivinglineDTOs.Add(new DetailDrivinglineDTO
                    {
                        StationName = s.StationName,
                        ArrivalTime = s.ArrivalTime,
                        DepartureTime = s.DepartureTime,
                        Tour = s.Tour,
                        StationScheduleId = s.StationScheduleId
                    });
                }
                return detailDrivinglineDTOs;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            var drivingLineId = ((Button)sender).Tag;
            Window addNewSchedule = new AddNewScheduleWindow((int)drivingLineId, this);
            addNewSchedule.Show();
        }

        private void btnChangeDays_Click(object sender, RoutedEventArgs e)
        {
            var drivingLineId = ((Button)sender).Tag;
            setAllDrivingLines();
            List<DrivingLineDTO> all = this.lines;
            DrivingLineDTO dlDTO =
                (from dto in all
                 where dto.drivingLineId == (int)drivingLineId
                 select dto).FirstOrDefault();


            Window changeDays = new ChangeTrafficDay(dlDTO, this);
            changeDays.Show();
            changeDays.Focus();

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var tour = ((Button)sender).Tag;
            updateStationSchedule((int)tour);

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var tour = ((Button)sender).Tag;
            deleteStationschedule((int)tour);

        }

        private void updateStationSchedule(int tour) {
            using (var db = new RailwayContext()) 
            {
                var station = (from stationSchedule in db.stationsSchedules
                      where stationSchedule.Tour == tour
                               select stationSchedule).ToList();


                foreach (var selected in currentSelected.schedule) {
                    foreach (var s in station)
                    { 
                        if (s.Tour == selected.Tour && s.Id == selected.StationScheduleId)
                        {
                            s.ArrivalTime = selected.ArrivalTime;            // ovde pravi noviii sa datumom od kada vayi
                            s.DepartureTime = selected.DepartureTime;
                            s.StartDate = DateTime.Now.AddDays(30);
                            break;
                        }
                    }
                }

                db.SaveChanges();
                setAllDrivingLines();
                MessageBox.Show("Izmenili ste red voznje za turu "+ tour +". Izmene će biti vidljive nakon mesec dana.");

            }
        }

        private void deleteStationschedule(int tour) {
            int drivingLineId = this.currentSelected.drivingLineId;

            using (var db = new RailwayContext()) 
            {
                var stations =
                    from stationSchedule in db.stationsSchedules
                    where stationSchedule.DrivingLineId == drivingLineId &&
                    stationSchedule.Tour == tour
                    select stationSchedule;

                foreach (var s in stations) 
                {
                    s.deleted = true;
                    s.StartDate = DateTime.Now.AddDays(30);
                }
                db.SaveChanges();
                setAllDrivingLines();
                MessageBox.Show("Obrisali ste red voznje. Izmene će biti vidljive nakon mesec dana.");

            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        public void doThings(string param)
        {
       //     btnOK.Background = new SolidColorBrush(Color.FromRgb(32, 64, 128));
            Title = param;
        }

    }
}
