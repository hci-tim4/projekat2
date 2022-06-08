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

namespace railway.drivingLineReport
{
    public partial class ViewDrivingLineTicketReport : UserControl, INotifyPropertyChanged
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
        private TicketService ticketService;
        public List<InformationForLineChartDisplay> currentData { get; set; }
        
        /*
        public List<InformationForLineChartDisplay> currentData
        {
            get
            {
                return _currentData;
            }
            set
            {
                if (value != _currentData)
                {
                    _currentData = value;
                    OnPropertyChanged("currentData");
                }
            }
        }*/

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
            InitializeComponent();
            setDrivingLinesInComboBox();
            ticketService = new TicketService();
            lineChartData = new LineChartInformation();
            Formatter = value => value.ToString("N");
            lineChartPanel.DataContext = this;
            dataGrid.DataContext = this;
        }

        private void setDrivingLinesInComboBox()
        {
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cmbx = sender as ComboBox;
            cmbx.ItemsSource = from item in drivingLines
                where item.Name.ToLower().Contains(cmbx.Text.ToLower())
                select item;
            this.currentDrivingLine = (from item in drivingLines
                where item.Name.ToLower().Equals(cmbx.Text.ToLower())
                select item).FirstOrDefault();

            cmbx.IsDropDownOpen = true;
            
        }

        private void ShowDrivingLineReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (cmbDrivingLine == null)
            {
                MessageBox.Show("Prvo morate da izaberete mrežnu liniju.");
                return;
            }

            tickets = ticketService.GetTicketByDrivingLine(currentDrivingLine.DrivingLineId);
            if (tickets.Count == 0)
            {
                MessageBox.Show("Nema prodatih karata za izabranu mrežnu liniju");
                return;
            }

            ConvertTicketsToGraphInformation();
            PrepareGraph();
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

    }
}