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

namespace railway.CRUDTrain
{
    /// <summary>
    /// Interaction logic for DeleteTrainModal.xaml
    /// </summary>
    public partial class DeleteTrainModal : UserControl
    {
        public DeleteTrainModal()
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

        public bool ShowHandlerDialog(string name)
        {
           deleteLabel.Content = "Voz " + name + " je obrisan.";


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

    }
}
