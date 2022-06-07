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
using railway.clientTimetable;
using railway.clientTicketAction;

namespace railway.client
{
    /// <summary>
    /// Interaction logic for GetTicketPage.xaml
    /// </summary>
    public delegate void TicketGotSavedHandler();
    public partial class GetTicketPage : UserControl
    {
        public event TicketGotSavedHandler ticketGotSaved;
        private SeatDisplay seatsPage;
        private List<TicketSeats> ticketSeats;//SACUVAAAAAAAAAAAAAAJ
        private GetTicketDTO getTicketDTO;
        private User user;
        public List<Window> openedWindows;
        private TicketConfirmationModal tickConfModal;
       
        
        public GetTicketPage(GetTicketDTO dto, User u, TicketConfirmationModal tickModal)
        {
            InitializeComponent();
            this.tickConfModal = tickModal;
            this.ticketGotSaved += new TicketGotSavedHandler(rerenderSeatDisplay);
            openedWindows = new List<Window>();
            getTicketDTO = dto;
            user = u;
            seatsPage = new SeatDisplay(dto.DrivingLineId, dto.ScheduleId);;
            seatDisplay.Content = seatsPage;
            displayInfo.Content = new ChosenSchedulePage(dto.FromStationScheduleId, dto.UntilStationScheduleId, dto.ScheduleId);
            
        }

        private void rerenderSeatDisplay()
        {
            seatsPage = new SeatDisplay(getTicketDTO.DrivingLineId, getTicketDTO.ScheduleId);
            seatDisplay.Content = seatsPage;

        }

        private void reserveBtn_Click(object sender, RoutedEventArgs e)
        {
            Ticket t = makeTicket();
            if (t == null)
                return;
            t.TicketType = TicketType.Reserved;
            //TicketConfirmationWindow confirmation = new TicketConfirmationWindow(t, "rezervišete", seatsPage.checkedSeatIds, ticketGotSaved);
            // openedWindows.Add(confirmation);
            //confirmation.Show();
            tickConfModal.ShowHandlerDialog(t, "rezervacije", seatsPage.checkedSeatIds, ticketGotSaved);

        }

        private void buyBtn_Click(object sender, RoutedEventArgs e)
        {
            Ticket t = makeTicket();
            if (t == null)
                return;
            t.TicketType = TicketType.Bought;
            //TicketConfirmationWindow confirmation = new TicketConfirmationWindow(t, "kupite", seatsPage.checkedSeatIds, ticketGotSaved);
            // openedWindows.Add(confirmation);
            // confirmation.Show();
            tickConfModal.ShowHandlerDialog(t, "kupovine", seatsPage.checkedSeatIds, ticketGotSaved);
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

        private void buyTicket_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            List<int> chosenSeatsId = seatsPage.checkedSeatIds;
            if (chosenSeatsId.Count == 0)
            {
                MessageBox.Show("Niste izabrali sedište! Pre nastavka morate da izaberete sedište");
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }

        private void buyTicket_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            buyBtn_Click(sender, e);
        }

       /* private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.parentFrame.Content = parentPage;
        }*/
    }

  
}
