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
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Models;
using ThinkSharp.FeatureTouring.Navigation;

namespace railway.CRUDTrain
{
    /// <summary>
    /// Interaction logic for CRUDTrains.xaml
    /// </summary>
    public partial class CRUDTrains : Page, TutorialInterface
    {
        public static List<TrainDTO> dto;
        Frame page;
        private Boolean Touring;
     



        public CRUDTrains()
        {
            try{
                InitializeComponent();
               
                dto = TrainService.getTrains();
                
            }
            catch (Exception e)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
     
            //loggedUser = user;



            // this.DataContext = dto;
            //dataGrid.ItemsSource = dto;
        }

    
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try{
                reloadWindow();
           
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        public void reloadWindow()
        {
            gridTrains.RowDefinitions.Clear();
            gridTrains.ColumnDefinitions.Clear();
            gridTrains.Children.Clear();

            TourHelper.SetElementID(gridTrains, "allTrains");
            TourHelper.SetPlacement(gridTrains, Placement.TopCenter);
           
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
            addBtn.CommandParameter = "btnAddTrain";
       
            BrushConverter bc = new BrushConverter();
            addBtn.Background = (Brush)bc.ConvertFrom("#FF1E90FF");
            addBtn.Foreground = new SolidColorBrush(Colors.White);
            addBtn.Content = "Dodaj novi voz";
            TourHelper.SetElementID(addBtn, "addTrain");
            TourHelper.SetPlacement(addBtn, Placement.RightCenter);
            
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
                    grid.Margin = new Thickness { Bottom = 0, Left =10, Right = 10, Top = 10 };
                    grid.Height = 250;
                    grid.Width = 250;
                    grid.VerticalAlignment = VerticalAlignment.Top;
                    Grid.SetColumn(grid, j);
                    Grid.SetRow(grid, i);
                    
                    grid.Background = (Brush)bc.ConvertFrom("#FFF3F3F3");



                    grid.Effect = new DropShadowEffect()
                    {
                        BlurRadius = 10,
                        ShadowDepth = 1,
                        Color = (Color)ColorConverter.ConvertFromString("#FFDEDEDE")
                    };


                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Width = 300;

                    StackPanel stackPanelName = new StackPanel();
                    stackPanelName.Orientation = Orientation.Horizontal;
              
                    stackPanelName.Margin = new Thickness { Bottom = 20, Left = 0, Right = 0, Top = 10 };
                    Image img = new Image();
                    img.Width = 50;
                    img.Height = 40;
                    img.Source = new BitmapImage(new Uri("/icon/trainIcon.png", UriKind.Relative));
                    stackPanelName.Children.Add(img);

                    TextBlock tb = new TextBlock();
                    tb.Text = dto[(i-1)*4+j].Name;
                    tb.FontSize = 30;
                    tb.Margin = new Thickness { Bottom = 0, Left = 10, Right = 0, Top = 0 };
                    tb.FontWeight = FontWeights.Bold;
                    tb.Width = 280;
                    tb.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    stackPanelName.Children.Add(tb);
                    stackPanel.Children.Add(stackPanelName);
                    
                    TextBlock tbColor = new TextBlock();
                    tbColor.Text = "Boja: " + dto[(i - 1) * 4 + j].Color;
                    tbColor.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    tbColor.Width = 280;
                    tbColor.FontSize = 15;
                    
                    stackPanel.Children.Add(tbColor);

                    TextBlock tbCol = new TextBlock();
                    tbCol.Text = "Broj kolona: " + dto[(i - 1) * 4 + j].col;
                    tbCol.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    tbCol.FontSize = 15;
                    tbCol.Width = 280;
                    stackPanel.Children.Add(tbCol);

                    TextBlock tbRegular = new TextBlock();

                    tbRegular.Text = "Broj redova regularne klase: " + dto[(i - 1) * 4 + j].numberREGULAR;

                    tbRegular.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    tbRegular.Width = 280;
                    tbRegular.FontSize = 15;
                    stackPanel.Children.Add(tbRegular);
                     

                    TextBlock tbBusiness = new TextBlock();

                    tbBusiness.Text = "Broj redova biznis klase: " + dto[(i - 1) * 4 + j].numberBUSINESS;

                    tbBusiness.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    tbBusiness.FontSize = 15;
                    tbBusiness.Width = 280;
                    stackPanel.Children.Add(tbBusiness);

                    TextBlock tbVip = new TextBlock();
                    tbVip.Text = "Broj redova vip klase: " + dto[(i - 1) * 4 + j].numberVIP;

                    tbVip.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    tbVip.FontSize = 15;
                    tbVip.Width = 280;
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
                    btnEdit.Margin = new Thickness { Bottom = 10, Left = 0, Right = 10, Top = 15 };
                    btnEdit.CommandParameter = "btnEditTrain";
                    stackPanel2.Children.Add(btnEdit);


                    Button btnDelete = new Button();
                    btnDelete.Width = 80;
                    btnDelete.Content = "Obriši";
                    btnDelete.Click += delete_Clicked;
                    btnDelete.Tag = dto[(i - 1) * 4 + j].Id;
                    btnDelete.Foreground = new SolidColorBrush(Colors.White);

                    btnDelete.Background = (Brush)bc.ConvertFrom("#FF1E90FF");
                    btnDelete.Margin = new Thickness { Bottom = 10, Left = 0, Right = 25, Top = 15 };
                    btnDelete.CommandParameter = "btnDeleteTrain";
                    stackPanel2.Children.Add(btnDelete);
                    stackPanel.Children.Add(stackPanel2);


                    grid.Children.Add(stackPanel);
                    gridTrains.Children.Add(grid);
                    
                }
            }
        }

