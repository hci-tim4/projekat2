﻿using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using railway.client;
using railway.database;
using railway.defineDrivingLine;
using railway.model;

namespace railway.defineDrivingLine
{
    public partial class DefineEndDateForDrivingLineModal : UserControl
    {
        

        private bool _hideRequest = false;
        private bool _result = false;
        private UIElement _parent;
        
        
        public DrivingLineViewDTO drivingLine { get; set; }
        public DrivingLines DrivingLinesView { get; set; }
        private DefineEndDateForDrivingLine defineEndDate;
        
        public DefineEndDateForDrivingLineModal()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }
        

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        public bool ShowHandlerDialog(DrivingLineViewDTO dto, DrivingLines drivingLinesView)
        {
            drivingLine = dto;
            this.DrivingLinesView = drivingLinesView;
            drivingLineStackPanel.DataContext = drivingLine;
            //askingForConfirmLabel.Content = "Krajnji datum mrežne linije";
            //this.defineEndDate = new DefineEndDateForDrivingLine(dto, drivingLinesView);
            //confirmationDataFrame.Content = defineEndDate;
            Visibility = Visibility.Visible;

            _parent.IsEnabled = false;
            
            IsEnabled = true;
            Focus();
            

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

        private void confirmBtn_Click_1(object sender, RoutedEventArgs e)
        {
            //defineEndDate.saveDate();
            _result = true;
            
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
                    dl.endDate = drivingLine.newEndDate;
                    MessageBox.Show(
                        "Uspešno ste sačuvali krajnji datum.");
                    
                    db.SaveChanges();
                    DrivingLinesView.setDrivingLines(new RailwayContext());
                    // this.Close();
                }
            }
            //MessageBox.Show("Chosen date: " + drivingLine.newEndDate);
            //MessageBox.Show("Uspešno su sačuvani podaci.", "Uspeh");
            HideHandlerDialog();
        }

        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

      

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            //_result = false;
            HideHandlerDialog();
        }

        private void HideHandlerDialog()
        {
            _hideRequest = true;
            Visibility = Visibility.Hidden;
            _parent.IsEnabled = true;
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //_result = false;
            HideHandlerDialog();
        }

        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            confirmBtn_Click_1(sender, e);
        }
    }
}