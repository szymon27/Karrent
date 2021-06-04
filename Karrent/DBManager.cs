using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using Karrent.Objects;
using Karrent.Enums;
using System.Linq;

namespace Karrent
{
    class DBManager
    {
        private string connectionString = "datasource=localhost; port=3306; username=root; database=karrent";
        private MySqlConnection mySqlConnection;
        protected static DBManager _instance;

        protected DBManager()
        {
            try
            {
                mySqlConnection = new MySqlConnection(this.connectionString);
                mySqlConnection.Open();
            }
            catch
            {
                ErrorBox.Show("błąd połączenia z bazą");
            }
        }

        public static DBManager GetInstance()
        {
            if (_instance == null)
                _instance = new DBManager();
            return _instance;
        }

        public List<Car> GetCars()
        {
            List<Car> list = new List<Car>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getCars();", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    int id = mySqlDataReader.GetInt32(0);
                    string plateNumber = mySqlDataReader.GetString(1);
                    double mileage = mySqlDataReader.GetDouble(2);
                    bool isActive = mySqlDataReader.GetBoolean(3);
                    DateTime inspectionDate = mySqlDataReader.GetDateTime(4);
                    int idCarDetails = mySqlDataReader.GetInt32(5);
                    BodyTypes bodyType = (BodyTypes)mySqlDataReader.GetInt32(6);
                    EngineTypes engineType = (EngineTypes)mySqlDataReader.GetInt32(7);
                    string brand = mySqlDataReader.GetString(8);
                    string model = mySqlDataReader.GetString(9);
                    int horsePower = mySqlDataReader.GetInt32(10);
                    decimal price = mySqlDataReader.GetDecimal(11);
                    byte[] photo = (byte[])mySqlDataReader[12];
                    CarDetails carDetails = new CarDetails(idCarDetails, bodyType, engineType, brand, model, horsePower, price, photo.ToBitmapImage());
                    list.Add(new Car(id, carDetails, plateNumber, mileage, isActive, inspectionDate));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<Car> GetActiveCars()
        {
            return GetCars().Where(car => car.IsActive == true && car.InspectionDate > DateTime.Now).ToList();
        }

        public UserTypes CheckUser(string username, string password)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call checkUser(\"{username}\", \"{password}\");", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                UserTypes result = UserTypes.Guest;
                while (mySqlDataReader.Read())
                    result = (UserTypes)mySqlDataReader.GetInt32(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return UserTypes.Guest;
            }
        }

        public User GetUser(string username, string password)
        {
            User user = new User(0, UserTypes.Guest, String.Empty, String.Empty, String.Empty, String.Empty, null, true, null);
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getUser(\"{username}\", \"{password}\");", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    int id = mySqlDataReader.GetInt32(0);
                    UserTypes userType = (UserTypes)mySqlDataReader.GetInt32(1);
                    //username (2)
                    //password (3)
                    string name = mySqlDataReader.GetString(4);
                    string surname = mySqlDataReader.GetString(5);
                    DateTime birthDate = mySqlDataReader.GetDateTime(6);
                    bool isActive = mySqlDataReader.GetBoolean(7);
                    DateTime creationDate = mySqlDataReader.GetDateTime(8);
                    user = new User(id, userType, username, password, name, surname, birthDate, isActive, creationDate);
                }
                mySqlDataReader.Close();
                return user;
            }
            catch
            {
                return new User(0, UserTypes.Guest, String.Empty, String.Empty, String.Empty, String.Empty, null, true, null);
            }
        }

        public bool AddUser(string username, string password, string name, string surname, string birthDate, UserTypes userType = UserTypes.Customer)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call addUser( {(int)userType}, \"{username}\", \"{password}\", \"{name}\",\"{surname}\",\'{birthDate}\');", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public Car GetCarById(int id)
        {
            Car car = null;
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getCarById({id});", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool hasRows = mySqlDataReader.HasRows;
                if (!hasRows)
                    return null;
                while (mySqlDataReader.Read())
                {
                    //int id = mySqlDataReader.GetInt32(0);
                    string plateNumber = mySqlDataReader.GetString(1);
                    double mileage = mySqlDataReader.GetDouble(2);
                    bool isActive = mySqlDataReader.GetBoolean(3);
                    DateTime inspectionDate = mySqlDataReader.GetDateTime(4);
                    int idCarDetails = mySqlDataReader.GetInt32(5);
                    BodyTypes bodyType = (BodyTypes)mySqlDataReader.GetInt32(6);
                    EngineTypes engineType = (EngineTypes)mySqlDataReader.GetInt32(7);
                    string brand = mySqlDataReader.GetString(8);
                    string model = mySqlDataReader.GetString(9);
                    int horsePower = mySqlDataReader.GetInt32(10);
                    decimal price = mySqlDataReader.GetDecimal(11);
                    byte[] photo = (byte[])mySqlDataReader[12];
                    CarDetails carDetails = new CarDetails(idCarDetails, bodyType, engineType, brand, model, horsePower, price, photo.ToBitmapImage());
                    car = new Car(id, carDetails, plateNumber, mileage, isActive, inspectionDate);
                }
                mySqlDataReader.Close();
                return car;
            }
            catch
            {
                return null;
            }
        }

