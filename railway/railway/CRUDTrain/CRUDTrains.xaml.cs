using railway.database;
using railway.dto.trains;
using railway.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace railway.CRUDTrain
{
    /// <summary>
    /// Interaction logic for CRUDTrains.xaml
    /// </summary>
    public partial class CRUDTrains : Page
    {
        List<TrainDTO> dto;
        Frame page;

       

        public CRUDTrains()
        {
            InitializeComponent();
            AddTrainModal.SetParent(gridTrains);
            EditTrainModal.SetParent(gridTrains);
            DeleteTrainModal.SetParent(gridTrains);
            dto = TrainService.getTrains();
     
            //loggedUser = user;



            // this.DataContext = dto;
            //dataGrid.ItemsSource = dto;
        }

        private void edit_clicked(object sender, RoutedEventArgs e)
        {

        }

        private void delete_clicked(object sender, RoutedEventArgs e)
        {

        }

        private void save_clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reloadWindow();
           
        }

        private void reloadWindow()
        {

            gridTrains.RowDefinitions.Clear();
            gridTrains.ColumnDefinitions.Clear();
           
            for(int i=0;i<4; i++) //4 kolone
            {
                var colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                gridTrains.ColumnDefinitions.Add(colDef);
            }
           

            int col = 4;
            int rowNum = dto.Count() / 4; 

            if (dto.Count() < 4)
            {
                col = dto.Count(); 
                rowNum = dto.Count() / 4 + 1;
            }
            if(dto.Count() % 4 != 0)
            {
                rowNum = dto.Count() / 4 + 1;
            
             }
            var rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(1, GridUnitType.Star);

            gridTrains.RowDefinitions.Add(rowDef1);

            for (int i = 0; i < rowNum; i++) 
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(3, GridUnitType.Star);
                gridTrains.RowDefinitions.Add(rowDef);
            }
    
            StackPanel titlePanel = new StackPanel();
            titlePanel.Orientation = Orientation.Horizontal;
            titlePanel.HorizontalAlignment = HorizontalAlignment.Center;
            titlePanel.Margin= new Thickness { Bottom = 20, Left = 0, Right = 10, Top = 10 };

            Button addBtn = new Button();
            addBtn.FontSize = 20;
            addBtn.Height = 40;
            BrushConverter bc = new BrushConverter();
            addBtn.Background = (Brush)bc.ConvertFrom("#FF1E90FF");
            addBtn.Foreground = new SolidColorBrush(Colors.White);
            addBtn.Content = "Dodaj novi voz";
            addBtn.Click += add_Clicked;
         
            titlePanel.Children.Add(addBtn);
            gridTrains.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, 0);




            for (int i = 1; i < rowNum+1; i++)
            {
                if (dto.Count() - 4 * (i - 1) < 4)
                {
                    col = dto.Count() - 4 * (i-1);
                }
                for (int j = 0; j < col; j++)
                {
                    Grid grid = new Grid();
                    grid.Margin = new Thickness { Bottom = 10, Left = 10, Right = 10, Top = 10 };
                    grid.Height = 400;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    Grid.SetColumn(grid, j);
                    Grid.SetRow(grid, i);
                    
                    grid.Background = (Brush)bc.ConvertFrom("#FFF3F3F3");



                    grid.Effect = new DropShadowEffect()
                    {
                        BlurRadius = 20,
                        ShadowDepth = 1,
                        Color = (Color)ColorConverter.ConvertFromString("#FFDEDEDE")
                    };


                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Width = 200;


                    TextBlock tb = new TextBlock();
                    tb.Text = dto[(i-1)*4+j].Name;
                    tb.FontSize = 20;
                    tb.FontWeight = FontWeights.Bold;
                    tb.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    stackPanel.Children.Add(tb);

                    Image img = new Image();

                    img.Source = new BitmapImage(new Uri("/images/srbijavoz.jpg", UriKind.Relative));
                    img.Width = 200;
                    img.Height = 150;
                    stackPanel.Children.Add(img);

                    TextBlock tbColor = new TextBlock();
                    tbColor.Text = "Boja: " + dto[(i - 1) * 4 + j].Color;
                    tbColor.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    stackPanel.Children.Add(tbColor);

                    TextBlock tbRegular = new TextBlock();
                    tbRegular.Text = "Broj redova klasa REGULAR: " + dto[(i - 1) * 4 + j].numberREGULAR;
                    tbRegular.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    stackPanel.Children.Add(tbRegular);


                    TextBlock tbBusiness = new TextBlock();
                    tbBusiness.Text = "Broj redova klasa BUSINESS: " + dto[(i - 1) * 4 + j].numberBUSINESS;
                    tbBusiness.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    stackPanel.Children.Add(tbBusiness);

                    TextBlock tbVip = new TextBlock();
                    tbVip.Text = "Broj redova klasa REGULAR: " + dto[(i - 1) * 4 + j].numberVIP;
                    tbVip.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    stackPanel.Children.Add(tbVip);


                    StackPanel stackPanel2 = new StackPanel();
                    stackPanel2.Orientation = Orientation.Horizontal;
                    stackPanel2.HorizontalAlignment = HorizontalAlignment.Center;
                    Button btnEdit = new Button();
                    btnEdit.Width = 80;
                    btnEdit.Content = "Izmeni";
                    btnEdit.Tag = dto[(i - 1) * 4 + j].Id;
                    btnEdit.Click += edit_Clicked;
                    btnEdit.Foreground = new SolidColorBrush(Colors.White);
                    btnEdit.Background = (Brush)bc.ConvertFrom("#FF1E90FF");
                    btnEdit.Margin = new Thickness { Bottom = 10, Left = 0, Right = 10, Top = 10 };
                    stackPanel2.Children.Add(btnEdit);


                    Button btnDelete = new Button();
                    btnDelete.Width = 80;
                    btnDelete.Content = "Obriši";
                    btnDelete.Click += delete_Clicked;
                    btnDelete.Tag = dto[(i - 1) * 4 + j].Id;
                    btnDelete.Foreground = new SolidColorBrush(Colors.White);

                    btnDelete.Background = (Brush)bc.ConvertFrom("#FF1E90FF");
                    btnDelete.Margin = new Thickness { Bottom = 10, Left = 10, Right = 0, Top = 10 };
                    stackPanel2.Children.Add(btnDelete);
                    stackPanel.Children.Add(stackPanel2);


                    grid.Children.Add(stackPanel);
                    gridTrains.Children.Add(grid);
                    
                }
            }
        }

        private void edit_Clicked(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).Tag;
            TrainDTO trainDTO = getTrainInfo((int)id);
            Window edit = new EditTrain(trainDTO);
            edit.Show();
            dto = TrainService.getTrains();
            reloadWindow();


        }

        private void delete_Clicked(object sender, RoutedEventArgs e)
        {
            var db = new RailwayContext();
            var id = ((Button)sender).Tag;
            var train = db.trains.Where(t => t.Id == (int)id).FirstOrDefault();
            db.trains.Remove(train);
            db.SaveChanges();
            DeleteTrainModal.ShowHandlerDialog(train.Name);
            dto = TrainService.getTrains();
            reloadWindow();


        }
        private void add_Clicked(object sender, RoutedEventArgs e)
        {
            Window add = new AddTrain();
            add.Show();
            dto = TrainService.getTrains();
            reloadWindow();
        }

        private TrainDTO getTrainInfo(int id)
        {
            TrainDTO trainInfo = null;
            foreach(TrainDTO t in dto){
                if(t.Id == id)
                {
                    trainInfo = t;
                }
            }
            return trainInfo;
        }
    }
}
