using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Weather_Information_App.Model;

namespace Weather_Information_App
{
    public class WeatherService
    {
        private readonly string apiKey = "ここに自分のAPIキー"; // ←ここに取得したAPIキー

        // ここを Form1 から呼びます
        public async Task<WeatherResult> GetWeatherAsync(string cityName)
        {
            using (var client = new HttpClient())
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric&lang=ja";
                var response = await client.GetStringAsync(url);
                dynamic data = JsonConvert.DeserializeObject(response);

                string description = data.weather[0].description;
                double temp = data.main.temp;

                return new WeatherResult
                {
                    Message = $"{cityName} の天気: {description}, 気温: {temp}℃"
                };
            }
        }
    }
}
