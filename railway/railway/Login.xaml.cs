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
using System.Windows.Shapes;
using railway.dto.login_dto;
using railway.model;
using railway.services;



namespace railway
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            if (!LoginService.isExistUser(username))
            {
                CustomMessageBox cmb = new CustomMessageBox("Korisnicko ime nije ispravno!");
                cmb.ShowDialog();

            }
            else
            {
                string password = PasswordBox.Password;
                LoginDTO dto = new LoginDTO { username = username, password = password };
                User u = LoginService.logIn(dto);
                if (u == null)
                {
                    CustomMessageBox cmb = new CustomMessageBox("Neispravna lozinka!");
                    cmb.ShowDialog();
                }
                else
                {
                    if ((int)u.UserType == 0)
                    {
                        Window managerhp = new ManagerHomePage(u);
                        App.Current.MainWindow.Close();
                        managerhp.Show();
                    }
                    else if ((int)u.UserType == 1)
                    {
                        Window clienthp = new ClientHomePage(u);
                        App.Current.MainWindow.Close();
                        clienthp.Show();
                    }
                }
               
            }
        }
    }
}
