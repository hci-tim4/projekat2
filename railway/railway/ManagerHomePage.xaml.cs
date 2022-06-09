using railway.model;
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
using railway.managerSchedule;
using railway.defineDrivingLine;
using System.Windows.Navigation;
using System.Windows.Shapes;
using railway.drivingLineReport;
using railway.monthlyReport;
using railway.CRUDTrain;

namespace railway
{
    /// <summary>
    /// Interaction logic for ManagerHomePage.xaml
    /// </summary>
    public partial class ManagerHomePage : Window
    {

        string last = "";
        User loggedUser;
        Page managerSchedule;
        UserControl viewDrivinglines;
        UserControl monthlyReport;
        private UserControl drivingLineReport;
        private TutorialInterface CurrentContent;
        
        public ManagerHomePage(User user)
        {
            InitializeComponent();
            this.loggedUser = user;
            //this.managerSchedule = new ManagerSchedule();
            //this.viewDrivinglines = new ViewDrivingLines(page2);
            //this.monthlyReport = new ViewMonthlyTicketView();
           // this.drivingLineReport = new ViewDrivingLineTicketReport();
        }

       


        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    //GridPrincipal.Children.Clear();
                    //GridPrincipal.Children.Add(new Timetable(loggedUser));
                    page.Content = "";
                    CurrentContent = new ManagerSchedule();
                    page.Content = CurrentContent;
                    //page.Content = 

                    break;
                case 1:
                    page.Content = "";
                    CurrentContent = new ViewDrivingLines(page);
                    page.Content = CurrentContent;
                    break;
                case 2:
                    CurrentContent = new CRUDTrains();
                    page.Content = CurrentContent;
                    break;
                case 3:
                    CurrentContent = new ViewMonthlyTicketView();
                    page.Content = CurrentContent;
                    break;
                case 4:
                    CurrentContent = new ViewDrivingLineTicketReport();
                    page.Content = CurrentContent;
                    break;

                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (50 * index)), 0, 0);
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ScheduleLines_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ScheduleLines_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            page.Content = "";
            CurrentContent = new ManagerSchedule();
            page.Content = CurrentContent;
        }

        private void DrivingLines_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DrivingLines_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            page.Content = "";
            CurrentContent = new ViewDrivingLines(page);
            page.Content = CurrentContent;
        }

        private void Trains_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Trains_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentContent = new CRUDTrains();
            page.Content = CurrentContent;
        }

        private void Report_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Report_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentContent = new ViewMonthlyTicketView();
            page.Content = CurrentContent;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        public void doThings(string param)
        {
            //     btnOK.Background = new SolidColorBrush(Color.FromRgb(32, 64, 128));
            Title = param;
        }

        private void tutorial_clicked(object sender, RoutedEventArgs e)
        {
            CurrentContent.StartTour_OnClick(sender, e);
        }

        private void StartTutorial_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void StartTutorial_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentContent.StartTour_OnClick(sender, e);
        }

        private void help_clicked(object sender, RoutedEventArgs e)
        {
            HelpProvider.ShowHelp("ManagerAppHelp", this);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Window login = new MainWindow();
            //App.Current.MainWindow.Close();
            this.Close();
            login.Show();
        }
    }


}
