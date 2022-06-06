﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using railway.database;
using railway.defineDrivingLine;
using railway.model;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace railway.CRUDDrivingLine
{
    public delegate void DrivingGotSavedHandler();
    public partial class AddDrivingLine : UserControl
    {
        public event DrivingGotSavedHandler drivingLineGotSaved;
        private Frame parentFrame;
        private ViewDrivingLines parentPage;
        public PointLatLng? previous { get; set; }
        
        
        public AddDrivingLine(Frame parentFrame, ViewDrivingLines viewDrivingLines)
        {
            InitializeComponent();
            this.drivingLineGotSaved += new DrivingGotSavedHandler(clearMap);
            this.parentFrame = parentFrame;
            this.parentFrame.Content = this;
            this.parentPage = viewDrivingLines;
            using (var db = new RailwayContext())
            {
                List<Station> s = (from st in db.stations orderby st.Name select st).ToList();
                
                stations = new ObservableCollection<Station>(s);
                stations2 = new ObservableCollection<Station>();
            }

            this.DataContext = this;
        }

        private void clearMap()
        {
            addStationsBack();
            stations2 = new ObservableCollection<Station>();
            setUpMapView();
            this.DataContext = this;
        }

        private void addStationsBack()
        {
            foreach (Station st in stations2)
            {
                stations.Add(st);
            }

            stations = new ObservableCollection<Station>(stations.OrderBy(x => x.Name).ToList());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            parentPage.setDrivingLines(new RailwayContext());
            this.parentFrame.Content = parentPage;
        }
        
        
        
        
        Point startPoint = new Point();

        public ObservableCollection<Station> stations
        {
            get;
            set;
        }

        public ObservableCollection<Station> stations2
        {
            get;
            set;
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    // Get the dragged ListViewItem
                    ListView listView = sender as ListView;
                    ListViewItem listViewItem =
                        FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                    // Find the data behind the ListViewItem
                    Station station = (Station)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", station);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
            catch (Exception)
            {
                
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Station station = e.Data.GetData("myFormat") as Station;
                stations.Remove(station);
                stations2.Add(station);
                //setUpMapView();
                addNewMarker(station);
            }
        }

        private void addNewMarker(Station s)
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
            }
            else
            {
                drawLineBetweenPoints(current, previous, mapView);
                previous = new PointLatLng(current.Lat, current.Lng);
            }

        }
        
        private void resetStations()
        {
            using (var db = new RailwayContext())
            {
                List<Station> s = (from st in db.stations orderby st.Name select st).ToList();
                stations = new ObservableCollection<Station>(s);//s;
            }
        }

        public void setUpMapView()
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

            previous = null;
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

        private void searchForStation(object sender, KeyEventArgs e)
        {
            string input = searchForStationTextBox.Text;
            using (var db = new RailwayContext())
            {
                List<Station> s = (from st in db.stations where st.Name.Contains(input) orderby st.Name select st).ToList();
                foreach (Station s2 in stations2)
                {
                    if (s.Contains(s2))
                    {
                        s.Remove(s2);
                    }
                }
                stations = new ObservableCollection<Station>(s);
                stationList.ItemsSource = stations;
            }
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

        private void ChangeDrivingLineDefView_OnClick(object sender, RoutedEventArgs e)
        {
            this.parentFrame.Content = new AddDrivingLineSimple(parentFrame, parentPage, this, stations2, stations);
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
    }
}