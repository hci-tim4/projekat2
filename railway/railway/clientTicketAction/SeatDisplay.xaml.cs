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
using railway.database;
using railway.model;
using System.Linq;
using System.IO;

namespace railway.client
{
    /// <summary>
    /// Interaction logic for SeatDisplay.xaml
    /// </summary>
    public partial class SeatDisplay : Page
    {
        int colCount = -1;
        int rowCount = -1;
        public List<int> occupiedSeats { get; set; }
        private string checkboxNamePrefix = "s";

        public List<int> checkedSeatIds { get; set; }

        public SeatDisplay(int drivingLineID, int scheduleId, int dtoFromStationScheduleId)
        {
            try{
                InitializeComponent();

                checkedSeatIds = new List<int>();

                getOccupiedSeats(scheduleId, dtoFromStationScheduleId);
                displaySeats(drivingLineID);
                addLegend();

                addCol();
            
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void getOccupiedSeats(int scheduleId, int dtoFromStationScheduleId)
        {
            using (var db = new RailwayContext())
            {
                List<Ticket> tickets = (from schedules in db.schedules
                                        where schedules.Id == scheduleId
                                        select schedules.Tickets).Single().ToList();
                StationSchedule fromStatSched =
                    (from ss in db.stationsSchedules where ss.Id == dtoFromStationScheduleId select ss).Single();
                tickets = (from t in tickets where t.Tour == fromStatSched.Tour select t).ToList();
                List<int> seatIds = new List<int>();
                foreach (Ticket t in tickets) {
                    List<Seat> seats = (from ticketSeats in db.ticketSeats
                              where ticketSeats.TicketId == t.Id
                              select ticketSeats.Seat).ToList();
                    foreach (Seat s in seats)
                    {
                        if (s == null)
                            continue;
                        seatIds.Add(s.Id);
                    }
                }
                this.occupiedSeats = seatIds;
            }
        }

        private void displaySeats(int drivingLineID)
        {
            using (var db = new RailwayContext())
            {
                Train train = (from drivingLines in db.drivingLines
                               where drivingLines.Id == drivingLineID
                               select drivingLines.Train).Single();
                List<Seat> seats = train.Seats.OrderBy(seat => seat.Row).ThenBy(seat => seat.Col).ToList();
                addRow();
                foreach (Seat s in seats)
                {
                    if (s.Row >= rowCount)
                    {
                        addRow();
                    }
                    if (s.Col > colCount)
                    {
                        addCol();
                    }
                    addSeat(s);
                }
            }
        }

        private void addSeat(Seat s)
        {
            StackPanel innerStack = new StackPanel();
            innerStack.Orientation = Orientation.Horizontal;


            Border myBorder1 = new Border();
            myBorder1.Margin = new Thickness(2, 2, 2, 2);
            myBorder1.Padding = new Thickness(2, 2, 2, 2);
            myBorder1.BorderBrush = Brushes.Black;
            myBorder1.BorderThickness = new Thickness(1);
            myBorder1.Child = innerStack;

            CheckBox cb = new CheckBox();
            cb.Name = checkboxNamePrefix + s.Id.ToString();

            Image i = getImageForId(-1);
            if (this.occupiedSeats.Contains(s.Id))
            {
                innerStack.Opacity = 50;
                cb.IsEnabled = false;
            }
            else
            {
                cb.Click += new RoutedEventHandler(checkbox_Click);
                i = getImageForId(s.SeatTypeId);
            }

            
            innerStack.Children.Add(cb);
            innerStack.Children.Add(i);

            Grid.SetColumn(myBorder1,  ++s.Col);
            Grid.SetRow(myBorder1,  ++s.Row);

            grid1.Children.Add(myBorder1);
        }

        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = e.Source as CheckBox;
            string cbName = cb.Name;
            int seatID = Int32.Parse(cbName.Substring(checkboxNamePrefix.Length));
            if (checkedSeatIds.Contains(seatID))
            {
                checkedSeatIds.Remove(seatID);
            }
            else
            {
                checkedSeatIds.Add(seatID);
            }
        }

        private Image getImageForId(int seatTypeId)
        {
            Uri uri = makeUri(seatTypeId);

            var b = new System.Windows.Media.Imaging.BitmapImage(uri);
            var i = new System.Windows.Controls.Image();
            i.Width = 20;
            i.Height = 15;
            i.Source = b;
            return i;
        }

        private Uri makeUri(int seatTypeId)
        {
            //var uri = new Uri("https://www.clipartmax.com/png/middle/112-1120561_black-x-transparent-background.png");
            var uri = new Uri("/icon/taken.png", UriKind.Relative);
            try
            {
                switch (seatTypeId)
                {
                    case 1:
                        uri = new Uri("/icon/vip.png", UriKind.Relative);//uri = new Uri("https://www.seekpng.com/png/full/38-387704_star-vector-png-transparent-image-star-vector-png.png");
                        break;
                    case 2:
                        uri = new Uri("/icon/buisness.png", UriKind.Relative);//uri = new Uri("https://cdn-icons-png.flaticon.com/512/2345/2345130.png");
                        break;
                    case 3:
                        uri = new Uri("/icon/economy.png", UriKind.Relative);//uri = new Uri("https://flyclipart.com/thumb2/black-circle-png-free-download-557178.png");
                        break;
                }
            }
            catch
            {

            }
            return uri;
        }


        private void addLegend()
        {
            //RowDefinition rowDef = new RowDefinition();
            //rowDef.Height = GridLength.Auto; // new GridLength(2, GridUnitType.Star);
            //grid2.RowDefinitions.Add(rowDef);
            //rowCount++;
            StackPanel sp = new StackPanel();
            using (var db = new RailwayContext())
            {
                foreach (SeatType st in db.seatTypes)
                {
                    DockPanel p = addElement(st, sp);
                    sp.Children.Add(p);
                }
            }
            DockPanel occupiedLegend = makeOccupeidLegend();
            sp.Children.Add(occupiedLegend);
            //Grid.SetRow(sp, rowCount++);
           // Grid.SetColumn(sp, 0);
            //Grid.SetColumnSpan(sp, colCount + 1);
            legend.Children.Add(sp);
        }

        private DockPanel makeOccupeidLegend()
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(1, 1, 1, 1);
            Image i = getImageForId(-1);
            dp.Children.Add(i);
            Label l = new Label
            {
                FontSize= 20,
                Content = "Zauzet"
            };
            dp.Children.Add(l);
            return dp;
        }

