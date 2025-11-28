using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Information_App
{
    public class Service
    {
        public class WeatherService
        {
            public WeatherResult ProcessInput(WeatherRequest request)
            {
                return new WeatherResult
                {
                    Message = $"入力された都市名:{request.CityName}"
                };
            }

        }

    }
}
