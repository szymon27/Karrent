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
using Microsoft.Win32;

namespace Karrent.Views
{
    /// <summary>
    /// Interaction logic for ControlPanelWindow.xaml
    /// </summary>
    public partial class ControlPanelWindow : Window
    {
        private User User = null;
        private string pathToImgModel = String.Empty;
        private CarDetails Model = null;
        private string pathToImgModelE = String.Empty;

        public ControlPanelWindow()
        {
            InitializeComponent();
            foreach (string bodyType in Enum.GetNames(typeof(BodyTypes))) 
            {
                cbxBodyTypes.Items.Add(bodyType);
                cbxBodyTypesE.Items.Add(bodyType);
            }
            foreach (string engineType in Enum.GetNames(typeof(EngineTypes)))
            {
                cbxEngineTypes.Items.Add(engineType);
                cbxEngineTypesE.Items.Add(engineType);
            }

            HideAllStackPanels();
        }

        private void HideAllStackPanels()
        {
            stackPanelAddUser.Visibility = Visibility.Hidden;
            stackPanelEditUser.Visibility = Visibility.Hidden;
            stackPanelAddModel.Visibility = Visibility.Hidden;
            stackPanelEditModel.Visibility = Visibility.Hidden;
            stackPanelAddCar.Visibility = Visibility.Hidden;
            stackPanelEditCar.Visibility = Visibility.Hidden;
            stackPanelRaports.Visibility = Visibility.Hidden;

            AddUserClear();
            EditUserClear();
            AddModelClear();
            EditModelClear();
        }

        private void AddUserClear()
        {
            txtUsername.Text = String.Empty;
            passPassword.Password = String.Empty;
            passConfirmPassword.Password = String.Empty;
            txtName.Text = String.Empty;
            txtSurname.Text = String.Empty;
            dateBrthDate.SelectedDate = null;
            rbtnCustomer.IsChecked = false;
            rbtnEmployee.IsChecked = false;
            rbtnManager.IsChecked = false;
        }

        private void EditUserClear()
        {
            this.User = null;
            txtUsernameE.Text = String.Empty;
            passPasswordE.Password = String.Empty;
            passConfirmPasswordE.Password = String.Empty;
            txtNameE.Text = String.Empty;
            txtSurnameE.Text = String.Empty;
            dateBrthDateE.SelectedDate = null;
            rbtnCustomerE.IsChecked = false;
            rbtnEmployeeE.IsChecked = false;
            rbtnManagerE.IsChecked = false;
            rbtnActiveE.IsChecked = false;
            rbtnInActiveE.IsChecked = false;
            lstUsers.ItemsSource = DBManager.GetInstance().GetUsers();
        }

        private void AddModelClear()
        {
            pathToImgModel = String.Empty;
            txtBrand.Text = String.Empty;
            txtModel.Text = String.Empty;
            txtHorsePower.Text = String.Empty;
            txtPrice.Text = String.Empty;
            cbxBodyTypes.SelectedIndex = -1;
            cbxEngineTypes.SelectedIndex = -1;
            imgModel.Source = null;
            lblImgModelPath.Content = String.Empty;
        }

        private void EditModelClear()
        {
            pathToImgModelE = String.Empty;
            txtBrandE.Text = String.Empty;
            txtModelE.Text = String.Empty;
            txtHorsePowerE.Text = String.Empty;
            txtPriceE.Text = String.Empty;
            cbxBodyTypesE.SelectedIndex = -1;
            cbxEngineTypesE.SelectedIndex = -1;
            imgModelE.Source = null;
            lblImgModelPathE.Content = String.Empty;
            lstModels.ItemsSource = DBManager.GetInstance().GetModels();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelAddUser.Visibility = Visibility.Visible;
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelEditUser.Visibility = Visibility.Visible;
        }

        private void btnAddModel_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelAddModel.Visibility = Visibility.Visible;
        }

