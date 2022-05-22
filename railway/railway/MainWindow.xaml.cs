using railway.database;
using railway.dto.tickets_view;
using railway.model;
using railway.services;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace railway
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            FillDatabase fd = new FillDatabase();
            fd.fill();

            page.Content = new Login();
        }
    }
}
