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
using railway.drivingLineReport;
using railway.monthlyReport;
using railway.CRUDTrain;

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
        private UserControl drivingLineReport;
        
        public ManagerHomePage(User user)
        {
            InitializeComponent();
            this.loggedUser = user;
            //this.managerSchedule = new ManagerSchedule();
            //this.viewDrivinglines = new ViewDrivingLines(page2);
            //this.monthlyReport = new ViewMonthlyTicketView();
           // this.drivingLineReport = new ViewDrivingLineTicketReport();
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
                       // page1.Content = this.managerSchedule;
                        break;
                    case "Vozne linije":
                        last = "Vozne linije";
                        //page2.Content = this.viewDrivinglines;
                        break;
                    case "Vozovi":
                        last = "Vozovi";
                      //  page3.Content = new CRUDTrains();
                        break;
                    case "Mesečni izveštaj":
                        last = "Mesečni izveštaj";
                       // page4.Content = this.monthlyReport;
                        break;
                    case "Izveštaj za mrežu linija":
                        last = "Izveštaj za mrežu linija";
                       // page5.Content = this.drivingLineReport;
                        break;

                    default:
                        return;
                }
            }
        }


        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    //GridPrincipal.Children.Clear();
                    //GridPrincipal.Children.Add(new Timetable(loggedUser));
                    page.Content = "";
                    page.Content = new ManagerSchedule();
                    //page.Content = 

                    break;
                case 1:
                    page.Content = "";
                    page.Content = new ViewDrivingLines();
                    break;
                case 2:
                    page.Content = new CRUDTrains();
                    break;
                case 3:
                    page.Content = new ViewMonthlyTicketView();
                    break;
                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (50 * index)), 0, 0);
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }


}