        private void btnEditModel_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelEditModel.Visibility = Visibility.Visible;
        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelAddCar.Visibility = Visibility.Visible;
        }

        private void btnEditCar_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelEditCar.Visibility = Visibility.Visible;
        }

        private void btnRaports_Click(object sender, RoutedEventArgs e)
        {
            HideAllStackPanels();
            stackPanelRaports.Visibility = Visibility.Visible;
        }

        private void btnAddUserDone_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = passPassword.Password;
            string confirmPassword = passConfirmPassword.Password;
            string name = txtName.Text;
            string surname = txtSurname.Text;
            DateTime? birthDate = dateBrthDate.SelectedDate;

            if (String.IsNullOrEmpty(username) || username.Length < 4)
            {
                ErrorBox.Show("Wrong username");
                return;
            }

            if (String.IsNullOrEmpty(password) || password.Length < 4)
            {
                ErrorBox.Show("Wrong password");
                return;
            }

            if (password != confirmPassword)
            {
                ErrorBox.Show("Passwords don't match");
                return;
            }

            if (String.IsNullOrEmpty(name) || name.Length < 2)
            {
                ErrorBox.Show("Wrong name");
                return;
            }

            if (String.IsNullOrEmpty(surname) || surname.Length < 2)
            {
                ErrorBox.Show("Wrong surname");
                return;
            }

            if (birthDate == null)
            {
                ErrorBox.Show("Wrong birth date");
                return;
            }

            UserTypes userType = UserTypes.Customer;
            if (rbtnCustomer.IsChecked == true) userType = UserTypes.Customer;
            if (rbtnEmployee.IsChecked == true) userType = UserTypes.Employee;
            if (rbtnManager.IsChecked == true) userType = UserTypes.Manager;

            if (DBManager.GetInstance().AddUser(username, password, name, surname, birthDate.GetValueOrDefault().ToString("yyyy-MM-dd"), userType))
            {
                InfoBox.Show($"Created {username}");
                AddUserClear();
            }
            else
                ErrorBox.Show("Couldn't create user");
        }

        private void btnEditUserDone_Click(object sender, RoutedEventArgs e)
        {
            if (this.User == null)
                return;

            string username = txtUsernameE.Text;
            string password = passPasswordE.Password;
            string confirmPassword = passConfirmPasswordE.Password;
            string name = txtNameE.Text;
            string surname = txtSurnameE.Text;
            DateTime? birthDate = dateBrthDateE.SelectedDate;
            UserTypes userType = UserTypes.Customer;
            bool active = true;

            if (String.IsNullOrEmpty(username) || username.Length < 4)
            {
                ErrorBox.Show("Wrong username");
                return;
            }

            if (username != this.User.Username)
            {
                List<User> users = DBManager.GetInstance().GetUsers();
                foreach (User user in users)
                    if (user.Username == username)
                    {
                        ErrorBox.Show("Username already in use");
                        return;
                    }
            }

            if (String.IsNullOrEmpty(name) || name.Length < 2)
            {
                ErrorBox.Show("Wrong name");
                return;
            }

            if (String.IsNullOrEmpty(surname) || surname.Length < 2)
            {
                ErrorBox.Show("Wrong surname");
                return;
            }

            if (birthDate == null)
            {
                ErrorBox.Show("Wrong birth date");
                return;
            }

            if (rbtnCustomerE.IsChecked == true) userType = UserTypes.Customer;
            if (rbtnEmployeeE.IsChecked == true) userType = UserTypes.Employee;
            if (rbtnManagerE.IsChecked == true) userType = UserTypes.Manager;
            if (rbtnActiveE.IsChecked == true) active = true;
            else active = false;

            if (String.IsNullOrEmpty(password) && String.IsNullOrEmpty(confirmPassword))
                password = this.User.Password;
            else
                if (password == confirmPassword && password.Length >= 4) ;
                else
                {
                    ErrorBox.Show("Wrong password");
                    return;
                }

            if (DBManager.GetInstance().UpdateUser(this.User.Id, userType, username, password, name,
                surname, birthDate.GetValueOrDefault().ToString("yyyy-MM-dd"), active))
            {
                InfoBox.Show("Changes saved");
                lstUsers.ItemsSource = DBManager.GetInstance().GetUsers();
                EditUserClear();
            }
            else
                ErrorBox.Show("Changes not saved");
        }

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstUsers.SelectedIndex;
            if (index <= -1 || index >= lstUsers.Items.Count)
            {
                EditUserClear();
                return;
            }
            this.User = (User)lstUsers.Items.GetItemAt(index);
            txtUsernameE.Text = this.User.Username;
            txtNameE.Text = this.User.Name;
            txtSurnameE.Text = this.User.Surname;
            dateBrthDateE.SelectedDate = this.User.BirthDate;
            UserTypes userType = this.User.UserType;
            if (userType == UserTypes.Customer) rbtnCustomerE.IsChecked = true;
            if (userType == UserTypes.Employee) rbtnEmployeeE.IsChecked = true;
            if (userType == UserTypes.Manager) rbtnManagerE.IsChecked = true;
            bool isActive = this.User.IsActive;
            if (isActive) rbtnActiveE.IsChecked = true;
            else rbtnInActiveE.IsChecked = true;
        }

        private void btnAddModelDone_Click(object sender, RoutedEventArgs e)
        {
            string brand = txtBrand.Text;
            string model = txtModel.Text;
            int horsePower;
            decimal price;

            if (String.IsNullOrEmpty(brand) || brand.Length < 2)
            {
                ErrorBox.Show("Wrong Brand");
                return;
            }

            if (String.IsNullOrEmpty(model))
            {
                ErrorBox.Show("Wrong model");
                return;
            }

            if (!(Int32.TryParse(txtHorsePower.Text, out horsePower) && horsePower > 0))
            {
                ErrorBox.Show("Wrong horse power");
                return;
            }

            if (!(Decimal.TryParse(txtPrice.Text, out price) && price > 0))
            {
                ErrorBox.Show("Wrong price");
                return;
            }

            if(String.IsNullOrEmpty(this.pathToImgModel))
            {
                ErrorBox.Show("Choose image");
                return;
            }

            int bodyTypeIndex = cbxBodyTypes.SelectedIndex;
            if(bodyTypeIndex <= -1 || bodyTypeIndex >= cbxBodyTypes.Items.Count)
            {
                ErrorBox.Show("Choose body type");
                return;
            }

            int engineTypeIndex = cbxEngineTypes.SelectedIndex;
            if(engineTypeIndex <= -1 || engineTypeIndex >= cbxEngineTypes.Items.Count)
            {
                ErrorBox.Show("Choose engine type");
                return;
            }

            if (DBManager.GetInstance().AddModel((BodyTypes)(bodyTypeIndex + 1), (EngineTypes)(engineTypeIndex + 1), brand, model, horsePower, price, pathToImgModel))
            {
                InfoBox.Show($"Created model");
                AddModelClear();
            }
            else
                ErrorBox.Show("Couldn't create model");

        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                pathToImgModel = openFileDialog.FileName;
                lblImgModelPath.Content = pathToImgModel;
                imgModel.Source = new BitmapImage(new Uri(pathToImgModel));
            }
        }

        private void btnRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(pathToImgModel)) ;
            else
            {
                pathToImgModel = String.Empty;
                lblImgModelPath.Content = pathToImgModel;
                imgModel.Source = null;
            }
        }

        private void btnRemoveImageE_Click(object sender, RoutedEventArgs e)
        {
            if (imgModelE.Source == null)
                return;
            else
            {
                pathToImgModelE = String.Empty;
                lblImgModelPathE.Content = pathToImgModelE;
                imgModelE.Source = null;
            }
        }

        private void btnAddImageE_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                pathToImgModelE = openFileDialog.FileName;
                lblImgModelPathE.Content = pathToImgModelE;
                imgModelE.Source = new BitmapImage(new Uri(pathToImgModelE));
            }
        }

        private void lstModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstModels.SelectedIndex;
            if (index <= -1 || index >= lstModels.Items.Count)
            {
                EditModelClear();
                return;
            }
            this.Model = (CarDetails)lstModels.Items.GetItemAt(index);
            txtBrandE.Text = this.Model.Brand;
            txtModelE.Text = this.Model.Model;
            txtHorsePowerE.Text = this.Model.HorsePower.ToString();
            txtPriceE.Text = this.Model.Price.ToString();
            cbxBodyTypesE.SelectedIndex = ((int)this.Model.BodyType) - 1;
            cbxEngineTypesE.SelectedIndex = ((int)this.Model.EngineType) - 1;
            imgModelE.Source = this.Model.Photo;
        }

        private void btnEditModelDone_Click(object sender, RoutedEventArgs e)
        {
            string brand = txtBrandE.Text;
            string model = txtModelE.Text;
            int horsePower;
            decimal price;

            if (String.IsNullOrEmpty(brand) || brand.Length < 2)
            {
                ErrorBox.Show("Wrong Brand");
                return;
            }

            if (String.IsNullOrEmpty(model))
            {
                ErrorBox.Show("Wrong model");
                return;
            }

            if (!(Int32.TryParse(txtHorsePowerE.Text, out horsePower) && horsePower > 0))
            {
                ErrorBox.Show("Wrong horse power");
                return;
            }

            if (!(Decimal.TryParse(txtPriceE.Text, out price) && price > 0))
            {
                ErrorBox.Show("Wrong price");
                return;
            }

            //if (String.IsNullOrEmpty(this.pathToImgModelE))
            if(imgModelE.Source == null)
            {
                ErrorBox.Show("Choose image");
                return;
            }

            int bodyTypeIndex = cbxBodyTypesE.SelectedIndex;
            if (bodyTypeIndex <= -1 || bodyTypeIndex >= cbxBodyTypesE.Items.Count)
            {
                ErrorBox.Show("Choose body type");
                return;
            }

            int engineTypeIndex = cbxEngineTypesE.SelectedIndex;
            if (engineTypeIndex <= -1 || engineTypeIndex >= cbxEngineTypesE.Items.Count)
            {
                ErrorBox.Show("Choose engine type");
                return;
            }

            if (DBManager.GetInstance().UpdateModel(this.Model.Id, (BodyTypes)(bodyTypeIndex + 1), (EngineTypes)(engineTypeIndex + 1), brand, model, horsePower, price, pathToImgModelE))
            {
                InfoBox.Show("Changes saved");
                EditModelClear();
            }
            else
                ErrorBox.Show("Changes not saved");
        }
    }
}
