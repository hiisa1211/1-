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

            // Enterキーで検索できるようにイベント追加
            textBox1.KeyDown += TextBox1_KeyDown;
            listBoxHistory.SelectedIndexChanged += listBoxHistory_SelectedIndexChanged;
        }

        // 共通検索メソッド
        private void SearchCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return;

            var request = new WeatherRequest { CityName = city };
            WeatherResult result = _service.ProcessInput(request);
            label1.Text = result.Message;
        }


        // 検索処理（履歴追加＆検索）
        private void PerformSearch(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return;

            // 既に履歴にある場合は削除して先頭に追加
            if (listBoxHistory.Items.Contains(city))
                listBoxHistory.Items.Remove(city);

            listBoxHistory.Items.Insert(0, city); // 最新を上に追加

            SearchCity(city);
        }

        // ボタンクリック
        private void button1_Click(object sender, EventArgs e)
        {
            PerformSearch(textBox1.Text);
        }

 
        

        // TextBoxでEnterキー押したとき
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch(textBox1.Text);
                e.Handled = true;    // ビープ音防止
                e.SuppressKeyPress = true;
            }
        }

        // 履歴クリック
        private void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxHistory.SelectedItem == null) return;

            string selectedCity = listBoxHistory.SelectedItem.ToString();
            textBox1.Text = selectedCity;

            // 検索実行
            SearchCity(selectedCity);
        }
    }
}
