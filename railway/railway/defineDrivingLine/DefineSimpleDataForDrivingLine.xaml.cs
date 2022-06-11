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
using System.Windows.Markup;
using railway.managerSchedule;

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
            try{
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
                startDate.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
                
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void SaveDrivingLine_OnClick(object sender, RoutedEventArgs e)
        {
            try{
                saveDrivingLineInDB();
                
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void saveDrivingLineInDB()
        {

            string name = drivingLineName.Text;
            DrivingLineService dlService = new DrivingLineService();
            try
            {
                DateTime ?startSelectedDate = startDate.SelectedDate;
                if (startSelectedDate == null)
                {
                    CustomMessageBox cmb1 = new CustomMessageBox("Početni datum mora da postoji");
                    cmb1.ShowDialog();
                    return;
                }else if (startSelectedDate < DateTime.Now)
                {
                    CustomMessageBox cmb2 =
                        new CustomMessageBox("Početni datum mora da bude veći \nod današnjeg datuma");
                    cmb2.ShowDialog();
                    return;
                }
                DrivingLine dl = dlService.saveDrivingService(name, newTrain, (DateTime)startSelectedDate, stations);
                Window win = new AddNewScheduleWindow(dl.Id, null);
                win.ShowDialog();
                CustomMessageBox cmb = new CustomMessageBox("Mrežna linija je uspešno sačuvana");
                cmb.ShowDialog();
                drivingGotSavedHandler();
                this.Close();
            }
            catch (NotDefinedException nd)
            {
                CustomMessageBox cmb1 = new CustomMessageBox(nd.message);
                cmb1.ShowDialog();
            }
            catch (AlreadyDefinedException ad)
            {
                CustomMessageBox cmb1 = new CustomMessageBox(ad.message);
                cmb1.ShowDialog();
            }
            catch (Exception e)
            {
                CustomMessageBox cmb1 = new CustomMessageBox("Ups, neočekivana greška se desilo");
                cmb1.ShowDialog();
            }
        }
    

        private void SaveDrivingLineAndDefineSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void TrainNameChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
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
        
        
        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveDrivingLine_OnClick(sender, e);
        }
    }
}