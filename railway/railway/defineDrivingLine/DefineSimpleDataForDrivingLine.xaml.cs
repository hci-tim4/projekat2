using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using railway.database;
using railway.model;
using System.Linq;
using System.Windows.Controls;
using railway.exception;
using railway.services;
using System.Windows.Input;

namespace railway.defineDrivingLine
{
    public partial class DefineSimpleDataForDrivingLine : Window
    {
        private List<Train> trains;
        private List<Train> fullTrains;
        private Train newTrain = null;
        private ObservableCollection<Station> stations;
        private DrivingGotSavedHandler drivingGotSavedHandler;
        
        public DefineSimpleDataForDrivingLine(ObservableCollection<Station> stations,
            DrivingGotSavedHandler drivingGotSavedHandler)
        {
            InitializeComponent();
            this.DataContext = this;
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
                this.Close();
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
    }
}