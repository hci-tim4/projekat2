using railway.database;
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
    /// Interaction logic for AddTrain.xaml
    /// </summary>
    public partial class AddTrain : Window
    {
        public AddTrain()
        {
            try{
                InitializeComponent();
                
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try{
                var db = new RailwayContext();
                string name = valuetb.Text;
                if (name == "")
                {
                    Window box = new CustomMessageBox("Unesite naziv voza!");
                    box.ShowDialog();
                    
                }
                string color = "";
                if (colorTrain.SelectedItem == null)
                {
                    Window box = new CustomMessageBox("Izaberite boju voza!");
                    box.ShowDialog();
                }
                else
                {
                    color = ((System.Windows.Controls.ComboBoxItem)colorTrain.SelectedItem).Content as string;
                }

                Train t = new Train();
                t.Color = color;
                t.Name = name;
                db.trains.Add(t);
                string colStr = "";
                if (colNumber.SelectedItem == null)
                {
                    Window box = new CustomMessageBox("Broj kolona nije izabran!");
                    box.ShowDialog();
                }
                else
                {
                    colStr = ((System.Windows.Controls.ComboBoxItem)colNumber.SelectedItem).Content as string;
                }
                int col = int.Parse(colStr);

                int regNumber = 0;
                int vipNumber = 0;
                int busNumber = 0;
                string RegNumberStr = regulartb.Text;

                if (!IsNumeric(RegNumberStr))
                {
                    Window box = new CustomMessageBox("Polje broj sedišta REGULAR mora biti ceo broj!" );
                    box.ShowDialog();
                }
                else
                {
                    regNumber = int.Parse(RegNumberStr);
                }
                string VipNumberStr = viptb.Text;
                if (!IsNumeric(VipNumberStr))
                {
                    Window box = new CustomMessageBox("Polje broj sedišta VIP mora biti ceo broj");
                    box.ShowDialog();
                }
                else
                {
                    vipNumber = int.Parse(VipNumberStr);
                }
                string BusNumberStr = businesstb.Text;
                if (!IsNumeric(BusNumberStr))
                {
                    Window box = new CustomMessageBox("Polje broj sedišta BUSINESS mora biti ceo broj");
                    box.ShowDialog();
                }
                else
                {
                    busNumber = int.Parse(BusNumberStr);
                }
               /////////////////// ///PROVERE DA LI SU PODACI DOBRI//////////////////////////
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
                CRUDTrains.dto = TrainService.getTrains();
                Window messageBox = new CustomMessageBox("Novi voz " + valuetb.Text + " je uspešno dodat!");
                messageBox.ShowDialog();
         
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
          
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[2]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            addBtn_Click(sender, e);
        }
    }
}
