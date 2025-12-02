using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Weather_Information_App.Model;

namespace Weather_Information_App
{
    public partial class Form1 : Form
    {
        private readonly WeatherService _service;

        public Form1()
        {
            InitializeComponent();
            _service = new WeatherService();

            SetupAutoComplete();

            // Enterキーで検索できるようにイベント追加
            textBox1.KeyDown += TextBox1_KeyDown;
            listBoxHistory.SelectedIndexChanged += listBoxHistory_SelectedIndexChanged;
        }

        private void SetupAutoComplete()
        {
            var source = new AutoCompleteStringCollection();

            // cities.txt の読み込み（例: "北海道 札幌市"）
            string[] lines = System.IO.File.ReadAllLines("cities.txt");

            // 候補と検索用マップを作る
            var autoCompleteMap = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                // 空白で分割
                string[] parts = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                string display = trimmed;          // 候補に表示する文字列
                string searchText = parts.Length > 1 ? parts[1] : parts[0]; // 空白以降を検索用に

                autoCompleteMap[display] = searchText;
            }

            source.AddRange(autoCompleteMap.Keys.ToArray());

            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = source;

            // 選択時に検索用文字列に置き換える
            textBox1.TextChanged += (s, e) =>
            {
                string text = textBox1.Text;

                if (autoCompleteMap.ContainsKey(text))
                {
                    string searchText = autoCompleteMap[text];

                    textBox1.TextChanged -= textBox1_TextChanged;
                    textBox1.Text = searchText;
                    textBox1.SelectionStart = textBox1.Text.Length;
                    textBox1.TextChanged += textBox1_TextChanged;
                }
            };
        }

        // 無限ループ防止用ハンドラ
        private void textBox1_TextChanged(object sender, EventArgs e) { }



        // 共通検索メソッド
        private async Task SearchCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return;

            WeatherResult result = await _service.GetWeatherAsync(city);
            label1.Text = result.Message;
        }


        // 検索処理（履歴追加＆検索）
        private async Task PerformSearch(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return;

            // 既に履歴にある場合は削除して先頭に追加
            if (listBoxHistory.Items.Contains(city))
                listBoxHistory.Items.Remove(city);

            listBoxHistory.Items.Insert(0, city); // 最新を上に追加

            WeatherResult result = await _service.GetWeatherAsync(city);
            label1.Text = result.Message;
        }

        // ボタンクリック
        private async void button1_Click(object sender, EventArgs e)
        {
            await PerformSearch(textBox1.Text);
        }

 
        

        // TextBoxでEnterキー押したとき
        private async void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await PerformSearch(textBox1.Text);
                e.Handled = true;    // ビープ音防止
                e.SuppressKeyPress = true;
            }
        }

        // 履歴クリック
        private async void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHistory.SelectedItem == null) return;

            string selectedCity = listBoxHistory.SelectedItem.ToString();
            textBox1.Text = selectedCity;

            // 検索実行 
            await SearchCity(selectedCity);
        }
    }
}
