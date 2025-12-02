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
            try
            {
                using (var client = new HttpClient())
                {
                    string url =
                        $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric&lang=ja";

                    // ★ GetAsync を使うことで「存在しない都市」で例外にならない
                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new WeatherResult
                        {
                            Message = $"都市「{cityName}」は見つかりませんでした。"
                        };
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    string description = data.weather[0].description;
                    double temp = data.main.temp;

                    return new WeatherResult
                    {
                        Message = $"{cityName} の天気: {description}, 気温: {temp}℃"
                    };
                }
            }
            catch
            {
                return new WeatherResult
                {
                    Message = "通信エラーが発生しました。"
                };
            }
        }

    }
}
