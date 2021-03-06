using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;

namespace railway.monthlyReport
{
    public partial class ViewMonthlyTicketView : UserControl, TutorialInterface, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public barChartInformation barChartData { get; set; }//= new barChartInformation();
        private List<InformationForGraphDisplay> currentData = null;
        private List<TicketForReportDTO> tickets = null;
        private TicketService ticketService;
        private bool Touring = false;
        public Func<double, string> Formatter { get; set; }
        private double _profit;
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

        public ViewMonthlyTicketView()
        {
            try{
                InitializeComponent();
                ticketService = new TicketService();
                barChartData = new barChartInformation();
                Formatter = value => value.ToString("N");
                chartStackPanel.DataContext = this;
                dataGrid.DataContext = this;
                this.DataContext = this;
                fromDateDatePicker.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
                untilDateDatePicker.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
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
            try{
                bool ?seatType = seatTypeRadioButton.IsChecked;
                bool ?driving = drivingLineRadioButton.IsChecked;
                DateTime? fromDate = fromDateDatePicker.SelectedDate;
                DateTime? untilDate = untilDateDatePicker.SelectedDate;
                if (fromDate == null && untilDate == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Niste izabrali početni datum i krajnji datum.");
                    cmb.ShowDialog();
                    return;
                }else if (fromDate == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Niste izabrali početni datum.");
                    cmb.ShowDialog();
                    return;
                }else if (untilDate == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Niste izabrali krajnji datum.");
                    cmb.ShowDialog();
                    return;
                }else if (fromDate > untilDate)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Početni datum je veći od krajnjeg.");
                    cmb.ShowDialog();
                    return;
                }
                IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals("ShowReportButton").GoNext();
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
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }    

        }

        private void FillInformationForGraphBySeatType()
        {
            Profit = 0;
            currentData = new List<InformationForGraphDisplay>();
            InformationForGraphDisplay vip = new InformationForGraphDisplay()
            {
                Type = "VIP",
                NumberOfSelledTickets = GetNumberOfVIPCards(),
                Price = CountPriceForVIPCards()
            };
            Profit += vip.Price;
            currentData.Add(vip);
            InformationForGraphDisplay biz = new InformationForGraphDisplay()
            {
                Type = "Biznis",
                NumberOfSelledTickets = GetNumberOfBiznisCards(),
                Price = CountPriceForBiznisCards()
            };
            currentData.Add(biz);
            Profit += biz.Price;
            InformationForGraphDisplay reg = new InformationForGraphDisplay()
            {
                Type = "Redovna",
                NumberOfSelledTickets = GetNumberOfRegularCards(),
                Price = CountPriceForRegularCards()
            };
            Profit += reg.Price;
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
            Profit = 0;
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
                    Profit += inf.Price;
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
            try{
                if (tickets == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Prvo morate da izaberete interval.");
                    cmb.ShowDialog();
                    return;
                }
                
                if (Touring)
                {
                    IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                    navigator.IfCurrentStepEquals("ChangeTypeOfMontlhyReport").Close();
                }
                FillInformationForGraphBySeatType();
                PrepareGraph();
            
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void showByDrivingLines(object sender, RoutedEventArgs e)
        {
            try{
                if (tickets == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Prvo morate da izaberete interval.");
                    cmb.ShowDialog();
                    return;
                }

                if (Touring)
                {
                    IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                    navigator.IfCurrentStepEquals("ChangeTypeOfMontlhyReport").Close();
                }
                FillInformationForGraphByDrivingLine();
                PrepareGraph();
                
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
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
                    new Step("StartDatePicker", "Početni datum", "Izaberite početni datum za podatke koji će se prikazati u izveštaju.")
                    {
                        ShowNextButton = false
                    },
                    new Step("EndDatePicker", "Krajnji datum", "Izaberite krajnji datum za podatke koji će se prikazati u izveštaju.")
                    {
                        ShowNextButton = false
                    },
                    //wait for click
                    new Step("ShowReportButton", "Prikaz izveštaja", "Kliknite na dugme 'Prikaži' da biste videli izveštaj za izabrani period.")
                    {
                        ShowNextButton = false
                    },
                    new Step("ReportChart", "Grafikon", "Grafički prikaz za izabrani period."),
                    new Step("WholeProfit", "Ukupan profit", "Ukupan profit za izabrani period."),
                    new Step("ReportTable", "Tabela", "Tabelarni prikaz izveštaja za izabrani period."),
                    new Step("ChangeTypeOfMontlhyReport", "Promena tipa izveštaja", "Za promenu tipa izveštaja izaberite jednu od ponuđenih opcija " +
                        "'Tipu sedišta' ili 'Mrežnim linijama'.")
                    {
                        ShowNextButton = false
                    },

                },
                
            };

            tour.Start();


        }

        private void UntilDateDatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Touring)
            {
                DateTime? untilDate = untilDateDatePicker.SelectedDate;
                if (untilDate != null)
                {
                    IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                    navigator.IfCurrentStepEquals("EndDatePicker").GoNext();
                }
            }
        }

        private void FromDateDatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Touring)
            {
                DateTime? fromDate = fromDateDatePicker.SelectedDate;
                if (fromDate != null)
                {
                    IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                    navigator.IfCurrentStepEquals("StartDatePicker").GoNext();
                }
            }
        }
    }
}