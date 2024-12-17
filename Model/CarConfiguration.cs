using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Model
{
    public class CarConfiguration
    {
        public int ConfigurationID { get; set; }  
        public int CarID { get; set; }            

        public string Salon { get; set; }
        public string SafetySystems { get; set; }
        public string Airbags { get; set; }
        public string AssistanceSystems { get; set; }
        public string Exterior { get; set; }
        public string Multimedia { get; set; }

        public virtual Car Car { get; set; }     
    }
}
