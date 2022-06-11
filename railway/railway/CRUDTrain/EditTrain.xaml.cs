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
            var comboBoxItem = colorTrain.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == trainDTO.Color);
            colorTrain.SelectedIndex = colorTrain.Items.IndexOf(comboBoxItem);
        }
    
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new RailwayContext();
            var train = db.trains.Where(t => t.Id == id).FirstOrDefault();
            if (valuetb.Text == "")
            {
                Window message = new CustomMessageBox("Niste uneli naziv voza!");
                message.ShowDialog();
            }
            else
            {
                train.Name = valuetb.Text;

                if (colorTrain.SelectedItem == null)
                {

                    Window messageb = new CustomMessageBox("Niste izabrali boju voza!");
                    messageb.ShowDialog();
                }
                else
                {
                    train.Color = ((System.Windows.Controls.ComboBoxItem)colorTrain.SelectedItem).Content as string;
                }

                db.SaveChanges();
                this.Close();
                CRUDTrains.dto = TrainService.getTrains();
                Window messageBox = new CustomMessageBox("Voz " + valuetb.Text + " je izmenjen!");
                messageBox.ShowDialog();
            }

        }

        
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
