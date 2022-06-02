using railway.dto.trains;
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

namespace railway
{
    /// <summary>
    /// Interaction logic for CRUDTrains.xaml
    /// </summary>
    public partial class CRUDTrains : Page
    {
        List<TrainDTO> dto;
        public CRUDTrains()
        {
            InitializeComponent();
            //loggedUser = user;

            dto = TrainService.getTrains();

            this.DataContext = dto;
            dataGrid.ItemsSource = dto;
        }

        private void edit_clicked(object sender, RoutedEventArgs e)
        {

        }

        private void delete_clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
