using CarShopApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShop.Model;
namespace CarShop.Services
{
    public class CarService
    {
        public List<Car> GetCars()
        {
            using (var context = new CarShopContext())
            {
                
                var cars = context.Cars.ToList();
                return cars;
            }
        }
    }
}
