using System.Windows;

namespace railway
{
    public partial class ConfirmDelete : Window
    {
        public bool delete { get; set; } = false;
        
        public ConfirmDelete()
        {
            InitializeComponent();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            delete = false;
            this.Close();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            delete = true;
            this.Close();
        }
    }
}