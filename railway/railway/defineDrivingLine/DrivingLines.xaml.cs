using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using railway.database;
using railway.defineDrivingLine;
using railway.exception;
using railway.model;
using railway.services;
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;

namespace railway.defineDrivingLine
{
    public partial class DrivingLines : UserControl, TutorialInterface
    {
        public List<DrivingLineViewDTO> DrivingLinesList;
        private List<Train> trains;
        private List<Train> fullTrains;
        private Train newTrain;
        private Frame parentFrame;
        private ViewDrivingLines parentPage;
        private DrivingLineViewDTO currentSelected = null;
        private Station currentStation;
        private List<Station> stations;
        private DefineEndDateForDrivingLineModal defEndDateModal;
        private DefineSimpleDataForDrivingLineModal defSimpleData;
        private Boolean Touring;
        
        public DrivingLines(Frame parentFrame, DefineEndDateForDrivingLineModal defineEndDateForDrivingLineModal,
            DefineSimpleDataForDrivingLineModal defineSimpleDataForDrivingLineModal, ViewDrivingLines viewDrivingLines)
        {
            InitializeComponent();
            //this.parentFrame = frame;
            this.parentFrame = parentFrame;
            this.parentPage = viewDrivingLines;
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
            this.defSimpleData = defineSimpleDataForDrivingLineModal;
            
            startDate.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
            endDate.Language = XmlLanguage.GetLanguage(new System.Globalization.CultureInfo("sr-ME").IetfLanguageTag);
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

            send.newEndDate = send.endDate;
            //defEndDateModal.ShowHandlerDialog(send, this);

            DefineEndDateForDrivingLine window = new DefineEndDateForDrivingLine(send, this);
            window.ShowDialog();
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
            setMap();
            setStationsOnDrivingLine();
            
            IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals("datagrid").GoNext();
        }

        private void setMap()
        {
            if (currentSelected == null)
            {
                CustomMessageBox cmb = new CustomMessageBox("Prvo morate da izaberete mrežnu liniju");
                cmb.ShowDialog();
                return;
            }

            using (var db = new RailwayContext())
            {
                List<StationSchedule> ss = (from sts in db.stationsSchedules
                    where currentSelected.DrivingLineId == sts.DrivingLineId && sts.Tour == 1
                    orderby sts.SerialNumber
                    select sts).ToList();
                int lastIndex = ss.Count - 1;
                mapPage.Content = new map.Map(ss, ss[0].StationId, ss[lastIndex].StationId);
            }
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
            parentPage.CurrentComponent = new AddDrivingLine(parentFrame, this, defSimpleData, parentPage);
            //this.parentFrame.Content = add;
        }

        private void SaveChanges_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentSelected == null)
            {
                CustomMessageBox cmb = new CustomMessageBox("Prvo morate da izaberete mrežnu liniju koji hoćete da editujete");
                cmb.ShowDialog();
                return;
            }
            string name = newName.Text;
            DrivingLineService dlService = new DrivingLineService();
            try
            {
                dlService.updateBasicDrivingService(name, newTrain, currentSelected.DrivingLineId);
                CustomMessageBox cmb = new CustomMessageBox("Mrežna linija je uspešno sačuvana");
                cmb.ShowDialog();
                setDrivingLines(new RailwayContext());
            }
            catch (NotDefinedException nd)
            {
                CustomMessageBox cmb = new CustomMessageBox(nd.message);
                cmb.ShowDialog();
            }
            catch (AlreadyDefinedException ad)
            {
                CustomMessageBox cmb = new CustomMessageBox(ad.message);
                cmb.ShowDialog();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                CustomMessageBox cmb = new CustomMessageBox("Ups, neočekivana greška se desilo");
                cmb.Show();
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
                    new Step("wholeDataGrid", "Tebala mrežnih linija", "Prikazani su podaci za pojedinačne mrežne linije."),
                    new Step("defEndDateButton", "Definisanje krajnjeg datuma", "Omogućeno je definisanje datuma do kad " +
                        "će saobraćati mrežna linija."),
                    new Step("datagrid", "Tabela mrežnih linija", "Izaberite jednu od linija za nastavak.")
                    {
                        ShowNextButton = false
                    },
                    new Step("changeNameTextBox", "Ime", "Prikaz imena izabrane mrežne linije"),
                    new Step("changeTrainComboBox", "Voz", "Prikaz voza izabrane mrežne linije"),
                    new Step("startDateDatePicker", "Početni datum", "Datum kada mrežna linija počinje da saobraća."),
                    new Step("endDateDatePicker", "Krajnji datum", "Datum kada mrežna linija prestaje da saobraća."),
                    new Step("saveChangesButton", "Sačuvaj", "Klikom na dugme 'Sačuvaj izmene', sačuvaće se napravljane izmene nad izabranom mrežnom linijom."),
                    new Step("routeOnMap", "Ruta", "Sve stanice izabrane mrežne linije."),
                    new Step("DefNewDrivingLineButton", "Definisanje nove mrežne linije", "Klikom na dugme '+' možete da definišete novu mrežnu liniju.")
                    // ...
                },
                
            };

            tour.Start();
            
            
            
            //Step s1 = 
            //if (currentSelected != null)
            //{ 
            //navigator.ForStep("stepDataGrid").s;
            //}

            IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
            //navigator.

            //navigator.IfCurrentStepEquals("datagrid").GoPrevious();
            //navigator.IfCurrentStepEquals("datagrid").Close();
        }
    }
}