        private void edit_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var id = ((Button)sender).Tag;
                TrainDTO trainDTO = getTrainInfo((int)id);
                Window edit = new EditTrain(trainDTO);
                edit.ShowDialog();

                if (!edit.IsActive)
                {
                    reloadWindow();
                }
            
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
        }

        private void delete_Clicked(object sender, RoutedEventArgs e)
        {
            try{
                
                ConfirmDelete conf = new ConfirmDelete();
                conf.ShowDialog();
                if (!conf.delete)
                {
                    return;
                }

                var db = new RailwayContext();
                var id = ((Button)sender).Tag;
                var train = db.trains.Where(t => t.Id == (int)id).FirstOrDefault();
                train.Deleted = true;
                db.SaveChanges();
                dto = TrainService.getTrains();
                Window messageBox = new CustomMessageBox("Voz " + train.Name + " je obrisan!");
                messageBox.ShowDialog();
                reloadWindow();

            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }
           
           

        }
        private void add_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Window add = new AddTrain();
                add.ShowDialog();
                if (!add.IsActive)
                {
                    reloadWindow();
                }
            
            }
            catch (Exception ex)
            {
                CustomMessageBox cmb = new CustomMessageBox("Nešto je pošlo po zlu.\nPokušajte ponovo.");
                cmb.ShowDialog();
            }

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
            


                Steps = new[]
                {
                    new Step("allTrains", "Prikaz svih vozova", "Prikazani su svi postojeći vozovi i informacije o njima, sa opcijama brisanja i izmene."),
                    new Step("addTrain", "Dodavanje novog voza", "Klikom na dugme otvoriće se forma za dodavanje novog voza."),
                    
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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);

                if (focusedControl is Button)
                {
                    Button b = (Button)focusedControl;
                if (b.CommandParameter.Equals("btnAddTrain"))
                {
                    HelpProvider.ShowHelp("btnAddTrain", this);
                }
                else if (b.CommandParameter.Equals("btnEditTrain"))
                {
                    HelpProvider.ShowHelp("btnEditTrain", this);
                }
                else if (b.CommandParameter.Equals("btnDeleteTrain")) {
                    HelpProvider.ShowHelp("btnDeleteTrain", this);
                }
            }
                
            
        }
    }
}
