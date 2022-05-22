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
        User loggedUser;
        public ClientHomePage(User user)
        {
            InitializeComponent();
            loggedUser = user;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            switch (tabItem)
            {
                case "Red vožnje":
                    page1.Content = new clientTimetable.Timetable();
                    break;

                case "Pregled karata":
                    
                    page2.Content = new TicketsView(loggedUser);
                    break;

                default:
                    return;
            }
        }

    }
}
