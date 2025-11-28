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
        }

        // 共通検索メソッド
        private void SearchCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return;

            var request = new WeatherRequest { CityName = city };
            WeatherResult result = _service.ProcessInput(request);
            label1.Text = result.Message;
        }

        // ボタンクリック
        private void button1_Click(object sender, EventArgs e)
        {
            string city = textBox1.Text;
            if (string.IsNullOrWhiteSpace(city)) return;

            // 検索履歴に追加
            if (listBoxHistory.Items.Contains(city))
                listBoxHistory.Items.Add(city);

            listBoxHistory.Items.Insert(0, city); // 先頭に追加

            // 検索実行
            SearchCity(city);
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
