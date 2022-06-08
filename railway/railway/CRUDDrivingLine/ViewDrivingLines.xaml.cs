using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using railway.database;
using railway.model;
using System.Linq;
using railway.CRUDDrivingLine;
using railway.exception;
using railway.services;
using System.Windows.Input;

namespace railway.defineDrivingLine
{
    public partial class ViewDrivingLines : UserControl
    {
        //public List<DrivingLineViewDTO> DrivingLines;
        private List<Train> trains;
        private List<Train> fullTrains;
        private Train newTrain;
        private Frame parentFrame;
        private DrivingLineViewDTO currentSelected = null;
        private Station currentStation;
        private List<Station> stations;
        
        public ViewDrivingLines(Frame frame)
        {
            InitializeComponent();
            this.parentFrame = frame;
            this.parentFrame.Content = this;
            //this.DataContext = DrivingLines;
            drivingLineViewPage.Content = new DrivingLines(drivingLineViewPage);
            /*using (var db = new RailwayContext())
            {
                setDrivingLines(db);
                trains = (from trains in db.trains
                    select trains).ToList();
                trainNameCmb.ItemsSource = trains;
                fullTrains = trains;
                stations = (from st in db.stations orderby st.Name select st).ToList();
            }*/
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
        
        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if(drivingLineViewPage.NavigationService.CanGoBack)
                drivingLineViewPage.NavigationService.GoBack();
        }

        private void Forward_OnClick(object sender, RoutedEventArgs e)
        {
            if(drivingLineViewPage.NavigationService.CanGoForward)
                drivingLineViewPage.NavigationService.GoForward();
        }

        private void GoBack_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = drivingLineViewPage.NavigationService.CanGoBack;
            MessageBox.Show(drivingLineViewPage.NavigationService.Source.ToString());
        }

        private void GoBack_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            drivingLineViewPage.NavigationService.GoBack();
        }

        private void GoForward_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = drivingLineViewPage.NavigationService.CanGoForward;
        }

        private void GoForward_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            drivingLineViewPage.NavigationService.GoForward();
        }

    }
}