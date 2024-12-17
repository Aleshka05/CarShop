using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Model
{
    public class Car
    {
        public int CarID { get; set; }
        public string CarModel { get; set; }
        public int Year { get; set; }
        public string TransmissionType { get; set; }
        public string CarType { get; set; }
        public string DriveType { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string BodyType { get; set; }
        public string CarDesc { get; set; }
        public decimal CarPrice { get; set; }

        public string CarImage { get; set; }

        public virtual CarConfiguration Configuration { get; set; }
    }
}
