using railway.database;
using railway.model;
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
using System.Linq;
namespace railway.managerSchedule
{
    /// <summary>
    /// Interaction logic for ChangeTrafficDayModal.xaml
    /// </summary>
    public partial class ChangeTrafficDayModal : UserControl
    {
        DrivingLineDTO drivingLine;
        private ManagerSchedule parentPage;


        private bool _hideRequest = false;
        private bool _result = false;
        private UIElement _parent;


        public ChangeTrafficDayModal()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;

        }

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        public bool ShowHandlerDialog(DrivingLineDTO currentDTO, ManagerSchedule parent)   /* konstruktor */
        {
           
            this.drivingLine = currentDTO;
            this.DataContext = drivingLine;
            this.parentPage = parent;
            stack.DataContext = drivingLine;

            // drivingLineStackPanel.DataContext = drivingLine;
            //askingForConfirmLabel.Content = "Krajnji datum mrežne linije";
            //this.defineEndDate = new DefineEndDateForDrivingLine(dto, drivingLinesView);
            //confirmationDataFrame.Content = defineEndDate;
            Visibility = Visibility.Visible;

            _parent.IsEnabled = false;

            IsEnabled = true;
            Focus();


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

        private void changeDays_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new RailwayContext())
            {
                var tr =
                    from days in db.TrafficDays
                    where days.DrivingLineId == drivingLine.drivingLineId
                    select days;

                foreach (var t in tr)
                {
                    db.TrafficDays.Remove(t);
                }
                db.SaveChanges();


                if (drivingLine.Monday)
                {
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

               // MessageBox.Show("Izmenili ste dane u kojima saobraća linija.");
                this.parentPage.setAllDrivingLines();
            }

            _result = true;
            HideHandlerDialog();
        }

        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }



        private void cancelChangeDays_Click(object sender, RoutedEventArgs e)
        {
            _result = false;
            HideHandlerDialog();
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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[2]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }



    }
}
