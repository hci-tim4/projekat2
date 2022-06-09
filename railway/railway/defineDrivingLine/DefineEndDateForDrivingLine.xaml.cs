using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using railway.database;
using railway.defineDrivingLine;
using railway.model;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace railway.defineDrivingLine
{
    public partial class DefineEndDateForDrivingLine : Window
    {
        public DrivingLineViewDTO drivingLine { get; set; }
        private DrivingLines DrivingLinesView;
        public DefineEndDateForDrivingLine(DrivingLineViewDTO dto, DrivingLines drivingLinesView)
        {
            InitializeComponent();
            drivingLine = dto;
            endDate.SelectedDate = drivingLine.newEndDate;
            endDateStackPanel.DataContext = drivingLine;
            this.DrivingLinesView = drivingLinesView;
            endDate.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
        }

        public void saveDate()
        {
            
            DateTime? selectedEndDate = endDate.SelectedDate;
            if (selectedEndDate == null)
            {
                MessageBox.Show("Morate da izaberete datum.");
                return;
            }

            using (var db = new RailwayContext())
            {
                List<Ticket> tickets = (from t in db.tickets
                    join s in db.schedules
                        on t.ScheduleId equals s.Id
                    where s.DepatureDate > DateTime.Now && s.DrivingLineId == drivingLine.DrivingLineId
                    select t).ToList();
                if (tickets.Count > 0)
                    MessageBox.Show("Postoje prodate karte za liniju.");
                else
                {
                    DrivingLine dl = (from d in db.drivingLines
                        where d.Id == drivingLine.DrivingLineId
                        select d).Single();
                    dl.endDate = selectedEndDate;
                    MessageBox.Show(
                        "Uspešno ste sačuvali krajnji datum.");
                    
                    db.SaveChanges();
                    DrivingLinesView.setDrivingLines(new RailwayContext());
                    // this.Close();
                }
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime? selectedEndDate = endDate.SelectedDate;
            if (selectedEndDate == null)
            {
                MessageBox.Show("Morate da izaberete datum.");
                return;
            }

            using (var db = new RailwayContext())
            {
                List<Ticket> tickets = (from t in db.tickets
                    join s in db.schedules
                        on t.ScheduleId equals s.Id
                    where s.DepatureDate > DateTime.Now && s.DrivingLineId == drivingLine.DrivingLineId
                    select t).ToList();
                if (tickets.Count > 0)
                    MessageBox.Show("Postoje prodate karte za liniju.");
                else
                {
                    DrivingLine dl = (from d in db.drivingLines
                        where d.Id == drivingLine.DrivingLineId
                        select d).Single();
                    dl.endDate = selectedEndDate;
                    MessageBox.Show(
                        "Uspešno ste sačuvali krajnji datum.");
                    
                    db.SaveChanges();
                    DrivingLinesView.setDrivingLines(new RailwayContext());
                   this.Close();
                }
            }
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

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        
        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Save_OnClick(sender, e);
        }
    }
}