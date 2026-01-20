using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Weather_Information_App
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);


        }
        
        

        private System.Windows.Forms.TextBox textBox1; 
        private System.Windows.Forms.Label label1;      
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBoxHistory;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelUpdateTime; 
        private System.Windows.Forms.Label labelMinMax;
        private FlowLayoutPanel flowForecastPanel;




        //Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listBoxHistory = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelUpdateTime = new System.Windows.Forms.Label();
            this.labelMinMax = new System.Windows.Forms.Label();
            this.lblHighTemp = new System.Windows.Forms.Label();
            this.lblLowTemp = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(20, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(400, 25);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(150, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 20);
            this.label1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(430, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "検索";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxHistory
            // 
            this.listBoxHistory.ItemHeight = 18;
            this.listBoxHistory.Location = new System.Drawing.Point(400, 50);
            this.listBoxHistory.Name = "listBoxHistory";
            this.listBoxHistory.Size = new System.Drawing.Size(310, 184);
            this.listBoxHistory.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(20, 65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(105, 92);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelUpdateTime
            // 
            this.labelUpdateTime.Location = new System.Drawing.Point(20, 210);
            this.labelUpdateTime.Name = "labelUpdateTime";
            this.labelUpdateTime.Size = new System.Drawing.Size(300, 25);
            this.labelUpdateTime.TabIndex = 5;
            // 
            // labelMinMax
            // 
            this.labelMinMax.Location = new System.Drawing.Point(20, 180);
            this.labelMinMax.Name = "labelMinMax";
            this.labelMinMax.Size = new System.Drawing.Size(350, 25);
            this.labelMinMax.TabIndex = 5;
            // 
            // lblHighTemp
            // 
            this.lblHighTemp.AutoSize = true;
            this.lblHighTemp.Location = new System.Drawing.Point(33, 192);
            this.lblHighTemp.Name = "lblHighTemp";
            this.lblHighTemp.Size = new System.Drawing.Size(52, 18);
            this.lblHighTemp.TabIndex = 6;
            this.lblHighTemp.Text = "label2";
            // 
            // lblLowTemp
            // 
            this.lblLowTemp.AutoSize = true;
            this.lblLowTemp.Location = new System.Drawing.Point(186, 192);
            this.lblLowTemp.Name = "lblLowTemp";
            this.lblLowTemp.Size = new System.Drawing.Size(52, 18);
            this.lblLowTemp.TabIndex = 7;
            this.lblLowTemp.Text = "label3";
            this.lblLowTemp.Click += new System.EventHandler(this.label3_Click);
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Location = new System.Drawing.Point(33, 235);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(52, 18);
            this.lblLastUpdate.TabIndex = 8;
            this.lblLastUpdate.Text = "label4";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(739, 550);
            this.Controls.Add(this.lblLastUpdate);
            this.Controls.Add(this.lblLowTemp);
            this.Controls.Add(this.lblHighTemp);
            this.Controls.Add(this.listBoxHistory);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelMinMax);
            this.Controls.Add(this.labelUpdateTime);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label lblHighTemp;
        private Label lblLowTemp;
        private Label lblLastUpdate;
    }
}

