using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using railway.client;

namespace railway
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Window> openedWindows = new List<Window>();
        private GetTicketPage getTicketPage;
        public MainWindow()
        {
            InitializeComponent();
            //FillDatabase fd = new FillDatabase();
            //fd.fill();
            getTicketPage = new GetTicketPage(new GetTicketDTO()
            {
                FromStationScheduleId = 1,
                UntilStationScheduleId = 2,
                ScheduleId = 1,
                DrivingLineId = 1
            }, new model.User());
            page.Content = getTicketPage;
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
            List<Window> openWindows = getTicketPage.openedWindows;
            foreach (Window w in openWindows)
            {
                w.Close();
            }
        }
    }
}
