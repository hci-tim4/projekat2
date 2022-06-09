using railway.database;
using railway.dto.trains;
using railway.model;
using railway.services;
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
    /// Interaction logic for EditTrain.xaml
    /// </summary>
    public partial class EditTrain : Window
    {
        public int id;
        public EditTrain(TrainDTO trainDTO)
        {
            InitializeComponent();
            id = trainDTO.Id;
            valuetb.Text = trainDTO.Name;
            regulartb.Text = Convert.ToString(trainDTO.numberREGULAR);
            viptb.Text = Convert.ToString(trainDTO.numberVIP);
            businesstb.Text = Convert.ToString(trainDTO.numberBUSINESS);
            // colNumber.SelectedIndex = colNumber.Items.IndexOf(trainDTO.col);
            var comboBoxItem = colorTrain.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == trainDTO.Color);
            colorTrain.SelectedIndex = colorTrain.Items.IndexOf(comboBoxItem);
            var comboBoxColItem = colNumber.Items.OfType<ComboBoxItem>().FirstOrDefault(x => int.Parse(x.Content.ToString()) == trainDTO.col);
            colNumber.SelectedIndex = colNumber.Items.IndexOf(comboBoxColItem);
            regulartb.Text = Convert.ToString(trainDTO.numberREGULAR);
            viptb.Text = Convert.ToString(trainDTO.numberVIP);
            businesstb.Text = Convert.ToString(trainDTO.numberBUSINESS);
        }
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new RailwayContext();
            var train = db.trains.Where(t => t.Id == id).FirstOrDefault();
            train.Name = valuetb.Text;
            train.Color = ((System.Windows.Controls.ComboBoxItem) colorTrain.SelectedItem).Content as string;
            string colStr = ((System.Windows.Controls.ComboBoxItem)colNumber.SelectedItem).Content as string;
            int col = int.Parse(colStr);
            var seats = from s in db.seats
                        where s.TrainId == id
                        select s;

            foreach(var s in seats)
            {
                db.seats.Remove(s);
            }

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
                    s.TrainId = id;
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
                    s.TrainId = id;
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
                    s.TrainId = id;
                    db.seats.Add(s);
                }
            }

            db.SaveChanges();
            this.Close();
            CRUDTrains.dto = TrainService.getTrains();
            Window messageBox = new CustomMessageBox("Voz " + valuetb.Text + " je izmenjen!");
            messageBox.ShowDialog();

        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