        private DockPanel addElement(SeatType st, StackPanel sp)
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(1, 1, 1, 1);
            Image i = getImageForId(st.Id);
            dp.Children.Add(i);
            Label l = new Label
            {
                FontSize= 20,
                Content = st.Name
            };
            dp.Children.Add(l);
            return dp;
        }

        private void addCol()
        {
            ColumnDefinition colDef = new ColumnDefinition();
            colDef.Width = GridLength.Auto; // new GridLength(2, GridUnitType.Star); ;
            grid1.ColumnDefinitions.Add(colDef);

            colCount++;
            if (colCount > 0)
                addColNumbering();

        }

        private void addRow()
        {
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto; // new GridLength(2, GridUnitType.Star);
            grid1.RowDefinitions.Add(rowDef);

            rowCount++;
            if (rowCount > 0)
                addRowNumbering();



        }

        private void addColNumbering()
        {
            Label l = new Label
            {
                Content = colCount
            };
            Grid.SetColumn(l, colCount);
            Grid.SetRow(l, 0);
            grid1.Children.Add(l);
        }

        private void addRowNumbering()
        {
            Label l = new Label
            {
                Content = rowCount
            };
            Grid.SetColumn(l, 0);
            Grid.SetRow(l, rowCount);
            grid1.Children.Add(l);
        }
    }
}