using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karrent.Objects
{
    class Reservation
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public decimal Price { get; set; }
        public Reservation(string brand, string model, DateTime begin, DateTime end, decimal price)
        {
            this.Brand = brand;
            this.Model = model;
            this.Begin = begin;
            this.End = end;
            this.Price = price;
        }
    }
}
