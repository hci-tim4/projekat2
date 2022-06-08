using railway.clientTimetable;
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
        public User loggedUser { get; set; }


        public ClientHomePage(User user)
        {
            loggedUser = user;
            InitializeComponent();
            //clientTimetable = new clientTimetable.Timetable(loggedUser, page1);
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
                        //page1.Content = this.clientTimetable;
                        break;

                    case "Pregled karata":
                        last = "Pregled karata";
                        // page2.Content = new TicketsView(loggedUser);
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
                    page.Content = new Timetable(loggedUser);

                    break;
                case 1:
                    page.Content = "";
                    page.Content = new TicketsView(loggedUser);
                    break;
                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Window login = new MainWindow();
            //App.Current.MainWindow.Close();
            this.Close();
            login.Show();
        }
    }
}