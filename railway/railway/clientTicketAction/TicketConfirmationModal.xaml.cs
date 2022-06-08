using railway.client;
using railway.database;
using railway.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace railway.clientTicketAction
{
    /// <summary>
    /// Interaction logic for TicketConfirmationModal.xaml
    /// </summary>
    public partial class TicketConfirmationModal : UserControl
    {
        public TicketConfirmationModal()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        private bool _hideRequest = false;
        private bool _result = false;
        private UIElement _parent;
        private Ticket ticket;
        private List<int> checkedSeatIds;
        public Boolean ticketsGotSaved { get; set; }
        private TicketGotSavedHandler ticketGotSavedHandler;

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }

        public bool ShowHandlerDialog(Ticket t, string action, List<int> checkedSeatIds, TicketGotSavedHandler ticketGotSaved)
        {

            this.ticketGotSavedHandler = ticketGotSaved;
            ticketsGotSaved = false;
            this.checkedSeatIds = checkedSeatIds;
            ticket = t;
            askingForConfirmLabel.Content = "Potvrda " + action;
            confirmationDataFrame.Content = new ConfirmationDataDisplay(t, checkedSeatIds.Count);
            Visibility = Visibility.Visible;

            _parent.IsEnabled = false;

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

            using (var db = new RailwayContext())
            {
                //save ticket
                //save ticketseats
                db.tickets.Add(ticket);
                db.SaveChanges(); //puca ovdee!!
                foreach (int seatId in checkedSeatIds)
                {
                    Seat seat = (from seats in db.seats
                                 where seats.Id == seatId
                                 select seats).Single();
                    TicketSeats ts = new TicketSeats()
                    {
                        SeatId = seatId,
                        TicketId = ticket.Id
                    };
                    db.ticketSeats.Add(ts);
                }
                db.SaveChanges();
            }
            ticketGotSavedHandler();
            MessageBox.Show("Uspešno su sačuvani podaci.", "Uspeh");
            HideHandlerDialog();
        }

        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

      

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
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
            _result = false;
            HideHandlerDialog();
        }




}
}
