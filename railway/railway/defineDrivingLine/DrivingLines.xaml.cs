using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using railway.database;
using railway.defineDrivingLine;
using railway.exception;
using railway.model;
using railway.services;

namespace railway.defineDrivingLine
{
    public partial class DrivingLines : UserControl
    {
        public List<DrivingLineViewDTO> DrivingLinesList;
        private List<Train> trains;
        private List<Train> fullTrains;
        private Train newTrain;
        private Frame parentFrame;
        private DrivingLineViewDTO currentSelected = null;
        private Station currentStation;
        private List<Station> stations;
        private DefineEndDateForDrivingLineModal defEndDateModal;
        
        public DrivingLines(Frame parentFrame, DefineEndDateForDrivingLineModal defineEndDateForDrivingLineModal)
        {
            InitializeComponent();
            //this.parentFrame = frame;
            this.parentFrame = parentFrame;
            this.DataContext = DrivingLinesList;
            using (var db = new RailwayContext())
            {
                setDrivingLines(db);
                trains = (from trains in db.trains
                    select trains).ToList();
                trainNameCmb.ItemsSource = trains;
                fullTrains = trains;
                stations = (from st in db.stations orderby st.Name select st).ToList();
            }

            this.defEndDateModal = defineEndDateForDrivingLineModal;
        }
        
        
        public void setDrivingLines(RailwayContext db)
        {
            this.DrivingLinesList = (from dl in db.drivingLines
                where dl.deleted == false
                select new DrivingLineViewDTO()
                {
                    DrivingLineId = dl.Id,
                    Name = dl.Name,
                    TrainName = dl.Train.Name,
                    startDate = dl.startDate,
                    endDate = dl.endDate
                }).ToList();
            drivingLineDataGrid.ItemsSource = DrivingLinesList;
        }

        private void ViewSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void DeleteDrivingLine_OnClick(object sender, RoutedEventArgs e)
        {
            
            var buttonId = ((Button)sender).Tag;
            //DrivingLineDTO dto = findDTOById((int)dtoId);
            int drivingLineId = (int)buttonId;
            DrivingLineViewDTO send = null;
            foreach (DrivingLineViewDTO dto in DrivingLinesList)
            {
                if (dto.DrivingLineId == drivingLineId)
                {
                    send = dto;
                    break;
                }
                    
            }
            defEndDateModal.ShowHandlerDialog(send, this);

            //DefineEndDateForDrivingLine window = new DefineEndDateForDrivingLine(send, this);
            //window.Show();
            /*
            using (var db = new RailwayContext())
            {
                List<Ticket> tickets = (from t in db.tickets
                    join s in db.schedules
                        on t.ScheduleId equals s.Id
                    where s.DepatureDate > DateTime.Now && s.DrivingLineId == drivingLineId
                    select t).ToList();
                if (tickets.Count > 0)
                    MessageBox.Show("Postoje prodate karte za liniju, brisanje je onemogućen");
                else
                {
                    DrivingLine dl = (from d in db.drivingLines
                        where d.Id == drivingLineId
                        select d).Single();
                    dl.deleted = true;
                    MessageBox.Show(
                        "Uspešno ste logičko brisali mrežnu liniju. I dalje možete da vidite kod izveštaja");
                    db.SaveChanges();
                    setDrivingLines(db);
                }
            }*/
        }

        private void ChangeLine_OnClick(object sender, RoutedEventArgs e)
        {
        }
        
        private void TrainNameChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            var trainNameCmb = sender as ComboBox;

            //this. = trainNameCmb.Text.ToLower();
            this.newTrain = (from item in trains
                where item.Name.ToLower().Equals(trainNameCmb.Text.ToLower())
                select item).FirstOrDefault();
            
            
            this.trains = (from item in fullTrains
                where item.Name.ToLower().Contains(trainNameCmb.Text.ToLower())
                select item).ToList();
            trainNameCmb.ItemsSource = trains;

            trainNameCmb.IsDropDownOpen = true;
        }

