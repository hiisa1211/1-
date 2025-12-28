using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Weather_Information_App.Model;

namespace Weather_Information_App
{
    public partial class Form1 : Form
    {
        //WeatherService の生成
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
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.LightBlue; // 背景色を変えて見やすく

            _service = new WeatherService();

            SetupAutoComplete();

            textBox1.KeyDown += TextBox1_KeyDown;
            listBoxHistory.SelectedIndexChanged += listBoxHistory_SelectedIndexChanged;

            // ListBox をフォームに追加（3時間ごとの天気表示用）
            /*listBoxForecast = new ListBox
            {
                Name = "listBoxForecast",
                Width = 300,
                Height = 200,
                Top = label1.Bottom + 10,  // label1 の下に配置
                Left = label1.Left
            };
            this.Controls.Add(listBoxForecast);*/
            flowForecastPanel = new FlowLayoutPanel()
            {
                Location = new Point(20, 260),
                Size = new Size(1200, 200),//どこまで表示するか
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,     //（スクロールするかしないか）
                AutoScroll = true
                //間違ってなかったらここに画像追加（多分？）
            };

            this.Controls.Add(flowForecastPanel);
        }

        //private ListBox listBoxForecast;

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
            //追加
            flowForecastPanel.Controls.Clear();


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
            /*listBoxForecast.Items.Clear();
            foreach (var f in forecasts)
            {
                listBoxForecast.Items.Add(f.Message);
            }*/
            


            foreach (var f in forecasts)
            {
                // APIから直接取り出した DateTime / Description / Temperature を使用
                DateTime dt = f.DateTime;
                string weather = f.Description;
                string temp = $"{f.Temperature}℃";

                Panel card = new Panel()
                {
                    Width = 140,
                    Height = 120,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5),
                    BackColor = Color.WhiteSmoke
                };

                Label lblDate = new Label()
                {
                    Text = dt.ToString("yyyy/MM/dd"),
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                Label lblTime = new Label()
                {
                    Text = dt.ToString("HH:mm"),
                    Location = new Point(10, 30),
                    AutoSize = true
                };

                Label lblWeather = new Label()
                {
                    Text = weather,
                    Location = new Point(10, 50),
                    AutoSize = true
                };

                Label lblTemp = new Label()
                {
                    Text = temp,
                    Location = new Point(10, 70),
                    AutoSize = true
                };

                card.Controls.Add(lblDate);
                card.Controls.Add(lblTime);
                card.Controls.Add(lblWeather);
                card.Controls.Add(lblTemp);

                flowForecastPanel.Controls.Add(card);
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

            // 履歴管理
            if (listBoxHistory.Items.Contains(cityOnly))
                listBoxHistory.Items.Remove(cityOnly);

            listBoxHistory.Items.Insert(0, cityOnly);

            // 天気取得（現在＋予報）
            await SearchCity(cityOnly);

            // 今日の最高 / 最低気温
            var (minTemp, maxTemp) = await _service.GetTodayMinMaxAsync(cityOnly);
            labelMinMax.Text = $"今日の最高: {maxTemp:F1}℃ / 最低: {minTemp:F1}℃";

            //  最終更新時刻を表示 
            labelUpdateTime.Text = $"最終更新: {DateTime.Now:yyyy/MM/dd HH:mm:ss}";

            WeatherResult result = await _service.GetWeatherAsync(cityOnly);
            label1.Text = result.Message;

            // アイコン表示処理
            if (!string.IsNullOrEmpty(result.IconUrl))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var iconBytes = await client.GetByteArrayAsync(result.IconUrl);
                        using (var ms = new MemoryStream(iconBytes))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"画像取得エラー: {ex.Message}");
                }

            }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
