using railway.database;
using railway.dto.trains;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace railway.CRUDTrain
{
    /// <summary>
    /// Interaction logic for EditTrainModal.xaml
    /// </summary>
    public partial class EditTrainModal : UserControl
    {
        public EditTrainModal()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }
        private bool _hideRequest = false;
        private bool _result = false;
        private UIElement _parent;
        private int id;


        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        public bool ShowHandlerDialog(TrainDTO trainDTO)
        {
            id = trainDTO.Id;
            valuetb.Text = trainDTO.Name;
            regulartb.Text = Convert.ToString(trainDTO.numberREGULAR);
            viptb.Text = Convert.ToString(trainDTO.numberVIP);
            businesstb.Text = Convert.ToString(trainDTO.numberBUSINESS);
            // colNumber.SelectedIndex = colNumber.Items.IndexOf(trainDTO.col);
            var comboBoxItem = colorTrain.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == trainDTO.Color);
            colorTrain.SelectedIndex = colorTrain.Items.IndexOf(comboBoxItem);
          

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

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new RailwayContext();
            var train = db.trains.Where(t => t.Id == id).FirstOrDefault();
            train.Name = valuetb.Text;
            train.Color = ((System.Windows.Controls.ComboBoxItem)colorTrain.SelectedItem).Content as string;
            //broj sedista!! redovi i kolone sredi to!
            db.SaveChanges();
        
        }


        private void HideHandlerDialog()
        {
            _hideRequest = true;
            Visibility = Visibility.Hidden;
            _parent.IsEnabled = true;
        }
    }
}
