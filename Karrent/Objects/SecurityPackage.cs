using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karrent.Objects
{
    class SecurityPackage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public SecurityPackage(int id, string name, string description, decimal price)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }

        public override string ToString()
        {
            return $"{this.Id} {this.Name} {this.Description} {this.Price:F2}";
        }
    }
}
