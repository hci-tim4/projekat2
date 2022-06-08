using railway.dto.trains;
using railway.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace railway
{
    /// <summary>
    /// Interaction logic for CRUDTrains.xaml
    /// </summary>
    public partial class CRUDTrains : Page
    {
        List<TrainDTO> dto;

       

        public CRUDTrains()
        {
            InitializeComponent();
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
            var rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(1, GridUnitType.Star);
            gridTrains.RowDefinitions.Add(rowDef1);
            var rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength(5, GridUnitType.Star);
            gridTrains.RowDefinitions.Add(rowDef2);

            StackPanel titlePanel = new StackPanel();
            titlePanel.Orientation = Orientation.Horizontal;
            titlePanel.HorizontalAlignment = HorizontalAlignment.Center;
            Button addBtn = new Button();
            addBtn.Content = "Dodaj novi voz";
           
            titlePanel.Children.Add(addBtn);
            gridTrains.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, 0);
            
            dto = TrainService.getTrains();
            foreach (TrainDTO t in dto)
             {
                 var colDef = new ColumnDefinition();
                 colDef.Width = new GridLength(1, GridUnitType.Star);
                 gridTrains.ColumnDefinitions.Add(colDef);
             }

            for (int i = 0; i < dto.Count; i++)
            {
                Grid grid = new Grid();
                grid.Margin = new Thickness { Bottom = 10, Left = 10, Right = 10, Top = 10 };
                Grid.SetColumn(grid, i);
                Grid.SetRow(grid, 1);
                BrushConverter bc = new BrushConverter();
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
                 tb.Text = dto[i].Name;
                 tb.FontSize = 20;
                 tb.FontWeight = FontWeights.Bold;
                 stackPanel.Children.Add(tb);

                Image img = new Image();

                img.Source = new BitmapImage(new Uri("/images/srbijavoz.jpg", UriKind.Relative));
                img.Width = 200;
                img.Height = 150;
                stackPanel.Children.Add(img);

                TextBlock tbColor = new TextBlock();
                tbColor.Text = "Boja: " + dto[i].Color;
                stackPanel.Children.Add(tbColor);

                TextBlock tbRegular = new TextBlock();
                tbRegular.Text = "Broj sedišta REGULAR: " + dto[i].numberREGULAR;
                stackPanel.Children.Add(tbRegular);


                TextBlock tbBusiness = new TextBlock();
                tbBusiness.Text = "Broj sedišta BUSINESS: " + dto[i].numberBUSINESS;
                stackPanel.Children.Add(tbBusiness);

                TextBlock tbVip = new TextBlock();
                tbVip.Text = "Broj sedišta REGULAR: " + dto[i].numberVIP;
                stackPanel.Children.Add(tbVip);
                

                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Orientation = Orientation.Horizontal;
                stackPanel2.HorizontalAlignment = HorizontalAlignment.Center;
                Button btnEdit = new Button();
                btnEdit.Width = 80;
                btnEdit.Content = "Izmeni";
                btnEdit.Margin = new Thickness { Bottom = 10, Left = 0, Right = 10, Top = 10 };
                stackPanel2.Children.Add(btnEdit);

                Button btnDelete = new Button();
                btnDelete.Width = 80;
                btnDelete.Content = "Obriši";
                btnDelete.Margin = new Thickness { Bottom = 10, Left = 10, Right = 0, Top = 10 };
                stackPanel2.Children.Add(btnDelete);
                stackPanel.Children.Add(stackPanel2);


                grid.Children.Add(stackPanel);
                gridTrains.Children.Add(grid);
            }
           
        }
        }
}
