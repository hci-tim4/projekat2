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
    /// Interaction logic for ChosenSchedulePage.xaml
    /// </summary>
    public partial class ChosenSchedulePage : Page
    {
        
        public ChosenSchedulePage(int fromStationScheduleId, int untilStationScheduleId, int scheduleId)
        {
            try
            {
                InitializeComponent();
                TicketDTO t = createTicketDTO(fromStationScheduleId, untilStationScheduleId, scheduleId);
                this.DataContext = t;
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private TicketDTO createTicketDTO(int fromStationScheduleId, int untilStationScheduleId, int scheduleId)
        {
            TicketDTO tDTO = new TicketDTO();
            using (var db = new RailwayContext())
            {
                StationSchedule s1 = (from stationSchedules in db.stationsSchedules
                                    where stationSchedules.Id == fromStationScheduleId
                                    select stationSchedules).Single();
                tDTO.fromStation = s1.Station.Name;

                tDTO.depatureTime = (TimeSpan)s1.DepartureTime;

                StationSchedule s2 = (from stationSchedules in db.stationsSchedules
                                     where stationSchedules.Id == untilStationScheduleId
                                     select stationSchedules).Single();

                tDTO.untilStation = s2.Station.Name;
                tDTO.arrivalTime = (TimeSpan)s2.ArrivalTime;

                Schedule d = (Schedule)(from schedules in db.schedules
                                        where schedules.Id == scheduleId
                                        select schedules).Single();
                tDTO.Date = d.DepatureDate;
            }
            return tDTO;
        }

    }
}
