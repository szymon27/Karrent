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

        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
