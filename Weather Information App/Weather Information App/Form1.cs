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
<<<<<<< HEAD
        //WeatherService の生成
        private readonly WeatherService _service; 
=======
        // WeatherService の生成
        private readonly WeatherService _service;
>>>>>>> origin/main

        // 都道府県リスト
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

        // プレースホルダー用のテキスト
        private const string PlaceholderText = "県または都市を入力";

        public Form1()
        {
            InitializeComponent();

<<<<<<< HEAD
            // 初期表示（仮の値）
            lblHighTemp.Text = "今日の最高: --℃";
            lblLowTemp.Text = "今日の最低: --℃";
            lblLastUpdate.Text = "最終更新: --/--/-- --:--:--";


=======
            // PictureBox 設定
>>>>>>> origin/main
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.LightBlue;

            _service = new WeatherService();

            // AutoComplete 設定
            SetupAutoComplete();

            // TextBox イベント登録（Enter / Leave でプレースホルダー表示）
            textBox1.Enter += TextBox1_Enter;
            textBox1.Leave += TextBox1_Leave;

            // KeyDown イベント（Enterで検索）
            textBox1.KeyDown += TextBox1_KeyDown;

            // 履歴クリック
            listBoxHistory.SelectedIndexChanged += listBoxHistory_SelectedIndexChanged;

            // 初期プレースホルダー表示
            SetPlaceholder();

            // FlowLayoutPanel（3時間ごとの予報用）
            flowForecastPanel = new FlowLayoutPanel()
            {
                Location = new Point(20, 260),
                Size = new Size(this.ClientSize.Width - 40, 200),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.LightGray,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            this.Controls.Add(flowForecastPanel);
        }

        #region --- プレースホルダー処理 ---

        private void SetPlaceholder()
        {
            textBox1.Text = PlaceholderText;
            textBox1.ForeColor = Color.Gray;
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            // フォーカス時、プレースホルダーなら消す
            if (textBox1.Text == PlaceholderText)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            // フォーカス外、空ならプレースホルダー再表示
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SetPlaceholder();
            }
        }

        #endregion

        #region --- 入力処理 / AutoComplete ---

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

        // AutoComplete入力補完
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

        #endregion

        #region --- 天気取得 / 表示 ---

        private async Task SearchCity(string city)
        {
            string cityOnly = ExtractCityName(city);
            flowForecastPanel.Controls.Clear();

            if (string.IsNullOrWhiteSpace(cityOnly))
            {
                label1.Text = "市区町村名または都道府県名を入力してください。";
                return;
            }

            // 現在の天気
            WeatherResult current = await _service.GetWeatherAsync(cityOnly);
            label1.Text = current.Message;

            // アイコン表示
            if (!string.IsNullOrEmpty(current.IconUrl))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var iconBytes = await client.GetByteArrayAsync(current.IconUrl);
                        using (var ms = new MemoryStream(iconBytes))
                        using (var img = Image.FromStream(ms))
                        {
                            pictureBox1.Image = (Image)img.Clone();
                        }
                    }
                }
                catch { pictureBox1.Image = null; }
            }
            else
            {
                pictureBox1.Image = null;
            }

            // 3時間ごとの予報
            var forecasts = await _service.GetHourlyForecastAsync(cityOnly);
<<<<<<< HEAD

            DateTime today = DateTime.Today;

            // 今日のデータだけ取り出す
            var todays = forecasts.Where(f => f.DateTime.Date == today);


            double maxTemp = forecasts.Max(f => f.Temperature);
            double minTemp = forecasts.Min(f => f.Temperature);

            lblHighTemp.Text = $"今日の最高: {maxTemp}℃";
            lblLowTemp.Text = $"今日の最低: {minTemp}℃";
            lblLastUpdate.Text = $"最終更新: {DateTime.Now:yyyy/MM/dd HH:mm:ss}";



            // 今の時間以降の予報だけにフィルター
=======
>>>>>>> origin/main
            DateTime now = DateTime.Now;
            var upcomingForecasts = forecasts.Where(f => f.DateTime >= now).Take(8);

            foreach (var f in upcomingForecasts)
            {
                DateTime dt = f.DateTime;
                Panel card = new Panel()
                {
                    Width = 140,
                    Height = 120,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5),
                    BackColor = Color.White
                };

                PictureBox pbIcon = new PictureBox()
                {
                    Location = new Point(80, 10),
                    Size = new Size(48, 48),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                if (!string.IsNullOrEmpty(f.IconUrl))
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var bytes = await client.GetByteArrayAsync(f.IconUrl);
                            using (var ms = new MemoryStream(bytes))
                            using (var img = Image.FromStream(ms))
                            {
                                pbIcon.Image = (Image)img.Clone();
                            }
                        }
                    }
                    catch { }
                }

                card.Controls.Add(new Label() { Text = dt.ToString("yyyy/MM/dd"), Location = new Point(10, 10), AutoSize = true });
                card.Controls.Add(new Label() { Text = dt.ToString("HH:mm"), Location = new Point(10, 30), AutoSize = true });
                card.Controls.Add(new Label() { Text = f.Description, Location = new Point(10, 50), AutoSize = true });
                card.Controls.Add(new Label() { Text = $"{f.Temperature}℃", Location = new Point(10, 70), AutoSize = true });
                card.Controls.Add(pbIcon);

                flowForecastPanel.Controls.Add(card);
            }
        }

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

            // 天気取得
            await SearchCity(cityOnly);

<<<<<<< HEAD
            // 今日の最高 / 最低気温
            //var (minTemp, maxTemp) = await _service.GetTodayMinMaxAsync(cityOnly);
            //labelMinMax.Text = $"今日の最高: {maxTemp:F1}℃ / 最低: {minTemp:F1}℃";

            //  最終更新時刻を表示 
            //labelUpdateTime.Text = $"最終更新: {DateTime.Now:yyyy/MM/dd HH:mm:ss}";
=======
            // 今日の最高/最低
            var (minTemp, maxTemp) = await _service.GetTodayMinMaxAsync(cityOnly);
            labelMinMax.Text = $"今日の最高: {maxTemp:F1}℃ / 最低: {minTemp:F1}℃";

            // 更新時間
            labelUpdateTime.Text = $"最終更新: {DateTime.Now:yyyy/MM/dd HH:mm:ss}";
>>>>>>> origin/main
        }

        #endregion

        #region --- イベントハンドラ ---

        private async void button1_Click(object sender, EventArgs e)
        {
            await PerformSearch(textBox1.Text);
        }

        private async void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await PerformSearch(textBox1.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private async void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHistory.SelectedItem == null) return;

            string selectedCity = listBoxHistory.SelectedItem.ToString();
            textBox1.Text = selectedCity;

            await SearchCity(selectedCity);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // 画像クリック時の処理（必要に応じて）
        }

<<<<<<< HEAD
        private void label3_Click(object sender, EventArgs e)
        {

        }
=======
        #endregion
>>>>>>> origin/main
    }
}
