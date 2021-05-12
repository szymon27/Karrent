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

namespace Karrent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void btnSingUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = passPassword.Password;
            string confirmPassword = passConfirmPassword.Password;
            string name = txtName.Text;
            string surname = txtSurname.Text;
            DateTime? birthDate = dateBrthDate.SelectedDate;

            if (String.IsNullOrEmpty(username) || username.Length < 4)
            {
                ErrorBox.Show("zła nazwa użytkownika");
                return;
            }

            if (String.IsNullOrEmpty(password) || password.Length < 4)
            {
                ErrorBox.Show("złe hasło");
                return;
            }

            if (password != confirmPassword)
            {
                ErrorBox.Show("hasła nie pasują do siebie");
                return;
            }

            if (String.IsNullOrEmpty(name) || name.Length < 2)
            {
                ErrorBox.Show("złe imie");
                return;
            }

            if (String.IsNullOrEmpty(surname) || surname.Length < 2)
            {
                ErrorBox.Show("złe nazwisko");
                return;
            }

            if (birthDate == null)
            {
                ErrorBox.Show("zła data urodzenia");
                return;
            }

            if (DBManager.GetInstance().AddUser(username, password, name, surname, birthDate.GetValueOrDefault().ToString("yyyy-MM-dd")))
            {
                this.Close();
            }
            else
                ErrorBox.Show("nie udało się utworzyć konta");
        }
    }
}
