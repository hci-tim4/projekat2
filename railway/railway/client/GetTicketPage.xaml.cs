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
using System.Windows.Navigation;
using System.Windows.Shapes;
using railway.model;
using railway.database;
using System.Linq;

namespace railway.client
{
    /// <summary>
    /// Interaction logic for GetTicketPage.xaml
    /// </summary>
    public partial class GetTicketPage : Page
    {
        private SeatDisplay seatsPage;
        private List<TicketSeats> ticketSeats;//SACUVAAAAAAAAAAAAAAJ
        private GetTicketDTO getTicketDTO;
        private User user;
        public GetTicketPage(GetTicketDTO dto, User u)
        {
            InitializeComponent();
            getTicketDTO = dto;
            user = u;
            seatsPage = new SeatDisplay(dto.DrivingLineId, dto.ScheduleId);;
            seatDisplay.Content = seatsPage;
            displayInfo.Content = new ChosenSchedulePage(dto.FromStationScheduleId, dto.UntilStationScheduleId, dto.ScheduleId);
        }

        private void reserveBtn_Click(object sender, RoutedEventArgs e)
        {
            Ticket t = makeTicket();
            if (t == null)
                return;
            t.TicketType = TicketType.Reserved;
            Window confirmation = new TicketConfirmationWindow(t, "rezervišete", seatsPage.checkedSeatIds);
            confirmation.Show();
        }

        private void buyBtn_Click(object sender, RoutedEventArgs e)
        {
            Ticket t = makeTicket();
            if (t == null)
                return;
            t.TicketType = TicketType.Bought;
            Window confirmation = new TicketConfirmationWindow(t, "kupite", seatsPage.checkedSeatIds);
            confirmation.Show();
        }

        private Ticket makeTicket()
        {
            Ticket t = new Ticket()
            {
                User = user,
                FromStationScheduleId = getTicketDTO.FromStationScheduleId,
                UntilStationScheduleId = getTicketDTO.UntilStationScheduleId,
                ScheduleId = getTicketDTO.ScheduleId
            };
            double price = countPrice();
            if (price == 0)
                return null;
            t.Price = price;
            return t;
        }

        private double countPrice()
        {
            List<int> chosenSeatsId = seatsPage.checkedSeatIds;
            double price = 0;
            using (var db = new RailwayContext())
            {
                StationSchedule ss1 = (from stationSchedules in db.stationsSchedules
                                       where stationSchedules.Id == getTicketDTO.FromStationScheduleId
                                       select stationSchedules).Single();

                StationSchedule ss2 = (from stationSchedules in db.stationsSchedules
                                       where stationSchedules.Id == getTicketDTO.UntilStationScheduleId
                                       select stationSchedules).Single();

                int numberOfStationsBetween = ss2.SerialNumber - ss1.SerialNumber;

                ticketSeats = new List<TicketSeats>();

                if (chosenSeatsId.Count == 0)
                {
                    MessageBox.Show("Niste izabrali sedište! Pre nastavka morate da izaberete sedište");
                    return 0;
                }

                foreach (int seatId in chosenSeatsId)
                {
                    Seat seat = (from seats in db.seats
                                   where seats.Id == seatId
                                   select seats).Single();
                    price = price + numberOfStationsBetween * seat.SeatType.Price;

                }


            }
            return price;
        }
    }
}
