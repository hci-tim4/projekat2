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
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace railway.defineDrivingLine
{
    public partial class AddDrivingLineSimple : UserControl
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
        
        public AddDrivingLineSimple(Frame parentFrame, DrivingLines viewDrivingLines, AddDrivingLine dragAndDrop,
            ObservableCollection<Station> stations2, ObservableCollection<Station> stations)
        {
            InitializeComponent();
            this.drivingLineGotSaved += new DrivingGotSavedHandler(clearMap);
            this.parentFrame = parentFrame;
            //this.parentFrame.Content = this;
            this.parentPage = viewDrivingLines;

            this.DataContext = this;
            this.stations2 = stations2;
            this.stations = stations;
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
            if (dis < 0.001)
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
                MessageBox.Show("Mrežna linija mora da sadrži barem 2 stanice");
                return;
            }
            Window def = new DefineSimpleDataForDrivingLine(stations2, drivingLineGotSaved);
            def.Show();
        }

        private void BackToDragAndDrop_OnClick(object sender, RoutedEventArgs e)
        {
            addDrivingLineDragAndDrop.stations = this.stations;
            addDrivingLineDragAndDrop.stations2 = this.stations2;
            addDrivingLineDragAndDrop.setUpMapView();
            this.parentFrame.Content = addDrivingLineDragAndDrop;
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

        private void AddStation_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentStation == null)
            {
                MessageBox.Show("Prvo morate da izaberete stanicu");
                return;
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
            parentFrame.Content = parentPage;
        }
        
    }
    
    
}