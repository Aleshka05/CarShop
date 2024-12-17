using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Model
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string ReviewText { get; set; }
        public DateTime Date { get; set; }
    }
}
