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
using System.Windows.Shapes;

namespace railway
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            InitializeComponent();
            messageLabel.Content = message;
        }

        public void ok_clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseMeesageBox_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseMeesageBox_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
