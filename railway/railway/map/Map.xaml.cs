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
using GMap.NET.WindowsPresentation;
using railway.model;
using railway.database;
using System.Linq;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace railway.map
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Page
    {
        private List<StationSchedule> stationSchedules;
        private List<Station> stations;
        public Map(List<StationSchedule> ss)
        {
            InitializeComponent();
            stationSchedules = ss;
            stations = new List<Station>();
        }
        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;

            using (var db = new RailwayContext())
            {
                GMap.NET.PointLatLng previous = new GMap.NET.PointLatLng(0, 0);
                GMap.NET.PointLatLng current = new GMap.NET.PointLatLng(0, 0);
                foreach (StationSchedule s in stationSchedules)
                {
                    Station st = (from stations in db.stations
                                  where stations.Id == s.StationId
                                  select stations).Single();
                    stations.Add(st);
                    current = new GMap.NET.PointLatLng(st.Latitude, st.Longitude);
                    GMap.NET.WindowsPresentation.GMapMarker marker = new GMap.NET.WindowsPresentation.GMapMarker(current);
                    marker.Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Stroke = Brushes.Red,
                        StrokeThickness = 1.5,
                        ToolTip = "This is tooltip",
                        Visibility = Visibility.Visible,
                        Fill = Brushes.Red,

                    };
                    mapView.Markers.Add(marker);
                    
                    //routesOverlay.Routes.Add(route);
                    //mapView.Overlays.Add(routesOverlay);

                    //mapView.Children.Add(route);
                }
            }





            // don't forget to add the marker to the map
            //mapView.Markers.Add(routeMarker);

            mapView.MinZoom = 6;
            mapView.MaxZoom = 17;
            // whole world zoom
            mapView.Zoom = 10;
            mapView.Position = new GMap.NET.PointLatLng(44.374229f, 19.105961f);
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.ShowCenter = false;

        }

        private GMapControl addMarkers(GMapControl mapView)
        {
            
            return null;
        }
    }
}
