using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using railway.database;
using railway.defineDrivingLine;
using railway.model;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;

namespace railway.defineDrivingLine
{
    public partial class AddDrivingLineSimple : UserControl, TutorialInterface
    {
        
        public event DrivingGotSavedHandler drivingLineGotSaved;
        private Frame parentFrame;
        private DrivingLines parentPage;
        private ObservableCollection<Station> stations;
        private ObservableCollection<Station> stations2;
        private AddDrivingLine addDrivingLineDragAndDrop;
        private Station currentStation;
        private StationSchedule changedStationSchedule;
        private PointLatLng? previous = null;
        private ViewDrivingLines greatParentPage;
        private Boolean Touring = false;
        
        public AddDrivingLineSimple(Frame parentFrame, DrivingLines viewDrivingLines, AddDrivingLine dragAndDrop,
            ObservableCollection<Station> stations2, ObservableCollection<Station> stations, ViewDrivingLines greatParentPage)
        {
            InitializeComponent();
            this.greatParentPage = greatParentPage; 
            this.drivingLineGotSaved += new DrivingGotSavedHandler(clearMap);
            this.parentFrame = parentFrame;
            //this.parentFrame.Content = this;
            this.parentPage = viewDrivingLines;

            this.stations2 = stations2;
            this.stations = stations;
            this.DataContext = this;
            stationsCmb.ItemsSource = stations;
            addDrivingLineDragAndDrop = dragAndDrop;
        }

        private void resetStations()
        {
            using (var db = new RailwayContext())
            {
                List<Station> s = (from st in db.stations orderby st.Name select st).ToList();
                stations = new ObservableCollection<Station>(s);//s;
            }
        }

        private void clearMap()
        {
            stations2 = new ObservableCollection<Station>();
            setUpMapView();
            resetStations();
            this.DataContext = this;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            parentPage.setDrivingLines(new RailwayContext());
            this.parentFrame.Content = parentPage;
        }
        

        private void setUpMapView()
        {
            
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            mapView.Markers.Clear();

            // don't forget to add the marker to the map
            //mapView.Markers.Add(routeMarker);

            mapView.MinZoom = 3;
            mapView.MaxZoom = 17;
            // whole world zoom
            mapView.Zoom = 7;
            mapView.Position = new GMap.NET.PointLatLng(44.81583333, 20.45944444);
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.ShowCenter = false;

            foreach (Station s in stations2)
            {
                GMap.NET.PointLatLng current = new GMap.NET.PointLatLng(s.Latitude, s.Longitude);
                GMap.NET.WindowsPresentation.GMapMarker marker = new GMap.NET.WindowsPresentation.GMapMarker(current);
                
                marker.Shape = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.Firebrick,
                    StrokeThickness = 1.5,
                    ToolTip = "Stanica",
                    Visibility = Visibility.Visible,
                    Fill = Brushes.Firebrick,

                };
                mapView.Markers.Add(marker);
                
                
                if (previous == null)
                {
                    previous = new PointLatLng(current.Lat, current.Lng);
                    continue;
                }
                else
                {
                    drawLineBetweenPoints(current, previous, mapView);
                    previous = new PointLatLng(current.Lat, current.Lng);
                }
            }
            
            
            
        }

        
        private void drawLineBetweenPoints(PointLatLng current, PointLatLng? previous, GMapControl gMapControl)
        {
            double dis = CountDistanceBetweenPoints(current, (PointLatLng)previous);
            if (dis < 0.01)
                return;
            PointLatLng middle = CountMiddlePoint(current, (PointLatLng)previous);
            GMap.NET.WindowsPresentation.GMapMarker markerLine = new GMap.NET.WindowsPresentation.GMapMarker(middle);
            markerLine.Shape = new Ellipse
            {
                Width = 3,
                Height = 3,
                Stroke = Brushes.Goldenrod,
                StrokeThickness = 1.5,
                ToolTip = "Put",
                Visibility = Visibility.Visible,
                Fill = Brushes.Goldenrod,

            };
            mapView.Markers.Add(markerLine);
            drawLineBetweenPoints(current, middle, mapView);
            drawLineBetweenPoints((PointLatLng)previous, middle, mapView);
        }

