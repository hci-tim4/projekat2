using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using railway.database;
using railway.defineDrivingLine;
using railway.model;
using System.Linq;

namespace railway.CRUDDrivingLine
{
    public partial class DefineEndDateForDrivingLine : Window
    {
        private DrivingLineViewDTO drivingLine;
        private ViewDrivingLines viewDrivingLines;
        public DefineEndDateForDrivingLine(DrivingLineViewDTO dto, ViewDrivingLines viewDrivingLines)
        {
            InitializeComponent();
            this.DataContext = dto;
            drivingLine = dto;
            this.viewDrivingLines = viewDrivingLines;
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
                    viewDrivingLines.setDrivingLines(new RailwayContext());
                    this.Close();
                }
            }
        }
    }
}