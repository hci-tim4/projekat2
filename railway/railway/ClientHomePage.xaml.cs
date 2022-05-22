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

namespace railway
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ClientHomePage : Window
    {

        string last = "";
        User loggedUser;
        Page clientTimetable;

        public ClientHomePage(User user)
        {
            InitializeComponent();
            loggedUser = user;
            clientTimetable = new clientTimetable.Timetable(loggedUser, page1);
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
                        page1.Content = this.clientTimetable;
                        break;

                    case "Pregled karata":
                        last = "Pregled karata";
                        break;

                    default:
                        return;
                }
            }
        }

 

    }
}