        private double CountDistanceBetweenPoints(PointLatLng p1, PointLatLng p2)
        {
            return Math.Sqrt(Math.Pow(p1.Lat - p2.Lat, 2) + Math.Pow(p1.Lng - p2.Lng, 2));
        }
        
        private PointLatLng CountMiddlePoint(PointLatLng p1, PointLatLng p2)
        {
            return new PointLatLng((p1.Lat +p2.Lat)/2, (p1.Lng + p2.Lng)/2);
        }


        private void map_Loaded(object sender, RoutedEventArgs e)
        {
            setUpMapView();
        }


        private void SaveDrivingLine_OnClick(object sender, RoutedEventArgs e)
        {
            //odabir voza i imena!
            if (stations2.Count < 2)
            {
                CustomMessageBox cmb = new CustomMessageBox("Mrežna linija mora da sadrži barem 2 stanice");
                cmb.ShowDialog();
                return;
            }
            Window def = new DefineSimpleDataForDrivingLine(stations2, drivingLineGotSaved);
            def.Show();
            //defSimpleData.ShowHandlerDialog(stations2, drivingLineGotSaved);
        }

        private void BackToDragAndDrop_OnClick(object sender, RoutedEventArgs e)
        {
            addDrivingLineDragAndDrop.stations = this.stations;
            addDrivingLineDragAndDrop.stations2 = this.stations2;
            addDrivingLineDragAndDrop.setUpMapView();
            this.parentFrame.Content = addDrivingLineDragAndDrop;
            this.greatParentPage.CurrentComponent = addDrivingLineDragAndDrop;
        }

        private void EventSetter_OnHandler(object sender, TextChangedEventArgs e)
        {
            var cmbx = sender as ComboBox;
            cmbx.ItemsSource = (from item in stations
                where item.Name.ToLower().Contains(cmbx.Text.ToLower())
                select item).ToList();
            this.currentStation  = (from item in stations
                where item.Name.ToLower().Equals(cmbx.Text.ToLower())
                select item).FirstOrDefault();

            if (Touring && currentStation != null)
            {
                IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals("AllDrivingLines").GoNext();
            }

            cmbx.IsDropDownOpen = true;
        }

        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentStation == null)
            {
                CustomMessageBox cmb = new CustomMessageBox("Prvo morate da izaberete stanicu");
                cmb.ShowDialog();
                return;
            }
            if (Touring)
            {
                IFeatureTourNavigator navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals("AddStation").GoNext();
            }
            stations2.Add(currentStation);
            stations.Remove(currentStation);
            stationsCmb.ItemsSource = stations;
            setUpMapView();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }
        
        
        
        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            parentPage.setDrivingLines(new RailwayContext());
            greatParentPage.CurrentComponent = parentPage;
            parentFrame.Content = parentPage;
        }
     
        
        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveDrivingLine_OnClick(sender, e);
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
                    new Step("AllDrivingLines", "Lista stanica", "U listi se nalaze stanice koje mogu da budu dodate u mrežnu liniju.")
                    {
                        ShowNextButton = false
                    },
                    new Step("AddStation", "Dodajte stanicu", "Kliknite na dugme 'Dodaj' da biste dodali stanicu.")
                    {
                        ShowNextButton = false
                    },
                    new Step("MapOfDrivingLine", "Mapa", "Nakon odabira željene stanice, ona se prikaže na mapi."),
                    new Step("AllDrivingLinesStack", "Definiši mrežnu liniju", "Ponavljajte ove korake dok ne dodate sve željene stanice."),
                    new Step("SaveDrivingLine", "Sačuvaj", "Kada završite sa dodavanjem stanica klikom na dugme 'Sačuvaj' sačuvaćete novu mrežnu liniju."),
                    new Step("ChangeTypeOfDrivingLineDefinition", "Promeni tip definisanja", "Na raspolaganju imate i drugačiji način defnisanja mrežne linije. Kliknite 'Prevlačenje' da biste videli."),
                    new Step("BackButton", "Vraćanje nazad", "Ako ste završili kliknite na strelicu da biste se vratili na listu mrežnih linija."),
                    
                },
                
            };

            tour.Start();

        }
    }
    
    
}