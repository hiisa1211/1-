using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Weather_Information_App.Model;

namespace Weather_Information_App
{
    public class WeatherService
    {
        private readonly string apiKey = "6d952d0fd8105cefbfd6605ab7d41edc"; // ←ここに取得したAPIキー

        // 天気情報取得
        public async Task<WeatherResult> GetWeatherAsync(string cityName)
        {
            using (var client = new HttpClient())
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric&lang=ja";
                var response = await client.GetStringAsync(url);
                dynamic data = JsonConvert.DeserializeObject(response);

                // エラーコードチェック
                if (data.cod != null && data.cod.ToString() != "200")
                {
                    // APIが返すエラー
                    return new WeatherResult
                    {
                        Message = $"都市「{cityName}」は見つかりませんでした。"
                    };
                }

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
