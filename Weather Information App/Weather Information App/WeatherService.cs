using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
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

                    var response = await client.GetAsync(url);

                    // エラーコードチェック
                    if (!response.IsSuccessStatusCode)
                    {
                        // APIが返すエラー
                        return new WeatherResult
                        {
                            Message = $"都市「{cityName}」は見つかりませんでした。"
                        };
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    string description = data.weather[0].description;
                    double temp = data.main.temp;

                    string iconCode = data.weather[0].icon; // "01d" などが入る
                    string iconUrl = $"https://openweathermap.org/img/wn/{iconCode}@2x.png";

                    return new WeatherResult
                    {
                        Message = $"{cityName} の天気: {description}, 気温: {temp}℃",
                        IconUrl = iconUrl
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
        public async Task<List<WeatherResult>> GetHourlyForecastAsync(string cityName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"http://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={apiKey}&units=metric&lang=ja";

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        return new List<WeatherResult> { new WeatherResult { Message = $"都市「{cityName}」は見つかりませんでした。" } };
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    var list = new List<WeatherResult>();

                    foreach (var item in data.list)
                    {
                        DateTime dt = DateTime.Parse((string)item.dt_txt);
                        string desc = item.weather[0].description;
                        double temp = item.main.temp;

                        // ★ 追加：アイコンコードとURL
                        string iconCode = item.weather[0].icon;

                        // ★ 晴れ系は昼アイコンに統一
                        if (iconCode.StartsWith("01") ||
                            iconCode.StartsWith("02") ||
                            iconCode.StartsWith("10"))
                        {
                            iconCode = iconCode.Substring(0, 2) + "d";
                        }

                        string iconUrl = $"https://openweathermap.org/img/wn/{iconCode}@2x.png";


                        list.Add(new WeatherResult
                        {
                            DateTime = dt,
                            Description = desc,
                            Temperature = temp,
                            Message = $"{dt}: {desc}, 気温: {temp}℃",
                            IconUrl = iconUrl
                        });
                    }


                    return list;
                }

            }
            catch
            {
                return new List<WeatherResult> { new WeatherResult { Message = "通信エラーが発生しました。" } };
            }
        }
        public async Task<(double minTemp, double maxTemp)> GetTodayMinMaxAsync(string cityName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"http://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={apiKey}&units=metric&lang=ja";

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        return (0, 0);
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    double? min = null;
                    double? max = null;

                    DateTime today = DateTime.Today;

                    foreach (var item in data.list)
                    {
                        DateTime dt = DateTime.Parse((string)item.dt_txt);

                        if (dt.Date == today)
                        {
                            double temp = item.main.temp;

                            if (min == null || temp < min) min = temp;
                            if (max == null || temp > max) max = temp;
                        }
                    }

                    if (min == null || max == null)
                    {
                        return (0, 0);
                    }

                    return (min.Value, max.Value);
                }
            }
            catch
            {
                return (0, 0);
            }
        }


    }
}
