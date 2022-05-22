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
using railway.model;
using railway.database;
using System.Linq;

namespace railway.client
{
    /// <summary>
    /// Interaction logic for TicketConfirmationWindow.xaml
    /// </summary>
    public partial class TicketConfirmationWindow : Window
    {
        private Ticket ticket;
        private List<int> checkedSeatIds;
        public TicketConfirmationWindow(Ticket t, string action, List<int> checkedSeatIds)
        {
            InitializeComponent();
            this.checkedSeatIds = checkedSeatIds;
            ticket = t;
            askingForConfirmLabel.Content = "Potvrdite da hoćete da " + action + "izabranu kartu/izabrane karte.";
            confirmationDataFrame.Content = new ConfirmationDataDisplay(t, checkedSeatIds.Count);
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            
            using (var db = new RailwayContext())
            {
                //save ticket
                //save ticketseats
                db.tickets.Add(ticket);
                db.SaveChanges();
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
            MessageBox.Show("Uspešno su sačuvani podaci.", "Uspeh");
        }

        private void confirmBtn_Click_1(object sender, RoutedEventArgs e)
        {

            using (var db = new RailwayContext())
            {
                //save ticket
                //save ticketseats
                db.tickets.Add(ticket);
                db.SaveChanges();
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
            MessageBox.Show("Uspešno su sačuvani podaci.", "Uspeh");
            this.Close();

        }
    }
}
