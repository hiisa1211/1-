using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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
        private void button1_Click(object sender, EventArgs e)
        {
            // 入力を Model に詰める
            var request = new WeatherRequest
            {
                CityName = textBox1.Text
            };

            // Service を呼ぶ
            WeatherResult result = _service.ProcessInput(request);

            // 結果を UI に反映
            label1.Text = result.Message;
        }
    }
}
