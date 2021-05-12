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

namespace Karrent
{
    class DBManager
    {
        private string connectionString = "datasource=localhost; port=3306; username=karrent; password=123qwe; database=karrent";
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
                MySqlCommand mySqlCommand = new MySqlCommand("call getCars();", _instance.mySqlConnection);
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
    }
}
