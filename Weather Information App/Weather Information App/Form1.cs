using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Weather_Information_App.Model;

namespace Weather_Information_App
{
    public partial class Form1 : Form
    {
        private readonly WeatherService _service;

        private readonly string[] prefectures = new[]
        {
            "北海道","青森県","岩手県","宮城県","秋田県","山形県","福島県",
            "茨城県","栃木県","群馬県","埼玉県","千葉県","東京都","神奈川県",
            "新潟県","富山県","石川県","福井県","山梨県","長野県",
            "岐阜県","静岡県","愛知県","三重県",
            "滋賀県","京都府","大阪府","兵庫県","奈良県","和歌山県",
            "鳥取県","島根県","岡山県","広島県","山口県",
            "徳島県","香川県","愛媛県","高知県",
            "福岡県","佐賀県","長崎県","熊本県","大分県","宮崎県","鹿児島県","沖縄県"
        };

        public Form1()
        {
            InitializeComponent();
            _service = new WeatherService();

            SetupAutoComplete();

            textBox1.KeyDown += TextBox1_KeyDown;
            listBoxHistory.SelectedIndexChanged += listBoxHistory_SelectedIndexChanged;

            // ListBox をフォームに追加（3時間ごとの天気表示用）
            listBoxForecast = new ListBox
            {
                Name = "listBoxForecast",
                Width = 300,
                Height = 200,
                Top = label1.Bottom + 10,  // label1 の下に配置
                Left = label1.Left
            };
            this.Controls.Add(listBoxForecast);
        }

        private ListBox listBoxForecast;

        // 入力から市区町村名を抽出
        private string ExtractCityName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            string text = input.Trim();
            string matchedPref = null;

            foreach (var pref in prefectures)
            {
                if (text.StartsWith(pref))
                {
                    matchedPref = pref;
                    text = text.Substring(pref.Length);
                    break;
                }
            }

            var parts = text.Split(new[] { ' ', '　', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                return parts.Last();
            }

            if (!string.IsNullOrEmpty(matchedPref))
            {
                return matchedPref;
            }

            return text;
        }

        // AutoComplete
        private void SetupAutoComplete()
        {
            var source = new AutoCompleteStringCollection();
            string[] lines = File.ReadAllLines("cities.txt");

            foreach (var line in lines)
            {
                string city = line.Trim();
                if (!string.IsNullOrWhiteSpace(city))
                {
                    source.Add(city);
                }
            }

            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = source;
        }

        // 履歴を使わず検索のみ
        private async Task SearchCity(string city)
        {
            string cityOnly = ExtractCityName(city);

            if (string.IsNullOrWhiteSpace(cityOnly))
            {
                label1.Text = "市区町村名または都道府県名を入力してください。";
                return;
            }

            // 現在の天気
            WeatherResult current = await _service.GetWeatherAsync(cityOnly);
            label1.Text = current.Message;

            // 3時間ごとの予報
            var forecasts = await _service.GetHourlyForecastAsync(cityOnly);
            listBoxForecast.Items.Clear();
            foreach (var f in forecasts)
            {
                listBoxForecast.Items.Add(f.Message);
            }
        }


        // 検索（履歴に追加)
        private async Task PerformSearch(string city)
        {
            string cityOnly = ExtractCityName(city);

            if (string.IsNullOrWhiteSpace(cityOnly))
            {
                label1.Text = "市区町村名または都道府県名を入力してください。";
                return;
            }

            if (listBoxHistory.Items.Contains(cityOnly))
                listBoxHistory.Items.Remove(cityOnly);

            listBoxHistory.Items.Insert(0, cityOnly);

            await SearchCity(cityOnly);
        }

        // ボタンクリック
        private async void button1_Click(object sender, EventArgs e)
        {
            await PerformSearch(textBox1.Text);
        }

        // Enterキー押下
        private async void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await PerformSearch(textBox1.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // 履歴クリック
        private async void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHistory.SelectedItem == null) return;

            string selectedCity = listBoxHistory.SelectedItem.ToString();
            textBox1.Text = selectedCity;

            await SearchCity(selectedCity);
        }
    }
}
