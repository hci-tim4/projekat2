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
using railway.database;
using railway.model;
using railway.managerSchedule;

namespace railway.managerSchedule
{
    /// <summary>
    /// Interaction logic for ChangeTrafficDay.xaml
    /// </summary>
    public partial class ChangeTrafficDay : Window
    {
        DrivingLineDTO drivingLine;
        private ManagerSchedule parentPage;
        public ChangeTrafficDay(DrivingLineDTO currentDTO, ManagerSchedule parent)
        {
            InitializeComponent();
            this.drivingLine = currentDTO;
            this.DataContext = drivingLine;
            this.parentPage = parent;
        }

        private void changeDays_Click(object sender, RoutedEventArgs e) {
            using (var db = new RailwayContext()) 
            {
                var tr =
                    from days in db.TrafficDays
                    where days.DrivingLineId == drivingLine.drivingLineId
                    select days;

                foreach (var t in tr) {
                    db.TrafficDays.Remove(t);
                }
                db.SaveChanges();


                if (drivingLine.Monday) {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Monday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                if (drivingLine.Tuesday)
                {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Tuesday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                if (drivingLine.Wednesday)
                {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Wednesday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                if (drivingLine.Thursday)
                {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Thursday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                if (drivingLine.Friday)
                {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Friday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                if (drivingLine.Saturday)
                {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Saturday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                if (drivingLine.Sunday)
                {
                    TrafficDays td = new TrafficDays();
                    td.Day = Days.Sunday;
                    td.DrivingLineId = drivingLine.drivingLineId;
                    db.TrafficDays.Add(td);
                }
                db.SaveChanges();
                Window box = new CustomMessageBox("Izmenili ste dane u kojima\nsaobraća linija.");
                box.ShowDialog();
                this.Close();
                this.parentPage.setAllDrivingLines();
            }


        }

        private void cancelChangeDays_Click(object sender, RoutedEventArgs e) {
            this.parentPage.setAllDrivingLines();
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
            changeDays_Click(sender, e);
        }
    }
}
