using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Weather_Information_App.Model;

namespace Weather_Information_App
{
    public class WeatherService
    {
        public WeatherResult ProcessInput(WeatherRequest request)
        {
            return new WeatherResult
            {
                Message = $"入力: {request.CityName}"
            };
        }
    }

}