        public List<ReservationPeriod> GetDatesOfCar(int id)
        {
            List<ReservationPeriod> list = new List<ReservationPeriod>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getDatesOfCar({id});", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    DateTime begin = mySqlDataReader.GetDateTime(0);
                    DateTime end = mySqlDataReader.GetDateTime(1);
                    list.Add(new ReservationPeriod(begin, end));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<SecurityPackage> GetSecurityPackages()
        {
            List<SecurityPackage> list = new List<SecurityPackage>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand("call getSecurityPackages();", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    int id = mySqlDataReader.GetInt32(0);
                    string name = mySqlDataReader.GetString(1);
                    string description = mySqlDataReader.GetString(2);
                    decimal price = mySqlDataReader.GetDecimal(3);
                    list.Add(new SecurityPackage(id, name, description, price));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public bool AddReservation(int carId, int securityPackadeId, ReservationPeriod reservationPeriod, decimal price)
        {
            try
            {
                int userId = CurrentUser.GetInstance().User.Id;
                string dateBegin = reservationPeriod.Begin.GetValueOrDefault().ToString("yyyy-MM-dd");
                string dateEnd = reservationPeriod.End.GetValueOrDefault().ToString("yyyy-MM-dd");
                //.ToString(CultureInfo.GetCultureInfo("en-GB"))
                MySqlCommand mySqlCommand = new MySqlCommand($"call addReservation({userId}, {carId}, {securityPackadeId}, \'{dateBegin}\', \'{dateEnd}\', {price.ToString(CultureInfo.GetCultureInfo("en-GB"))});", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public List<User> GetUsers()
        {
            List<User> list = new List<User>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getUsers();", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    int id = mySqlDataReader.GetInt32(0);
                    UserTypes userType = (UserTypes)mySqlDataReader.GetInt32(1);
                    string username = mySqlDataReader.GetString(2);
                    string password = mySqlDataReader.GetString(3);
                    string name = mySqlDataReader.GetString(4);
                    string surname = mySqlDataReader.GetString(5);
                    DateTime birthDate = mySqlDataReader.GetDateTime(6);
                    bool isActive = mySqlDataReader.GetBoolean(7);
                    DateTime creationDate = mySqlDataReader.GetDateTime(8);
                    list.Add(new User(id, userType, username, password, name, surname, birthDate, isActive, creationDate));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public bool UpdateUser(int id, UserTypes userType, string username, string password, string name, string surname, string birthDate, bool isActive)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call updateUser({id}, {(int)userType}, \"{username}\", \"{password}\", \"{name}\",\"{surname}\",\'{birthDate}\', {isActive});", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool AddModel(BodyTypes bodyType, EngineTypes engineType, string brand, string model, int horsePower, decimal price, string pathToImage)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call addModel({(int)bodyType}, {(int)engineType}, \"{brand}\",\"{model}\", {horsePower}, {price.ToString(CultureInfo.GetCultureInfo("en-GB"))}, \"{pathToImage.Replace("\\", "/")}\");", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public List<CarDetails> GetModels()
        {
            List<CarDetails> list = new List<CarDetails>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getCarDetails();", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    int id = mySqlDataReader.GetInt32(0);
                    BodyTypes bodyType = (BodyTypes)mySqlDataReader.GetInt32(1);
                    EngineTypes engineType = (EngineTypes)mySqlDataReader.GetInt32(2);
                    string brand = mySqlDataReader.GetString(3);
                    string model = mySqlDataReader.GetString(4);
                    int horsePower = mySqlDataReader.GetInt32(5);
                    decimal price = mySqlDataReader.GetDecimal(6);
                    byte[] photo = (byte[])mySqlDataReader[7];
                    list.Add(new CarDetails(id, bodyType, engineType, brand, model, horsePower, price, photo.ToBitmapImage()));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public bool UpdateModel(int id, BodyTypes bodyType, EngineTypes engineType, string brand, string model, int horsePower, decimal price, string path = null)
        {
            try
            {
                string query;
                if (!String.IsNullOrEmpty(path))
                    query = $"call updateModel({id}, {(int)bodyType}, {(int)engineType}, \"{brand}\",\"{model}\", {horsePower}, {price.ToString(CultureInfo.GetCultureInfo("en-GB"))}, \"{path.Replace("\\", "/")}\");";
                else
                    query = $"call updateModelWithoutPhoto({id}, {(int)bodyType}, {(int)engineType}, \"{brand}\",\"{model}\", {horsePower}, {price.ToString(CultureInfo.GetCultureInfo("en-GB"))});";
                MySqlCommand mySqlCommand = new MySqlCommand(query, _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool AddCar(int modelId, string plateNumber, double mileage, string inspectionDate)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call addCar({modelId}, \"{plateNumber}\", {mileage.ToString(CultureInfo.GetCultureInfo("en-GB"))}, \"{inspectionDate}\");", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool EditCar(int id, int modelId, string plateNumber, double mileage, bool isActive, string inspectionDate)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call editCar({id}, {modelId}, \"{plateNumber}\", {mileage.ToString(CultureInfo.GetCultureInfo("en-GB"))}, {isActive}, \"{inspectionDate}\");", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangePassword(string password)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call changePassword({CurrentUser.GetInstance().User.Id}, \"{password}\");", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangePersonalDetails(string name, string surname, string birthDate)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call changePersonalDetails({CurrentUser.GetInstance().User.Id}, \"{name}\", \"{surname}\", \'{birthDate}\');", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                bool result = false;
                while (mySqlDataReader.Read())
                    result = mySqlDataReader.GetBoolean(0);
                mySqlDataReader.Close();
                return result;
            }
            catch
            {
                return false;
            }
        }

        public List<Reservation> GetReservations()
        {
            List <Reservation> list = new List<Reservation>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call getReservationsByUserId({CurrentUser.GetInstance().User.Id});", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    string brand = mySqlDataReader.GetString(0);
                    string model = mySqlDataReader.GetString(1);
                    DateTime begin = mySqlDataReader.GetDateTime(2);
                    DateTime end = mySqlDataReader.GetDateTime(3);
                    Decimal price = mySqlDataReader.GetDecimal(4);
                    list.Add(new Reservation(brand, model, begin, end, price));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<(string, DateTime, DateTime, string, string, string)> GetNewReservations(string dateBegin, string dateEnd)
        {
            List<(string, DateTime, DateTime, string, string, string)> list = new List<(string, DateTime, DateTime, string, string, string)>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call raportNewReservations(\'{dateBegin}\', \'{dateEnd}\');", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    string login = mySqlDataReader.GetString(0);
                    DateTime begin = mySqlDataReader.GetDateTime(1);
                    DateTime end = mySqlDataReader.GetDateTime(2);
                    string plateNumber = mySqlDataReader.GetString(3);
                    string brand = mySqlDataReader.GetString(4);
                    string model = mySqlDataReader.GetString(5);
                    list.Add((login, begin, end, plateNumber, brand, model));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<(string, string, string, DateTime)>GetNewUsers(string dateBegin, string dateEnd)
        {
            List<(string, string, string, DateTime)> list = new List<(string, string, string, DateTime)>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call raportNewUsers(\'{dateBegin}\', \'{dateEnd}\');", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    string login = mySqlDataReader.GetString(0);
                    string name = mySqlDataReader.GetString(1);
                    string surname = mySqlDataReader.GetString(2);
                    DateTime birthDate = mySqlDataReader.GetDateTime(3);
                    list.Add((login, name, surname, birthDate));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<(DateTime, DateTime, decimal)> GetReservationsByPlateNumber(string plateNumber)
        {
            List<(DateTime, DateTime, decimal)> list = new List<(DateTime, DateTime, decimal)>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call raportReservationsByPlateNumber(\'{plateNumber}\');", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    
                    DateTime dateBegin = mySqlDataReader.GetDateTime(0);
                    DateTime dateEnd = mySqlDataReader.GetDateTime(1);
                    decimal price = mySqlDataReader.GetDecimal(2);
                    list.Add((dateBegin, dateEnd, price));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }

        public List<(DateTime, DateTime, decimal, string, string)> GetReservationsByLogin(string login)
        {
            List<(DateTime, DateTime, decimal, string, string)> list = new List<(DateTime, DateTime, decimal, string, string)>();
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"call raportUserReservationsByLogin(\'{login}\');", _instance.mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {

                    DateTime dateBegin = mySqlDataReader.GetDateTime(0);
                    DateTime dateEnd = mySqlDataReader.GetDateTime(1);
                    decimal price = mySqlDataReader.GetDecimal(2);
                    string brand = mySqlDataReader.GetString(3);
                    string model = mySqlDataReader.GetString(4);
                    list.Add((dateBegin, dateEnd, price, brand, model));
                }
                mySqlDataReader.Close();
                return list;
            }
            catch
            {
                return list;
            }
        }
    }
}
