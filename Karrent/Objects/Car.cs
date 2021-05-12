using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karrent.Objects
{
    class Car
    {
        public int Id { get; set; }
        public CarDetails CarDetails { get; set; }
        public string PlateNumber { get; set; }
        public double Mileage { get; set; }
        public bool IsActive{ get; set; }
        public DateTime InspectionDate { get; set; }

        public Car(int id, CarDetails carDetails, string plateNumber, double mileage, bool isActive, DateTime inspectionDate)
        {
            this.Id = id;
            this.CarDetails = carDetails;
            this.PlateNumber = plateNumber;
            this.Mileage = mileage;
            this.IsActive = isActive;
            this.InspectionDate = inspectionDate;
        }

        public override string ToString()
        {
            return $"Id:{this.Id} " +
                $"Car details:{this.CarDetails.ToString()} " +
                $"Plate number:{this.PlateNumber} " +
                $"Mileage:{this.Mileage} " +
                $"Is active:{this.IsActive} " +
                $"Inspection date:{this.InspectionDate:dd-MM-yyyy}";
        }
    }
}
