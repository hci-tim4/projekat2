using railway.model;
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
using railway.managerSchedule;
using railway.defineDrivingLine;
using System.Windows.Navigation;
using System.Windows.Shapes;
using railway.monthlyReport;


namespace railway
{
    /// <summary>
    /// Interaction logic for ManagerHomePage.xaml
    /// </summary>
    public partial class ManagerHomePage : Window
    {

        string last = "";
        User loggedUser;
        Page managerSchedule;
        UserControl viewDrivinglines;
        UserControl monthlyReport;
        public ManagerHomePage(User user)
        {
            InitializeComponent();
            this.loggedUser = user;
            this.managerSchedule = new ManagerSchedule();
            this.viewDrivinglines = new ViewDrivingLines(page2);
            this.monthlyReport = new ViewMonthlyTicketView();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            if (!tabItem.Equals(last))
            {
                switch (tabItem)
                {

                    case "Red vožnje":
                        last = "Red vožnje";
                        page1.Content = this.managerSchedule;
                        break;
                    case "Vozne linije":
                        last = "Vozne linije";
                        page2.Content = this.viewDrivinglines;
                        break;
                    case "Vozovi":
                        last = "Vozovi";
                        page3.Content = new CRUDTrains();
                        break;
                    case "Mesečni izveštaj":
                        last = "Mesečni izveštaj";
                        page4.Content = this.monthlyReport;
                        break;
                    case "Izveštaj za mrežu linija":
                        last = "Izveštaj za mrežu linija";
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
