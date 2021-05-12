using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karrent.Enums;
using System.Windows.Media.Imaging;

namespace Karrent.Objects
{
    class CarDetails
    {
        public int Id { get; set; }
        public BodyTypes BodyType { get; set; }
        public EngineTypes EngineType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public decimal Price { get; set; }
        public BitmapImage Photo { get; set; }

        public CarDetails(int id, BodyTypes bodyType, EngineTypes engineType, string brand,
            string model, int horsePower, decimal price, BitmapImage photo)
        {
            this.Id = id;
            this.BodyType = bodyType;
            this.EngineType = engineType;
            this.Brand = brand;
            this.Model = model;
            this.HorsePower = horsePower;
            this.Price = price;
            this.Photo = photo;
        }

        public override string ToString()
        {
            return $"Id:{this.Id} " +
                $"Body type:{this.BodyType} " +
                $"Engine type:{this.EngineType} " +
                $"Brand:{this.Brand} " +
                $"Model:{this.Model} " +
                $"Horse power:{this.HorsePower} " +
                $"Price:{this.Price} ";
        }
    }
}
