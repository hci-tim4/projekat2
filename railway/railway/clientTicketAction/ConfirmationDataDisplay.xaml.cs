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
    /// Interaction logic for ConfirmationDataDisplay.xaml
    /// </summary>
    public partial class ConfirmationDataDisplay : Page
    {
        public ConfirmationDataDisplay(Ticket ticket, int numberOfTickets)
        {
            InitializeComponent();
            ConfirmationTicketDTO confTicketDTO = createConfirmationTicketDTO(ticket, numberOfTickets);
            this.DataContext = confTicketDTO;
        }
        private ConfirmationTicketDTO createConfirmationTicketDTO(Ticket ticket, int numberOfTickets)
        {
            ConfirmationTicketDTO confTicketDTO = new ConfirmationTicketDTO();
            using (var db = new RailwayContext())
            {
                StationSchedule s1 = (from stationSchedules in db.stationsSchedules
                                    where stationSchedules.Id == ticket.FromStationScheduleId
                                    select stationSchedules).Single();
                confTicketDTO.fromStation = s1.Station.Name;
                confTicketDTO.depatureTime = (TimeSpan)s1.DepartureTime;

                StationSchedule s2 = (from stationSchedules in db.stationsSchedules
                                     where stationSchedules.Id == ticket.UntilStationScheduleId
                                     select stationSchedules).Single();
                confTicketDTO.untilStation = s2.Station.Name;
                confTicketDTO.arrivalTime = (TimeSpan)s2.ArrivalTime;

                Schedule d = (Schedule)(from schedules in db.schedules
                                        where schedules.Id == ticket.ScheduleId
                                        select schedules).Single();
                confTicketDTO.Date = d.DepatureDate;
                confTicketDTO.ticketCount = numberOfTickets;
                confTicketDTO.price = ticket.Price;
            }
            return confTicketDTO;
        }
    }
}
