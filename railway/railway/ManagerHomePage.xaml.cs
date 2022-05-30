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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace railway
{
    /// <summary>
    /// Interaction logic for ManagerHomePage.xaml
    /// </summary>
    public partial class ManagerHomePage : Window
    {
        User loggedUser;
        string last = "";
        public ManagerHomePage(User user)
        {
            InitializeComponent();
            loggedUser = user;
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
                        break;
                    case "Vozne linije":
                        last = "Vozne linije";
                        break;
                    case "Vozovi":
                        last = "Vozovi";
                        page3.Content = new CRUDTrains();
                        break;
                    case "Pregled prodatih karata":
                        last = "Pregled karata";
                        break;

                    default:
                        return;
                }
            }
        }
        
    }
}
