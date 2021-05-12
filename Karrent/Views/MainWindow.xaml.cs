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
        public MainWindow()
        {
            InitializeComponent();
            cars = DBManager.GetInstance().GetCars();
            listView.ItemsSource = cars;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnControlPanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbFilterBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbFilterModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbFilterBody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbFilterEngineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void createReservationWindow(object sender, RoutedEventArgs e)
        {

        }
    }
}
