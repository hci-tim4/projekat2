using railway.database;
using railway.dto.tickets_view;
using railway.model;
using railway.services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using railway.database;
using railway.model;
using railway.client;
using System.Linq;

namespace railway
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Window> openedWindows = new List<Window>();
        //private GetTicketPage getTicketPage;
        public MainWindow()
        {
            InitializeComponent();
           // FillDatabase fd = new FillDatabase();
            //fd.fill();
            //page.Content = new Login();
/*
            using (var db = new RailwayContext())
            {
                List<StationSchedule> ss = (from stationSchedules in db.stationsSchedules
                                            where stationSchedules.DrivingLineId == 1
                                            orderby stationSchedules.SerialNumber
                                            select stationSchedules).ToList();

                page.Content = new map.Map(ss);
            }
//*/


            //FillDatabase fd = new FillDatabase();
            //fd.fill();
            page.Content = new Login();
            /*
            getTicketPage = new GetTicketPage(new GetTicketDTO()
            {
                FromStationScheduleId = 1,
                UntilStationScheduleId = 2,
                ScheduleId = 1,
                DrivingLineId = 1
            }, new model.User());
            page.Content = getTicketPage;*/
        }

        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          /* 
>>>>>>> 53b025e3cee3613ea9cb6e9f3c5f7ec3a97d55e1
            List<Window> openWindows = getTicketPage.openedWindows;
            foreach (Window w in openWindows)
            {
                w.Close();
            }*/
        }
        
    }
}