        private void DrivingLineDataGrid_OnSelected(object sender, RoutedEventArgs e)
        {
            currentSelected = drivingLineDataGrid.SelectedItem as DrivingLineViewDTO;
            newName.Text = currentSelected.Name;
            trainNameCmb.SelectedItem = (from ts in this.fullTrains
                                        where ts.Name == currentSelected.TrainName
                                            select ts).Single();//Datarow.Cells[0].Value//
            trainNameCmb.IsDropDownOpen = false;
            startDate.SelectedDate = currentSelected.startDate;
            startDate.IsEnabled = false;
            if (currentSelected.startDate < DateTime.Now)
                startDate.IsEnabled = false;
            endDate.SelectedDate = currentSelected.endDate;
            setStationsOnDrivingLine();
        }

        private void setStationsOnDrivingLine()
        {
            
            using (var db = new RailwayContext())
            {
                List<StationDTO> ss = (from sdb in db.stationsSchedules
                    where sdb.DrivingLineId == currentSelected.DrivingLineId && sdb.deleted == false
                    select new StationDTO()
                    {
                        StationId = sdb.Station.Id,
                        StationName = sdb.Station.Name
                    }).ToList();
                currentSelected.stationSchedules = ss;
            }
            
            //stationScheduleDataGrid.ItemsSource = currentSelected.stationSchedules;
        }
        
        private void AddDrivingLine_OnClick(object sender, RoutedEventArgs e)
        {
            UserControl add = new AddDrivingLine(parentFrame, this);
            //this.parentFrame.Content = add;
        }

        private void SaveChanges_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentSelected == null)
            {
                MessageBox.Show("Prvo morate da izaberete mrežnu liniju koji hoćete da editujete");
                return;
            }
            string name = newName.Text;
            DrivingLineService dlService = new DrivingLineService();
            try
            {
                dlService.updateBasicDrivingService(name, newTrain, currentSelected.DrivingLineId);
                MessageBox.Show("Mrežna linija je uspešno sačuvana");
                setDrivingLines(new RailwayContext());
            }
            catch (NotDefinedException nd)
            {
                MessageBox.Show(nd.message);
            }
            catch (AlreadyDefinedException ad)
            {
                MessageBox.Show(ad.message);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("Ups, neočekivana greška se desilo");
            }

        }

        private void RemoveStation_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonId = ((Button)sender).Tag;
            //DrivingLineDTO dto = findDTOById((int)dtoId);
            int stationId = (int)buttonId;
            using (var db = new RailwayContext())
            {
                List<StationSchedule> stationSchedules = (from ss in db.stationsSchedules
                    where ss.DrivingLineId == currentSelected.DrivingLineId
                    select ss).ToList();
                bool removed = false;
                int previousSerialNumber = -1;
                foreach (StationSchedule s in stationSchedules)
                {
                    if (removed)
                        s.SerialNumber = previousSerialNumber;
                    if (s.StationId == stationId)
                    {
                        s.deleted = true;
                        removed = true;
                    }
                    previousSerialNumber = s.SerialNumber;
                }

                db.SaveChanges();
                setStationsOnDrivingLine();
            }
        }

        //SACUVAJ ON SACUVAJ IZMENE!!!!!!!!!!!
        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new RailwayContext())
            {
                List<StationSchedule> stationSchedules = (from ss in db.stationsSchedules
                    where ss.DrivingLineId == currentSelected.DrivingLineId
                    where ss.deleted == false
                    select ss).ToList();
                StationSchedule newStationSchedule = new StationSchedule()
                {
                    SerialNumber = stationSchedules.Count,
                    StationId = currentStation.Id,
                    ArrivalTime = null,
                    DepartureTime = null,
                    DrivingLineId = currentSelected.DrivingLineId,
                    deleted = false
                };
                db.stationsSchedules.Add(newStationSchedule);
                db.SaveChanges();
                setStationsOnDrivingLine();
            }
        }

        private void EventSetter_OnHandler(object sender, TextChangedEventArgs e)
        {
            var cmbx = sender as ComboBox;
            cmbx.ItemsSource = from item in stations
                where item.Name.ToLower().Contains(cmbx.Text.ToLower())
                select item;
            this.currentStation = (from item in stations
                where item.Name.ToLower().Equals(cmbx.Text.ToLower())
                select item).FirstOrDefault();

            cmbx.IsDropDownOpen = true;
        }

        
    }
}