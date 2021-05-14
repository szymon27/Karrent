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
using Karrent.Objects;
using Karrent.Enums;

namespace Karrent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ReservationWindow.xaml
    /// </summary>
    public partial class ReservationWindow : Window
    {
        private Car car;
        private List<ReservationPeriod> reservationPeriods = new List<ReservationPeriod>();
        private List<SecurityPackage> securityPackages = new List<SecurityPackage>();

        public ReservationWindow()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            dpckBegin.BlackoutDates.AddDatesInPast();
            dpckEnd.BlackoutDates.AddDatesInPast();
        }

        public ReservationWindow(int id) : this()
        {
            car = DBManager.GetInstance().GetCarById(id);
            reservationPeriods = DBManager.GetInstance().GetDatesOfCar(id);
            securityPackages = DBManager.GetInstance().GetSecurityPackages();
            if (car != null)
            {
                txtCarDetails.Text = car.ToString();
                imgCar.Source = car.CarDetails.Photo;
            }
            foreach (ReservationPeriod e in reservationPeriods)
                lbxReservationDates.Items.Add(e.ToString());

            foreach (ReservationPeriod element in reservationPeriods)
            {
                CalendarDateRange calendarDateRange = new CalendarDateRange(element.Begin.GetValueOrDefault(), element.End.GetValueOrDefault());
                dpckBegin.BlackoutDates.Add(calendarDateRange);
                dpckEnd.BlackoutDates.Add(calendarDateRange);
            }
            if (securityPackages.Count == 3)
            {
                rbtn1.Content = securityPackages.ElementAt(0);
                rbtn1.IsChecked = true;
                rbtn2.Content = securityPackages.ElementAt(1);
                rbtn3.Content = securityPackages.ElementAt(2);
            }
        }

        private decimal GetPrice()
        {
            SecurityPackage securityPackage = securityPackages.ElementAt(0);
            if (rbtn1.IsChecked == true) securityPackage = securityPackages.ElementAt(0);
            if (rbtn2.IsChecked == true) securityPackage = securityPackages.ElementAt(1);
            if (rbtn3.IsChecked == true) securityPackage = securityPackages.ElementAt(2);
            DateTime? datebegin = dpckBegin.SelectedDate.GetValueOrDefault();
            DateTime? dateend = dpckEnd.SelectedDate.GetValueOrDefault();
            TimeSpan? timeSpan = dateend - datebegin;
            int days = timeSpan.GetValueOrDefault().Days + 1;
            return Convert.ToDecimal(days) * car.CarDetails.Price + securityPackage.Price;
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dpckBegin.SelectedDate == null)
            {
                ErrorBox.Show("wybierz datę początku rezerwacji");
                return;
            }
            if (dpckEnd.SelectedDate == null)
            {
                ErrorBox.Show("wybierz datę końca rezerwacji");
                return;
            }
            if (dpckBegin.SelectedDate > dpckEnd.SelectedDate)
            {
                ErrorBox.Show("data początku > data końca");
                return;
            }

            DateTime? datebegin = dpckBegin.SelectedDate.GetValueOrDefault();
            DateTime? dateend = dpckEnd.SelectedDate.GetValueOrDefault();
            if (!ReservationPeriod.CheckReservation(new ReservationPeriod(datebegin, dateend), reservationPeriods))
            {
                ErrorBox.Show("zły przedział daty");
                return;
            }

            if (UserTypes.Guest == CurrentUser.GetInstance().User.UserType)
            {
                SignInWindow signInWindow = new SignInWindow();
                signInWindow.ShowDialog();

                if(UserTypes.Guest == CurrentUser.GetInstance().User.UserType)
                    return;
            }

            int securityPackageId = securityPackages.ElementAt(0).Id;
            if (rbtn1.IsChecked == true) securityPackageId = securityPackages.ElementAt(0).Id;
            if (rbtn2.IsChecked == true) securityPackageId = securityPackages.ElementAt(1).Id;
            if (rbtn3.IsChecked == true) securityPackageId = securityPackages.ElementAt(2).Id;

            decimal price = GetPrice();
            if (DBManager.GetInstance().AddReservation(car.Id, securityPackageId, new ReservationPeriod(datebegin, dateend), price))
                InfoBox.Show(price.ToString());
        }
    }
}
