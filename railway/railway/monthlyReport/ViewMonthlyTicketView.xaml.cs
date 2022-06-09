using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using LiveCharts;
using LiveCharts.Wpf;
using railway.client;
using railway.database;
using railway.model;
using railway.services;

namespace railway.monthlyReport
{
    public partial class ViewMonthlyTicketView : UserControl, TutorialInterface
    {
        public barChartInformation barChartData { get; set; }//= new barChartInformation();
        private List<InformationForGraphDisplay> currentData = null;
        private List<TicketForReportDTO> tickets = null;
        private TicketService ticketService;
        public Func<double, string> Formatter { get; set; }

        public ViewMonthlyTicketView()
        {
            InitializeComponent();
            ticketService = new TicketService();
            barChartData = new barChartInformation();
            Formatter = value => value.ToString("N");
            chartStackPanel.DataContext = this;
            dataGrid.DataContext = this;
            fromDateDatePicker.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
            untilDateDatePicker.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
        }
        
        
        private void PrepareGraph()
        {
            barChartData.reset();
            dataGrid.ItemsSource = currentData;

            ChartValues<int> numberOfSelledTickets = new ChartValues<int>();

            foreach (InformationForGraphDisplay inf in currentData)
            {
                numberOfSelledTickets.Add(inf.NumberOfSelledTickets);
                barChartData.xAxisLabels.Add(inf.Type);

            }

            barChartData.BarLineSeriesCollection.Add(new ColumnSeries()
            {
                Title = "Bar chart",
                Values = numberOfSelledTickets,
                PointGeometry = DefaultGeometries.Diamond,
            });


            //DataContext = this;

            //var lineChartObject = (CartesianChart)this.FindName("barChart");
            //lineChartObject.HideLegend();
            Formatter = value => value.ToString("N");


        }

        private void ShowReport_OnClick(object sender, RoutedEventArgs e)
        {
            bool ?seatType = seatTypeRadioButton.IsChecked;
            bool ?driving = drivingLineRadioButton.IsChecked;
            DateTime? fromDate = fromDateDatePicker.SelectedDate;
            DateTime? untilDate = untilDateDatePicker.SelectedDate;
            if (fromDate == null && untilDate == null)
            {
                MessageBox.Show("Niste izabrali početni datum i krajnji datum.");
                return;
            }else if (fromDate == null)
            {
                MessageBox.Show("Niste izabrali početni datum.");
                return;
            }else if (untilDate == null)
            {
                MessageBox.Show("Niste izabrali krajnji datum.");
                return;
            }else if (fromDate > untilDate)
            {
                MessageBox.Show("Početni datum je veći od krajnjeg.");
                return;
            }
            tickets = ticketService.getTicketsInPeriod(fromDate, untilDate);
            if ((bool)seatType)
                FillInformationForGraphBySeatType();
            else if ((bool)driving)
                FillInformationForGraphByDrivingLine();
            else
            {
                seatTypeRadioButton.IsChecked = true;
                FillInformationForGraphBySeatType();
                return;
            }
            PrepareGraph();
        }

        private void FillInformationForGraphBySeatType()
        {
            currentData = new List<InformationForGraphDisplay>();
            InformationForGraphDisplay vip = new InformationForGraphDisplay()
            {
                Type = "VIP",
                NumberOfSelledTickets = GetNumberOfVIPCards(),
                Price = CountPriceForVIPCards()
            };
            currentData.Add(vip);
            InformationForGraphDisplay biz = new InformationForGraphDisplay()
            {
                Type = "biznis",
                NumberOfSelledTickets = GetNumberOfBiznisCards(),
                Price = CountPriceForBiznisCards()
            };
            currentData.Add(biz);
            InformationForGraphDisplay reg = new InformationForGraphDisplay()
            {
                Type = "Regularan",
                NumberOfSelledTickets = GetNumberOfRegularCards(),
                Price = CountPriceForRegularCards()
            };
            currentData.Add(reg);
        }

        private double CountPriceForRegularCards()
        {
            double res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                res += t.PriceOfRegularSeats;
            }

            return res;
        }

        private double CountPriceForBiznisCards()
        {
            double res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                res += t.PriceOfBiznisSeats;
            }

            return res;
        }

        private double CountPriceForVIPCards()
        {
            double res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                res += t.PriceOfVIPSeats;
            }

            return res;
        }

        private int GetNumberOfVIPCards()
        {
            int res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                res += t.NumberOfVIPSeats;
            }

            return res;
        }
        
        private int GetNumberOfBiznisCards()
        {
            int res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                res += t.NumberOfBiznisSeats;
            }

            return res;
        }
        
        private int GetNumberOfRegularCards()
        {
            int res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                res += t.NumberOfRegularSeats;
            }

            return res;
        }

        private void FillInformationForGraphByDrivingLine()
        {
            currentData = new List<InformationForGraphDisplay>();
            using (var db = new RailwayContext())
            {
                foreach (DrivingLine dl in db.drivingLines)
                {
                    InformationForGraphDisplay inf = new InformationForGraphDisplay()
                    {
                        Type = dl.Name
                    };
                    inf.NumberOfSelledTickets = CountTicketsForDrivingLine(dl.Id);
                    inf.Price = CountPriceForDrivingLine(dl.Id);
                    currentData.Add(inf);
                }
            }
        }

        private double CountPriceForDrivingLine(int drivingLineId)
        {
            double res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                if (t.DrivingLineId == drivingLineId)
                {
                    res += t.Price;
                }
            }

            return res;
        }

        private int CountTicketsForDrivingLine(int drivingLineId)
        {
            int res = 0;
            foreach (TicketForReportDTO t in tickets)
            {
                if (t.DrivingLineId == drivingLineId)
                {
                    res += t.NumberOfBiznisSeats + t.NumberOfRegularSeats + t.NumberOfVIPSeats;
                }
            }

            return res;
        }

        private void showBySeatTypes(object sender, RoutedEventArgs e)
        {
            if (tickets == null)
            {
                MessageBox.Show("Prvo morate da izaberete interval.");
                return;
            }
            FillInformationForGraphBySeatType();
            PrepareGraph();
        }

        private void showByDrivingLines(object sender, RoutedEventArgs e)
        {
            if (tickets == null)
            {
                MessageBox.Show("Prvo morate da izaberete interval.");
                return;
            }
            FillInformationForGraphByDrivingLine();
            PrepareGraph();
        }

        public void StartTour_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tutorijal");
        }
    }
}