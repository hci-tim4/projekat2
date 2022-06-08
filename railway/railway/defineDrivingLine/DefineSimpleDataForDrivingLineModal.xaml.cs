using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using railway.database;
using railway.exception;
using railway.model;
using railway.services;

namespace railway.defineDrivingLine
{
    public partial class DefineSimpleDataForDrivingLineModal : UserControl
    {
        
        
        private List<Train> trains;
        private List<Train> fullTrains;
        private Train newTrain = null;
        private ObservableCollection<Station> stations;
        private DrivingGotSavedHandler drivingGotSavedHandler;

        public DefineSimpleDataForDrivingLineModal()
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

        public bool ShowHandlerDialog(ObservableCollection<Station> stations,
            DrivingGotSavedHandler drivingGotSavedHandler)
        {
            
            
            simpleDataGrid.DataContext = this;
            using (var db = new RailwayContext())
            {
                trains = (from trains in db.trains
                    select trains).ToList();
                trainNameCmb.ItemsSource = trains;
                this.fullTrains = (from trains in db.trains
                    select trains).ToList();
            }

            this.stations = stations;
            this.drivingGotSavedHandler = drivingGotSavedHandler;
            
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

        private void confirmBtn_Click_1(object sender, RoutedEventArgs e)
        {
            //defineEndDate.saveDate();
            _result = true;
            //save driving line
            saveDrivingLineInDB();
            HideHandlerDialog();
        }

        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

      

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
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

        
        private void SaveDrivingLine_OnClick(object sender, RoutedEventArgs e)
        {
            saveDrivingLineInDB();
        }

        private void saveDrivingLineInDB()
        {

            string name = drivingLineName.Text;
            DrivingLineService dlService = new DrivingLineService();
            try
            {
                //MessageBox.Show(startSelectedDate+"");
                DateTime ?startSelectedDate = startDate.SelectedDate;
                if (startSelectedDate == null)
                {
                    MessageBox.Show("Početni datum mora da postoji");
                    return;
                }
                dlService.saveDrivingService(name, newTrain, (DateTime)startSelectedDate, stations);
                MessageBox.Show("Mrežna linija je uspešno sačuvana");
                drivingGotSavedHandler();
            }
            catch (NotDefinedException nd)
            {
                MessageBox.Show(nd.message);
            }
            catch (AlreadyDefinedException ad)
            {
                MessageBox.Show(ad.message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show("Ups, neočekivana greška se desilo");
            }
        }
    

        private void SaveDrivingLineAndDefineSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void TrainNameChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            var trainNameCmb = sender as ComboBox;

            //this. = trainNameCmb.Text.ToLower();
            this.newTrain = (from item in trains
                where item.Name.ToLower().Equals(trainNameCmb.Text.ToLower())
                select item).FirstOrDefault();


            this.trains = (from item in fullTrains
                where item.Name.ToLower().Contains(trainNameCmb.Text.ToLower())
                select item).ToList();
            trainNameCmb.ItemsSource = trains;

            trainNameCmb.IsDropDownOpen = true;
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
        
        
        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            confirmBtn_Click_1(sender, e);
        }

    }
}