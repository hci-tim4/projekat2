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
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;


namespace railway.managerSchedule
{
    /// <summary>
    /// Interaction logic for ManagerSchedule.xaml
    /// </summary>
    
    public partial class ManagerSchedule : Page, INotifyPropertyChanged, TutorialInterface
    {
        public List<DrivingLineDTO> lines { get; set; }
        public DrivingLineDTO currentSelected { get; set; }
        public ObservableCollection<DetailDrivinglineDTO> Studenti { get;  set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UniqueTimestampValidator validator = new UniqueTimestampValidator();
        private Boolean Touring;

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

            IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals("drivingTable").GoNext();
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
                     where drivingLine.deleted == false
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
            ConfirmDelete conf = new ConfirmDelete();
            conf.ShowDialog();
            if (conf.delete)
            {
                var tour = ((Button)sender).Tag;
                deleteStationschedule((int)tour);
            }
        }

        private void updateStationSchedule(int tour) {
           if (validate(tour)) {
                using (var db = new RailwayContext())
                {
                    var station = (from stationSchedule in db.stationsSchedules
                                   where stationSchedule.Tour == tour
                                   select stationSchedule).ToList();


                    foreach (var selected in currentSelected.schedule)
                    {
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
                    Window box = new CustomMessageBox("Izmenili ste red voznje za turu " + tour + ".");
                    box.ShowDialog();

                }
            }else{
                Window box = new CustomMessageBox("Greška, potrebno je da vreme dolaska\nbude pre vremena polaska.");
                box.ShowDialog();

            }

        }


        private bool validate(int tour)
        {
            var scheduleTour =
                (from s in currentSelected.schedule
                 where s.Tour == tour
                 select s).ToList();

            for (int i = 0; i < scheduleTour.Count - 1; i++)
            {
                if (scheduleTour[i].ArrivalTime >= scheduleTour[i].DepartureTime)
                {
                    return false;
                }

                if (scheduleTour[i].DepartureTime >= scheduleTour[i + 1].ArrivalTime)
                {
                    return false;
                }
            }
            return true;
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
                Window box = new CustomMessageBox("Obrisali ste red vožnje.");
                box.ShowDialog();

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

        public void StartTour_OnClick(object sender, RoutedEventArgs e)
        {
        //MessageBox.Show("Tutorijal");

            Touring = true;
            TextLocalization.Close = "Zatvori";
            TextLocalization.Next = "Sledeći";
            var tour = new Tour()
            {
                Name = "My Demo Tour",
                ShowNextButtonDefault = true,
                EnableNextButtonAlways = true,

                Steps = new[]
                {
                    new Step("drivingTable", "Tebala mrežnih linija", "Prikazan je naziv mrežne linije."),
                    new Step("daysBtn", "Pregled i izmena dana u kojima saobraća linija", "Omogućen je pregled i promena dana u kojima saobraća mrežna linija."),
                    new Step("addNewSchedule", "Dodavanje novog reda vožnje za mrežnu liniju", "Omogućeno je dodavanje novog reda vožnje za mrežnu liniju."),
                    new Step("drivingTable", "Tabela mrežnih linija", "Izaberite jednu od linija za nastavak.")
                    {
                        ShowNextButton = false
                    },
                    new Step("scheduleDataGrid", "Redovi vožnje", "Prikaz svih redova vožnje izabrane mrežne linije"),
                    new Step("arrTime", "Vreme dolaska", "Vreme dolaska predstavlja vreme kada voz stiže na stanicu. Ovo vreme je moguće izmeniti."),
                    new Step("depTime", "Vreme polaska", "Vreme polaska predstavlja vreme kada voz polazi sa stanice. Ovo vreme je moguće izmeniti"),
                    new Step("update", "Izmeni", "Klikom na dugme izmeni, biće sačuvani novo vreme polaska i vreme dolaska za red vožnje."),
                    new Step("delete", "Obriši", "Klikom na dugme obriši, obrisaćete red vožnje.")

                }

            };

            tour.Start();
        }
    }
}
