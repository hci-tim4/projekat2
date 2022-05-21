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
            string username = textBoxUsername.Text;
            if (!LoginService.isExistUser(username))
            {
                MessageBox.Show("Korisnicko ime nije ispravno!");

            }
            else
            {
                string password = passwordBox1.Password;
                LoginDTO dto = new LoginDTO { username = username, password = password };
                int u = LoginService.logIn(dto);
                if (u == 2)
                {
                    MessageBox.Show("Neispravna lozinka!");
                }
                else if(u==0)
                {
                    MessageBox.Show("Ulogovan menadzer!");
                }
                else
                {
                    MessageBox.Show("Ulogovan klijent!");
                }
            }
        }
    }
}
