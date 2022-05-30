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

namespace railway.CRUDDrivingLine
{
    public partial class AddDrivingLineSimple : UserControl
    {
        
        public event DrivingGotSavedHandler drivingLineGotSaved;
        private Frame parentFrame;
        private ViewDrivingLines parentPage;
        private List<Station> stations;
        private ObservableCollection<Station> stations2;
        private AddDrivingLine addDrivingLineDragAndDrop;
        private Station currentStation;
        private StationSchedule changedStationSchedule;
        
        public AddDrivingLineSimple(Frame parentFrame, ViewDrivingLines viewDrivingLines, AddDrivingLine dragAndDrop, ObservableCollection<Station> stations2)
        {
            InitializeComponent();
            this.drivingLineGotSaved += new DrivingGotSavedHandler(clearMap);
            this.parentFrame = parentFrame;
            this.parentFrame.Content = this;
            this.parentPage = viewDrivingLines;
            using (var db = new RailwayContext())
            {
                List<Station> s = (from st in db.stations orderby st.Name select st).ToList();
                
                stations = s;
                stationsCmb.ItemsSource = s;
                this.stations2 = stations2;
            }

            this.DataContext = this;
            addDrivingLineDragAndDrop = dragAndDrop;
        }

        private void clearMap()
        {
            stations2 = new ObservableCollection<Station>();
            setUpMapView();
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
            }
            
            
            
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
            addDrivingLineDragAndDrop.stations2 = this.stations2;
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
    }
    
    
}