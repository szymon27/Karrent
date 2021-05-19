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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Karrent.Enums;
using Karrent.Objects;

namespace Karrent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Car> cars = new List<Car>();
        List<string> Models = new List<string>();
        List<string> Brands = new List<string>();
        List<string> Bodies = new List<string>();
        List<string> EngineTypes = new List<string>();
        string[] sortingOptions = new string[] { "Default", "A-Z brand", "Z-A brand", "A-Z model", "Z-A model", "Price low-high", "Price high-low" };
        bool isSetUp = false;
        public MainWindow()
        {
            InitializeComponent();
            CurrentUser.GetInstance().MainWindow = this;
            cars = DBManager.GetInstance().GetActiveCars();
            listView.ItemsSource = cars;
            SortSetUp();
            FilterSetUp();
        }
        private void SortSetUp()
        {
            cmbSorting.ItemsSource = sortingOptions;
            cmbSorting.SelectedIndex = 0;
        }
        private void FilterSetUp()
        {
            Brands = cars.Select(c => c.CarDetails.Brand).Distinct().ToList();
            Brands.Insert(0, "Brand");
            cmbFilterBrand.ItemsSource = Brands;
            cmbFilterBrand.SelectedIndex = 0;

            Models = cars.Select(c => c.CarDetails.Model).Distinct().ToList();
            Models.Insert(0, "Model");
            cmbFilterModel.ItemsSource = Models;
            cmbFilterModel.SelectedIndex = 0;

            Bodies = cars.Select(c => c.CarDetails.BodyType.ToString("G")).Distinct().ToList();
            Bodies.Insert(0, "Body");
            cmbFilterBodyType.ItemsSource = Bodies;
            cmbFilterBodyType.SelectedIndex = 0;

            EngineTypes = cars.Select(c => c.CarDetails.EngineType.ToString("G")).Distinct().ToList();
            EngineTypes.Insert(0, "EngineType");
            cmbFilterEngineType.ItemsSource = EngineTypes;
            cmbFilterEngineType.SelectedIndex = 0;

            isSetUp = true;
        }
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.ShowDialog();
        }

        public void setButtonsAfterLogin()
        {
            if (CurrentUser.GetInstance().User.UserType != UserTypes.Guest)
            {
                if (btnSignUp.Visibility == Visibility.Visible)
                    btnSignUp.Visibility = Visibility.Hidden;
                if (btnSignIn.Content.ToString() == "SIGN IN")
                    btnSignIn.Content = "LOGOUT";
                txtCurrentUser.Content = CurrentUser.GetInstance().User.Username;
                if (CurrentUser.GetInstance().User.UserType != UserTypes.Customer)
                {
                    if (btnControlPanel.Visibility == Visibility.Hidden)
                        btnControlPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.GetInstance().User.UserType == UserTypes.Guest)
            {
                SignInWindow signInWindow = new SignInWindow();
                signInWindow.ShowDialog();
                setButtonsAfterLogin();
            }
            else
            {
                if (btnSignUp.Visibility == Visibility.Hidden)
                    btnSignUp.Visibility = Visibility.Visible;
                if (btnSignIn.Content.ToString() == "LOGOUT")
                    btnSignIn.Content = "SIGN IN";
                if (btnControlPanel.Visibility == Visibility.Visible)
                    btnControlPanel.Visibility = Visibility.Hidden;
                CurrentUser.GetInstance().SetUserAsGuest();
                txtCurrentUser.Content = CurrentUser.GetInstance().User.Username;
            }
        }
        private void btnControlPanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //"A-Z brand", "Z-A brand", "A-Z model", "Z-A model", "Price low-high", "Price high-low"
            int inx = cmbSorting.SelectedIndex;
            switch (inx)
            {
                default:
                case 0: listView.ItemsSource = cars; break;
                case 1: listView.ItemsSource = cars.OrderBy(c => c.CarDetails.Brand); break;
                case 2: listView.ItemsSource = cars.OrderByDescending(c => c.CarDetails.Brand); break;
                case 3: listView.ItemsSource = cars.OrderBy(c => c.CarDetails.Model); break;
                case 4: listView.ItemsSource = cars.OrderByDescending(c => c.CarDetails.Model); break;
                case 5: listView.ItemsSource = cars.OrderBy(c => c.CarDetails.Price); break;
                case 6: listView.ItemsSource = cars.OrderByDescending(c => c.CarDetails.Price); break;
            }
        }
        private void Filter()
        {
            if (!isSetUp) return;
            
            if (cmbFilterBrand.SelectedIndex > 0)
            {
                cars = cars.Where(c => c.CarDetails.Brand == cmbFilterBrand.SelectedItem.ToString()).ToList();
                listView.ItemsSource = cars;

                Models = cars.Where(c => c.CarDetails.Brand == cmbFilterBrand.SelectedItem.ToString()).Select(c => c.CarDetails.Model).Distinct().ToList();
                Models.Insert(0, "Model");
                cmbFilterModel.ItemsSource = Models;

                Bodies = cars.Where(c => c.CarDetails.Brand == cmbFilterBrand.SelectedItem.ToString()).Select(c => c.CarDetails.BodyType.ToString("G")).Distinct().ToList();
                Bodies.Insert(0, "Body");
                cmbFilterBodyType.ItemsSource = Bodies;

                EngineTypes = cars.Where(c => c.CarDetails.Brand == cmbFilterBrand.SelectedItem.ToString()).Select(c => c.CarDetails.EngineType.ToString("G")).Distinct().ToList();
                EngineTypes.Insert(0, "EngineType");
                cmbFilterEngineType.ItemsSource = EngineTypes;
            }
            if (cmbFilterModel.SelectedIndex > 0)
            {
                cars = cars.Where(c => c.CarDetails.Model == cmbFilterModel.SelectedItem.ToString()).ToList();
                listView.ItemsSource = cars;

                Brands = cars.Where(c => c.CarDetails.Model == cmbFilterModel.SelectedItem.ToString()).Select(c => c.CarDetails.Brand).Distinct().ToList();
                Brands.Insert(0, "Brand");
                cmbFilterBrand.ItemsSource = Brands;

                Bodies = cars.Where(c => c.CarDetails.Model == cmbFilterModel.SelectedItem.ToString()).Select(c => c.CarDetails.BodyType.ToString("G")).Distinct().ToList();
                Bodies.Insert(0, "Body");
                cmbFilterBodyType.ItemsSource = Bodies;

                EngineTypes = cars.Where(c => c.CarDetails.Model == cmbFilterModel.SelectedItem.ToString()).Select(c => c.CarDetails.EngineType.ToString("G")).Distinct().ToList();
                EngineTypes.Insert(0, "EngineType");
                cmbFilterEngineType.ItemsSource = EngineTypes;
            }
            if (cmbFilterBodyType.SelectedIndex > 0)
            {
                cars = cars.Where(c => c.CarDetails.BodyType.ToString("G") == cmbFilterBodyType.SelectedItem.ToString()).ToList();
                listView.ItemsSource = cars;

                Brands = cars.Where(c => c.CarDetails.BodyType.ToString("G") == cmbFilterBodyType.SelectedItem.ToString()).Select(c => c.CarDetails.Brand).Distinct().ToList();
                Brands.Insert(0, "Brand");
                cmbFilterBrand.ItemsSource = Brands;

                Models = cars.Where(c => c.CarDetails.BodyType.ToString("G") == cmbFilterBodyType.SelectedItem.ToString()).Select(c => c.CarDetails.Model).Distinct().ToList();
                Models.Insert(0, "Model");
                cmbFilterModel.ItemsSource = Models;

                EngineTypes = cars.Where(c => c.CarDetails.BodyType.ToString("G") == cmbFilterBodyType.SelectedItem.ToString()).Select(c => c.CarDetails.EngineType.ToString("G")).Distinct().ToList();
                EngineTypes.Insert(0, "EngineType");
                cmbFilterEngineType.ItemsSource = EngineTypes;
            }
            if (cmbFilterEngineType.SelectedIndex > 0)
            {
                cars = cars.Where(c => c.CarDetails.EngineType.ToString("G") == cmbFilterEngineType.SelectedItem.ToString()).ToList();
                listView.ItemsSource = cars;

                Brands = cars.Where(c => c.CarDetails.EngineType.ToString("G") == cmbFilterEngineType.SelectedItem.ToString()).Select(c => c.CarDetails.Brand).Distinct().ToList();
                Brands.Insert(0, "Brand");
                cmbFilterBrand.ItemsSource = Brands;

                Models = cars.Where(c => c.CarDetails.EngineType.ToString("G") == cmbFilterEngineType.SelectedItem.ToString()).Select(c => c.CarDetails.Model).Distinct().ToList();
                Models.Insert(0, "Model");
                cmbFilterModel.ItemsSource = Models;

                Bodies = cars.Where(c => c.CarDetails.EngineType.ToString("G") == cmbFilterEngineType.SelectedItem.ToString()).Select(c => c.CarDetails.BodyType.ToString("G")).Distinct().ToList();
                Bodies.Insert(0, "Body");
                cmbFilterBodyType.ItemsSource = Bodies;
            }
        }
        
        private void cmbFilterBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbFilterModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbFilterBodyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbFilterEngineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cars = DBManager.GetInstance().GetActiveCars();
            Filter();
            listView.ItemsSource = cars;
        }
        private void btnRemoveFilters_Click(object sender, RoutedEventArgs e)
        {
            cars = DBManager.GetInstance().GetActiveCars();
            listView.ItemsSource = cars;
            isSetUp = false;
            FilterSetUp();
        }

        private void createReservationWindow(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((Button)sender).Tag.ToString());
            ReservationWindow reservationWindow = new ReservationWindow(id);
            reservationWindow.ShowDialog();
        }

        
    }
}
