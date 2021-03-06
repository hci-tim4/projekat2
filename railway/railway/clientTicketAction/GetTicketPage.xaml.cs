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
        //private TicketConfirmationModal tickConfModal;
       
        
        public GetTicketPage(GetTicketDTO dto, User u, TicketConfirmationModal tickModal)
        {
            try{
                InitializeComponent();
                //this.tickConfModal = tickModal;
                this.ticketGotSaved += new TicketGotSavedHandler(rerenderSeatDisplay);
                openedWindows = new List<Window>();
                getTicketDTO = dto;
                user = u;
                seatsPage = new SeatDisplay(dto.DrivingLineId, dto.ScheduleId, dto.FromStationScheduleId);;
                seatDisplay.Content = seatsPage;
                displayInfo.Content = new ChosenSchedulePage(dto.FromStationScheduleId, dto.UntilStationScheduleId, dto.ScheduleId);
                
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void rerenderSeatDisplay()
        {
            seatsPage = new SeatDisplay(getTicketDTO.DrivingLineId, getTicketDTO.ScheduleId, getTicketDTO.FromStationScheduleId);
            seatDisplay.Content = seatsPage;

        }

        private void reserveBtn_Click(object sender, RoutedEventArgs e)
        {
            try{
                Ticket t = makeTicket();
                if (t == null)
                    return;
                t.TicketType = TicketType.Reserved;
                TicketConfirmationWindow confirmation = new TicketConfirmationWindow(t, "rezervišete", seatsPage.checkedSeatIds, ticketGotSaved);
                //openedWindows.Add(confirmation);
                confirmation.ShowDialog();
                //tickConfModal.ShowHandlerDialog(t, "rezervacije", seatsPage.checkedSeatIds, ticketGotSaved);

            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void buyBtn_Click(object sender, RoutedEventArgs e)
        {
            try{
                Ticket t = makeTicket();
                if (t == null)
                    return;
                t.TicketType = TicketType.Bought;
                TicketConfirmationWindow confirmation = new TicketConfirmationWindow(t, "kupite", seatsPage.checkedSeatIds, ticketGotSaved);
                // openedWindows.Add(confirmation);
                confirmation.Show();
                //tickConfModal.ShowHandlerDialog(t, "kupovine", seatsPage.checkedSeatIds, ticketGotSaved);
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private Ticket makeTicket()
        {
            StationSchedule fromStatSched = null;
            using (var db = new RailwayContext())
            {
                fromStatSched =
                    (from ss in db.stationsSchedules where ss.Id == getTicketDTO.FromStationScheduleId select ss).Single();
            }
            Ticket t = new Ticket()
            {
                UserId = user.Id,
                FromStationScheduleId = getTicketDTO.FromStationScheduleId,
                UntilStationScheduleId = getTicketDTO.UntilStationScheduleId,
                ScheduleId = getTicketDTO.ScheduleId,
                Tour = fromStatSched.Tour
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
                if (numberOfStationsBetween < 0)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Greška se desilo.");
                    cmb.ShowDialog();
                    return 0;
                }

                ticketSeats = new List<TicketSeats>();

                if (chosenSeatsId.Count == 0)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Niste izabrali sedište! \nPre nastavka morate da izaberete sedište.");
                    cmb.ShowDialog();
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
                CustomMessageBox cmb = new CustomMessageBox("Niste izabrali sedište! \nPre nastavka morate da izaberete sedište");
                cmb.ShowDialog();
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
