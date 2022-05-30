using railway.dto.tickets_view;
using railway.model;
using railway.services;
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
using System.Linq;

namespace railway
{
    /// <summary>
    /// Interaction logic for TicketsView.xaml
    /// </summary>
    public partial class TicketsView : Page
    {
        public User loggedUser;
        public List<TicketsDTO> dto;
        public TicketType ttype;
        public TicketsView(User user)
        {
            InitializeComponent();
            loggedUser = user;
            ttype = TicketType.Bought;
            dto = TicketService.getTickets(loggedUser.Id, ttype);

            this.DataContext = dto;
            dataGrid.ItemsSource = dto;
        }

        public TicketsView()
        {
        }

        private void buy_checked(object sender, RoutedEventArgs e)
        {
            ttype = TicketType.Bought;
            dto = TicketService.getTickets(loggedUser.Id, ttype);

            this.DataContext = dto;
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dto;
        }
        private void reserve_checked(object sender, RoutedEventArgs e)
        {
            ttype = TicketType.Reserved;
            dto = TicketService.getTickets(loggedUser.Id, ttype);

            this.DataContext = dto;
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dto;
        }

        private void sortBy_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string chosedSortBy = ((System.Windows.Controls.ComboBoxItem)sortBy.SelectedItem).Content as string;
            switch (chosedSortBy)
            {
                case "Ceni karti":
                    sortByPrice();
                    break;
                case "Datumu vožnje":
                    sortByDate();
                    break;
            }
            this.DataContext = dto;
            dataGrid.ItemsSource = null;

            dataGrid.ItemsSource = dto;

        }

        private void sortByPrice()
        {
            dto = (from t in dto
                   orderby t.Price
                   select t).ToList();

        }

        private void sortByDate()
        {

            dto = (from t in dto
                   orderby t.DepatureDate
                   select t).ToList();
        }

        private void updateDataGrid()
        {

        }

        private void searchBtn(object sender, RoutedEventArgs e)
        {
            string param = ((System.Windows.Controls.ComboBoxItem)searchParameter.SelectedItem).Content as string;
            string value = valuetb.Text;


            if (value == "")
            {
                MessageBox.Show("Niste uneli vrednost za pretragu");
            }
            try
            {
                value = value.Trim();

                getSearchedData(param, value);
            }
            catch
            {

                return;
            }
        }

        private void getSearchedData(string param, string value)
        {
            List<TicketsDTO> tickets_backup = dto;
            if (param == "Polazištu")
            {
                dto = (from t in dto
                       where t.DepartureStationName == value
                       select t).ToList();
            }
            else
            {
                dto = (from t in dto
                       where t.ArrivalStationName == value
                       select t).ToList();
            }

            if (dto.Count() == 0)
            {
                MessageBox.Show("Nema rezultata Vaše pretrage!");
                dto = tickets_backup;
                valuetb.Text = "";
                searchParameter.SelectedIndex = 1;
            }
            else
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = dto;
                MessageBox.Show("Rezultati!");
            }

        }



    }
}
