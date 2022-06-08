using railway.database;
using railway.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

namespace railway.CRUDTrain
{
    /// <summary>
    /// Interaction logic for AddTrainModal.xaml
    /// </summary>
    public partial class AddTrainModal : UserControl
    {
        public AddTrainModal()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        private bool _hideRequest = false;
        private bool _result = false;
        private UIElement _parent;
      

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        public bool ShowHandlerDialog()
        {

            
            Visibility = Visibility.Visible;

            _parent.IsEnabled = false;

            _hideRequest = false;
            while (!_hideRequest)
            {
                // HACK: Stop the thread if the application is about to close
                if (this.Dispatcher.HasShutdownStarted ||
                    this.Dispatcher.HasShutdownFinished)
                {
                    break;
                }

                // HACK: Simulate "DoEvents"
                this.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Background,
                    new ThreadStart(delegate { }));
                Thread.Sleep(20);
            }

            return _result;
        }

        
          

        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }



        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            HideHandlerDialog();
        }

        private void HideHandlerDialog()
        {
            _hideRequest = true;
            Visibility = Visibility.Hidden;
            _parent.IsEnabled = true;
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
           
            for (int i = 0; i < regNumber; i++)//redovi
            {
                for(int j = 0; j < col; j++)//kolone
                {
                    Seat s = new Seat();
                    s.Col = j++;
                    s.Row = i++;
                    s.SeatTypeId = 3;
                    s.TrainId = t.Id;
                    db.seats.Add(s);
                }
            }

            for (int i = 0; i < vipNumber; i++)//redovi
            {
                for (int j = 0; j < col; j++)//kolone
                {
                    Seat s = new Seat();
                    s.Col = j++;
                    s.Row = i++;
                    s.SeatTypeId = 1;
                    s.TrainId = t.Id;
                    db.seats.Add(s);
                }
            }

            for (int i = 0; i < busNumber; i++)//redovi
            {
                for (int j = 0; j < col; j++)//kolone
                {
                    Seat s = new Seat();
                    s.Col = j++;
                    s.Row = i++;
                    s.SeatTypeId = 2;
                    s.TrainId = t.Id;
                    db.seats.Add(s);
                }
            }
            db.SaveChanges();
            

            HideHandlerDialog();
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
