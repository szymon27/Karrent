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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();
            txtName.Text = CurrentUser.GetInstance().User.Name;
            txtSurname.Text = CurrentUser.GetInstance().User.Surname;
            dateBirthDate.SelectedDate = CurrentUser.GetInstance().User.BirthDate;
            var rentedCarsList = DBManager.GetInstance().GetReservations();
            lstRentedCars.Items.Clear();
            foreach (var element in rentedCarsList)
                lstRentedCars.Items.Add($"{element.Item1} {element.Item2} {element.Item3.ToString("yyyy-MM-dd")}" +
                    $" {element.Item4.ToString("yyyy-MM-dd")} {element.Item5.ToString()}");
        }
        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string current = passCurrPassword.Password;
            string newpass = passNewPassword.Password;
            string confpass = passConfirmNewPassword.Password;
            if(string.IsNullOrEmpty(current))
            {
                ErrorBox.Show("Password not entered");
                return;
            }
            if (string.IsNullOrEmpty(newpass))
            {
                ErrorBox.Show("New password not entered");
                return;
            }
            if (string.IsNullOrEmpty(confpass))
            {
                ErrorBox.Show("Confirm password not entered");
                return;
            }
            if(current != CurrentUser.GetInstance().User.Password)
            {
                ErrorBox.Show("Current password doesn't match");
                return;
            }
            if (newpass != confpass)
            {
                ErrorBox.Show("Passwords doesn't match");
                return;
            }
            if(current == newpass)
            {
                ErrorBox.Show("New password is same as old");
                return;
            }

            if (DBManager.GetInstance().ChangePassword(newpass))
            {
                InfoBox.Show("Password changed");
                CurrentUser.GetInstance().User.Password = newpass;
            }
            else
                ErrorBox.Show("Password not changed");
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string surname = txtSurname.Text;
            DateTime? birthDate = dateBirthDate.SelectedDate;
            
            if(string.IsNullOrEmpty(name) || name.Length < 2)
            {
                ErrorBox.Show("Wrong name");
                return;
            }

            if (string.IsNullOrEmpty(surname) || surname.Length < 2)
            {
                ErrorBox.Show("Wrong surname");
                return;
            }

            if(birthDate == null)
            {
                ErrorBox.Show("Choose birth date");
                return;
            }

            if (DBManager.GetInstance().ChangePersonalDetails(name, surname, birthDate.GetValueOrDefault().ToString("yyyy-MM-dd")))
                InfoBox.Show("Personal details changed");
            else
                ErrorBox.Show("Personal details not changed");
        }
    }
}
