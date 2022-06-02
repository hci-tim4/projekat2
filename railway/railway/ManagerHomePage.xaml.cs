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
        public ManagerHomePage(User user)
        {
            InitializeComponent();
            this.loggedUser = user;
            this.managerSchedule = new ManagerSchedule();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            if (!tabItem.Equals(last))
            {
                switch (tabItem)
                {
                    case "Mesečni izveštaj":
                        last = "Mesečni izveštaj";
                      //  page1.Content = ;
                        break;

                    case "Linijski izveštaj":
                        last = "Linijski izveštaj";
                  //      page2.Content = ;
                        break;

                    case "Red vožnje":
                        last = "Red vožnje";
                        page3.Content = this.managerSchedule;
                        break;

                    case "Mrežna linija":
                        last = "Mrežna linija";
                  //    page4.Content = ;
                        break;

                    case "Vozovi":
                        last = "Vozovi";
                     //page5.Content = ;
                        break;

                    case "Pomoć":
                        last = "Pomoć";
                    //  page6.Content = ;
                        break;
                    default:
                        return;
                }
            }
        }



    }
}
