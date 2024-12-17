using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime OrderDate { get; set; }


        public string CarModel { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
    }
}
