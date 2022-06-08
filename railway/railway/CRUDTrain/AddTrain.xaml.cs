using railway.database;
using railway.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace railway.CRUDTrain
{
    /// <summary>
    /// Interaction logic for AddTrain.xaml
    /// </summary>
    public partial class AddTrain : Window
    {
        public AddTrain()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new RailwayContext();
            string name = valuetb.Text;
            string color = ((System.Windows.Controls.ComboBoxItem)colorTrain.SelectedItem).Content as string;
            Train t = new Train();
            t.Color = color;
            t.Name = name;
            db.trains.Add(t);

            string colStr = ((System.Windows.Controls.ComboBoxItem)colNumber.SelectedItem).Content as string;
            int col = int.Parse(colStr);

            int regNumber = 0;
            int vipNumber = 0;
            int busNumber = 0;
            string RegNumberStr = regulartb.Text;

            if (!IsNumeric(RegNumberStr))
            {
                MessageBox.Show("Mora biti broj");
            }
            else
            {
                regNumber = int.Parse(RegNumberStr);
            }
            string VipNumberStr = viptb.Text;
            if (!IsNumeric(VipNumberStr))
            {
                MessageBox.Show("Mora biti broj");
            }
            else
            {
                vipNumber = int.Parse(VipNumberStr);
            }
            string BusNumberStr = businesstb.Text;
            if (!IsNumeric(BusNumberStr))
            {
                MessageBox.Show("Mora biti broj");
            }
            else
            {
                busNumber = int.Parse(BusNumberStr);
            }

            for (int i = 1; i <= vipNumber; i++)//redovi
            {
                for (int j = 1; j <= col; j++)//kolone
                {
                    Seat s = new Seat();
                    s.Col = j;
                    s.Row = i;
                    s.SeatTypeId = 1;
                    s.TrainId = t.Id;
                    db.seats.Add(s);
                }
            }

            for (int i = vipNumber + 1; i <= busNumber + vipNumber; i++)//redovi
            {
                for (int j = 1; j <= col; j++)//kolone
                {
                    Seat s = new Seat();
                    s.Col = j;
                    s.Row = i;
                    s.SeatTypeId = 2;
                    s.TrainId = t.Id;
                    db.seats.Add(s);
                }
            }

            for (int i = vipNumber + busNumber + 1; i <= busNumber + vipNumber + regNumber; i++)//redovi
            {
                for (int j = 1; j <= col; j++)//kolone
                {
                    Seat s = new Seat();
                    s.Col = j;
                    s.Row = i;
                    s.SeatTypeId = 3;
                    s.TrainId = t.Id;
                    db.seats.Add(s);
                }
            }
            db.SaveChanges();
            this.Close();


          
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
