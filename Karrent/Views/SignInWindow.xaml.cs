using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Karrent.Enums;
using Karrent.Objects;

namespace Karrent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = passPassword.Password;
            UserTypes type = (UserTypes)DBManager.GetInstance().CheckUser(username, password);
            if (type == UserTypes.Guest)
                ErrorBox.Show("nie udało się zalogować");
            else
            {
                User user = DBManager.GetInstance().GetUser(username, password);
                if (user.Id == 0)
                    ErrorBox.Show("nie udało się pobrać danych użytkownika");
                else
                {
                    CurrentUser.GetInstance().SetCredentials(user);
                    this.Close();
                }
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.ShowDialog();
        }
    }
}
