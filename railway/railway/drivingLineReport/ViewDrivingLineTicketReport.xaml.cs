using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using railway.database;
using railway.model;
using railway.monthlyReport;
using railway.services;
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;

namespace railway.drivingLineReport
{
    public partial class ViewDrivingLineTicketReport : UserControl, INotifyPropertyChanged, TutorialInterface
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private List<drivingLineForReportDTO> drivingLines;
        private drivingLineForReportDTO currentDrivingLine;
        private List<TicketsForDrivingLineReportDTO> tickets;
        private Boolean Touring = false;
        private TicketService ticketService;
        public List<InformationForLineChartDisplay> currentData { get; set; }

        private LineChartInformation _lineChartData;// { get; set; }
        public LineChartInformation lineChartData
        {
            get
            {
                return _lineChartData;
            }
            set
            {
                if (value != _lineChartData)
                {
                    _lineChartData = value;
                    OnPropertyChanged("lineChartData");
                }
            }
        }
        public Func<double, string> Formatter { get; set; }
        
        private double _profit;// { get; set; }
        public double Profit
        {
            get
            {
                return _profit;
            }
            set
            {
                if (value != _profit)
                {
                    _profit = value;
                    OnPropertyChanged("Profit");
                }
            }
        }
        
        public ViewDrivingLineTicketReport()
        {
            try{
                InitializeComponent();
                setDrivingLinesInComboBox();
                ticketService = new TicketService();
                lineChartData = new LineChartInformation();
                Formatter = value => value.ToString("N");
                lineChartPanel.DataContext = this;
                dataGrid.DataContext = this;
                
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void setDrivingLinesInComboBox()
        {
            try{
                using (var db = new RailwayContext())
                {
                    drivingLines = (from dl in db.drivingLines
                        select new drivingLineForReportDTO()
                        {
                            DrivingLineId = dl.Id,
                            Name = dl.Name
                        }).ToList();
                    cmbDrivingLine.ItemsSource = drivingLines;
                }
                
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var cmbx = sender as ComboBox;
                cmbx.ItemsSource = from item in drivingLines
                    where item.Name.ToLower().Contains(cmbx.Text.ToLower())
                    select item;
                this.currentDrivingLine = (from item in drivingLines
                    where item.Name.ToLower().Equals(cmbx.Text.ToLower())
                    select item).FirstOrDefault();

                if (Touring)
                {
                    if (currentDrivingLine != null)
                    {
                        IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                        navigator.IfCurrentStepEquals("ChoseDrivingLine").GoNext();
                    }
                }

                cmbx.IsDropDownOpen = true;
            
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void ShowDrivingLineReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            try{
                if (currentDrivingLine == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Prvo morate da izaberete mrežnu liniju.");
                    cmb.ShowDialog();
                    return;
                }
                if (Touring)
                {
                    IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                    navigator.IfCurrentStepEquals("ShowReport").GoNext();
                }
                tickets = ticketService.GetTicketByDrivingLine(currentDrivingLine.DrivingLineId);
                if (tickets.Count == 0)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Nema prodatih karata za izabranu mrežnu liniju");
                    cmb.ShowDialog();
                    return;
                }

                ConvertTicketsToGraphInformation();
                PrepareGraph();
                
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void ConvertTicketsToGraphInformation()
        {
            tickets = tickets.OrderBy(x => x.DateOfDepature).ToList();
            DateTime fromDate = tickets[0].DateOfDepature;
            int lastIndex = tickets.Count - 1;
            DateTime untilDate = tickets[lastIndex].DateOfDepature;
            currentData = new List<InformationForLineChartDisplay>();
            Profit = 0;
            while (fromDate.Month <= untilDate.Month && fromDate.Year <= untilDate.Year)
            {
                countInfoForMonthPeriod(fromDate);
                fromDate = fromDate.AddMonths(1);
            }
            dataGrid.ItemsSource = currentData;
        }

        private void countInfoForMonthPeriod(DateTime fromDate)
        {
            InformationForLineChartDisplay inf = new InformationForLineChartDisplay()
            {
                Type = DateTimeToMonthAndYearString.Convert(fromDate),
                NumberOfSelledTickets = 0,
                Price = 0
            };

            List<TicketsForDrivingLineReportDTO> ticketsInCurrentPeriod = (from t in tickets
                where t.DateOfDepature.Month == fromDate.Month && t.DateOfDepature.Year == fromDate.Year
                select t).ToList();

            foreach (TicketsForDrivingLineReportDTO ticketForReport in ticketsInCurrentPeriod)
            {
                inf.NumberOfSelledTickets += ticketForReport.numberOfTickets;
                inf.Price += ticketForReport.Price;
                this.Profit += inf.Price;
            }
            
            currentData.Add(inf);

        }

        private void PrepareGraph()
        {

            lineChartData.reset();
            lineChartPanel.DataContext = this;

            ChartValues<int> values = new ChartValues<int>();

            foreach (InformationForLineChartDisplay inf in currentData)
            {
                values.Add(inf.NumberOfSelledTickets);
                lineChartData.xAxisLabels.Add(inf.Type);


            }

            lineChartData.LineSeriesCollection.Add(new LineSeries()
            {
                Title = "Line chart",
                Values = values,
                PointGeometry = DefaultGeometries.Diamond,
                PointGeometrySize = 8,
            });

            var lineChartObject = (CartesianChart)this.FindName("lineChart");
            lineChartObject.HideLegend();
            Formatter = value => value.ToString("N");

        }

        public void StartTour_OnClick(object sender, RoutedEventArgs e)
        {
            Touring = true;
            TextLocalization.Close = "Zatvori";
            TextLocalization.Next = "Sledeći";
            var tour = new Tour()
            {
                Name = "My Demo Tour",
                ShowNextButtonDefault = true,
                EnableNextButtonAlways = true,
                
                Steps = new []
                {
                    //wait for dates
                    new Step("ChoseDrivingLine", "Mrežna linija", "Izaberite jedanu od ponuđenih mrežnih linija.")
                    {
                        ShowNextButton = false
                    },
                    new Step("ShowReport", "Prikaži", "Kliknite na dugme 'Prikaži' da biste videli izveštaj za izabrani period.")
                    {
                        ShowNextButton = false
                    },
                    //wait for click
                    new Step("ReportChart", "Grafikon", "Grafički prikaz za izabranu mrežnu liniju."),
                    new Step("WholeProfit", "Ukupan profit", "Ukupan profit za izabranu mrežnu liniju."),
                    new Step("ReportTable", "Tabela", "Tabelarni prikaz izveštaja za izabranu mrežnu liniju."),
                    
                },
                
            };

            tour.Start();

        }
    }
}