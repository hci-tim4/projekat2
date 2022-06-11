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
using System.Windows.Shapes;
using System.Linq;
using railway.model;

namespace railway.managerSchedule
{
    /// <summary>
    /// Interaction logic for AddNewScheduleWindow.xaml
    /// </summary>
    public partial class AddNewScheduleWindow : Window
    {
        public int drivingLineId;
        public List<ScheduleDTO> newSchedule = new List<ScheduleDTO>();
        private ManagerSchedule parentPage;

        public AddNewScheduleWindow(int drivingLineId, ManagerSchedule parent)
        {
            InitializeComponent();
            this.drivingLineId = drivingLineId;
            this.parentPage = parent;
            this.newSchedule = findStationsForDrivingLine(drivingLineId);
            this.DataContext = newSchedule;
            addDataGridStationSchedule.ItemsSource = newSchedule;
        }

        private List<ScheduleDTO> findStationsForDrivingLine(int drivingLineId) {

            List<ScheduleDTO> newSchedule = new List<ScheduleDTO>();

            using (var db = new RailwayContext()) 
            {
                var stations =
                    from stationSchedule in db.stationsSchedules
                    join station in db.stations
                    on stationSchedule.StationId equals station.Id
                    where stationSchedule.DrivingLineId == drivingLineId &&  stationSchedule.Tour==1
                    select new
                    {
                        stationId= station.Id,
                        stationName = station.Name,
                        serialNumber = stationSchedule.SerialNumber,
                    };

                var tours =
                    from stationSchedule in db.stationsSchedules
                    where stationSchedule.DrivingLineId == drivingLineId
                    select stationSchedule.Tour;
                int max = 0;
           
                foreach (var t in tours) {
                    if (t > max)
                    { 
                        max = t;
                    }
                }


                foreach (var s in stations) {
                    newSchedule.Add(new ScheduleDTO
                    {
                        StationId = s.stationId,
                        SerialNumber = s.serialNumber,
                        StationName = s.stationName,
                        DrivingLineId = drivingLineId,
                        Tour = max + 1,
                        ArrivalTime = new TimeSpan(),
                        DepartureTime = new TimeSpan()
                        
                    });
                }
                return newSchedule;


            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e) {

            if (validate())
            {

                using (var db = new RailwayContext())
                {
                    foreach (ScheduleDTO schedule in this.newSchedule)
                    {
                        StationSchedule s = new StationSchedule
                        {
                            DrivingLineId = schedule.DrivingLineId,
                            StationId = schedule.StationId,
                            SerialNumber = schedule.SerialNumber,
                            DepartureTime = schedule.DepartureTime,
                            ArrivalTime = schedule.ArrivalTime,
                            Tour = schedule.Tour,
                            StartDate = DateTime.Now
                        };

                        db.stationsSchedules.Add(s);
                    }
                    db.SaveChanges();

                    Window box = new CustomMessageBox("Doodali ste novi red vožnje");
                    box.ShowDialog();
                    parentPage.setAllDrivingLines();
                    this.Close();
                }

            }
            else {
                Window box = new CustomMessageBox("Greška, potrebno je da vreme dolaska\n bude pre vremena polaska.");
                box.ShowDialog();
            }
        }

        private bool validate()
        {
            for (int i = 0; i < this.newSchedule.Count - 1; i++)
            {
                if (newSchedule[i].ArrivalTime >= newSchedule[i].DepartureTime)
                {
                    return false;
                }

                if (newSchedule[i].DepartureTime >= newSchedule[i + 1].ArrivalTime)
                {
                    return false;
                }
            }
            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[2]);
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
        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(sender, e);
        }
    }
}
