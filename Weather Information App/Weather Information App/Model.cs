using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Information_App
{
    public  class Model
    {
        public class WeatherRequest
        {
            public string CityName { get; set; }
        }
        
        public class WeatherResult
        {
            public string Message { get; set; }
        }
    }
}
