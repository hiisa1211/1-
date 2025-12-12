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
            public string IconUrl { get; set; }

            public DateTime DateTime { get; set; }
            public string Description { get; set; }
            public double Temperature { get; set; }
        }

    }
}
