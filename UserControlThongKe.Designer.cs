namespace QL_ThuocTay
{
    partial class UserControlThongKe
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tPDoanhThu = new System.Windows.Forms.TabPage();
            this.lbTong = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btInBaoCao = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btApDung = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rBNam = new System.Windows.Forms.RadioButton();
            this.rBThang = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chartDoanhThu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tPThuoc = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btApDung2 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbTong2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btInBaoCao2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tPDoanhThu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoanhThu)).BeginInit();
            this.tPThuoc.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tPDoanhThu);
            this.tabControl1.Controls.Add(this.tPThuoc);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1054, 643);
            this.tabControl1.TabIndex = 0;
            // 
            // tPDoanhThu
            // 
            this.tPDoanhThu.BackColor = System.Drawing.Color.LightCyan;
            this.tPDoanhThu.Controls.Add(this.lbTong);
            this.tPDoanhThu.Controls.Add(this.label1);
            this.tPDoanhThu.Controls.Add(this.label4);
            this.tPDoanhThu.Controls.Add(this.btInBaoCao);
            this.tPDoanhThu.Controls.Add(this.groupBox1);
            this.tPDoanhThu.Controls.Add(this.dataGridView1);
            this.tPDoanhThu.Controls.Add(this.chartDoanhThu);
            this.tPDoanhThu.Location = new System.Drawing.Point(4, 29);
            this.tPDoanhThu.Name = "tPDoanhThu";
            this.tPDoanhThu.Padding = new System.Windows.Forms.Padding(3);
            this.tPDoanhThu.Size = new System.Drawing.Size(1046, 610);
            this.tPDoanhThu.TabIndex = 0;
            this.tPDoanhThu.Text = "Doanh thu";
            this.tPDoanhThu.Click += new System.EventHandler(this.tPDoanhThu_Click);
            // 
            // lbTong
            // 
            this.lbTong.AutoSize = true;
            this.lbTong.Location = new System.Drawing.Point(671, 542);
            this.lbTong.Name = "lbTong";
            this.lbTong.Size = new System.Drawing.Size(0, 20);
            this.lbTong.TabIndex = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(577, 542);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 100;
            this.label1.Text = "Tổng cộng:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(244, 29);
            this.label4.TabIndex = 99;
            this.label4.Text = "Thống kê doanh thu";
            // 
            // btInBaoCao
            // 
            this.btInBaoCao.Location = new System.Drawing.Point(771, 565);
            this.btInBaoCao.Name = "btInBaoCao";
            this.btInBaoCao.Size = new System.Drawing.Size(108, 39);
            this.btInBaoCao.TabIndex = 3;
            this.btInBaoCao.Text = "In báo cáo";
            this.btInBaoCao.UseVisualStyleBackColor = true;
            this.btInBaoCao.Click += new System.EventHandler(this.btInBaoCao_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.btApDung);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rBNam);
            this.groupBox1.Controls.Add(this.rBThang);
            this.groupBox1.Location = new System.Drawing.Point(6, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 108);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lựa chọn thống kê";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(352, 32);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(195, 26);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // btApDung
            // 
            this.btApDung.Location = new System.Drawing.Point(396, 64);
            this.btApDung.Name = "btApDung";
            this.btApDung.Size = new System.Drawing.Size(102, 34);
            this.btApDung.TabIndex = 4;
            this.btApDung.Text = "Áp dụng";
            this.btApDung.UseVisualStyleBackColor = true;
            this.btApDung.Click += new System.EventHandler(this.btApDung_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(106, 70);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(229, 28);
            this.comboBox1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Chi nhánh:";
            // 
            // rBNam
            // 
            this.rBNam.AutoSize = true;
            this.rBNam.Location = new System.Drawing.Point(183, 34);
            this.rBNam.Name = "rBNam";
            this.rBNam.Size = new System.Drawing.Size(105, 24);
            this.rBNam.TabIndex = 5;
            this.rBNam.TabStop = true;
            this.rBNam.Text = "Theo năm";
            this.rBNam.UseVisualStyleBackColor = true;
            this.rBNam.CheckedChanged += new System.EventHandler(this.rBNam_CheckedChanged);
            // 
            // rBThang
            // 
            this.rBThang.AutoSize = true;
            this.rBThang.Location = new System.Drawing.Point(13, 34);
            this.rBThang.Name = "rBThang";
            this.rBThang.Size = new System.Drawing.Size(115, 24);
            this.rBThang.TabIndex = 4;
            this.rBThang.TabStop = true;
            this.rBThang.Text = "Theo tháng";
            this.rBThang.UseVisualStyleBackColor = true;
            this.rBThang.CheckedChanged += new System.EventHandler(this.rBThang_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(577, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(463, 523);
            this.dataGridView1.TabIndex = 1;
            // 
            // chartDoanhThu
            // 
            chartArea1.Name = "ChartArea1";
            this.chartDoanhThu.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartDoanhThu.Legends.Add(legend1);
            this.chartDoanhThu.Location = new System.Drawing.Point(3, 178);
            this.chartDoanhThu.Name = "chartDoanhThu";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartDoanhThu.Series.Add(series1);
            this.chartDoanhThu.Size = new System.Drawing.Size(568, 426);
            this.chartDoanhThu.TabIndex = 0;
            this.chartDoanhThu.Text = "chart1";
            // 
            // tPThuoc
            // 
            this.tPThuoc.BackColor = System.Drawing.Color.LightCyan;
            this.tPThuoc.Controls.Add(this.groupBox2);
            this.tPThuoc.Controls.Add(this.lbTong2);
            this.tPThuoc.Controls.Add(this.label6);
            this.tPThuoc.Controls.Add(this.btInBaoCao2);
            this.tPThuoc.Controls.Add(this.dataGridView2);
            this.tPThuoc.Location = new System.Drawing.Point(4, 29);
            this.tPThuoc.Name = "tPThuoc";
            this.tPThuoc.Padding = new System.Windows.Forms.Padding(3);
            this.tPThuoc.Size = new System.Drawing.Size(1046, 610);
            this.tPThuoc.TabIndex = 1;
            this.tPThuoc.Text = "Thuốc hết hạn";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btApDung2);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(8, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(565, 67);
            this.groupBox2.TabIndex = 108;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Lựa chọn thống kê";
            // 
            // btApDung2
            // 
            this.btApDung2.Location = new System.Drawing.Point(397, 19);
            this.btApDung2.Name = "btApDung2";
            this.btApDung2.Size = new System.Drawing.Size(102, 34);
            this.btApDung2.TabIndex = 4;
            this.btApDung2.Text = "Áp dụng";
            this.btApDung2.UseVisualStyleBackColor = true;
            this.btApDung2.Click += new System.EventHandler(this.btApDung2_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(107, 25);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(229, 28);
            this.comboBox2.TabIndex = 5;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Chi nhánh:";
            // 
            // lbTong2
            // 
            this.lbTong2.AutoSize = true;
            this.lbTong2.Location = new System.Drawing.Point(26, 537);
            this.lbTong2.Name = "lbTong2";
            this.lbTong2.Size = new System.Drawing.Size(0, 20);
            this.lbTong2.TabIndex = 106;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(334, 29);
            this.label6.TabIndex = 105;
            this.label6.Text = "Thống kê thuốc sắp hết hạn";
            // 
            // btInBaoCao2
            // 
            this.btInBaoCao2.Location = new System.Drawing.Point(465, 565);
            this.btInBaoCao2.Name = "btInBaoCao2";
            this.btInBaoCao2.Size = new System.Drawing.Size(108, 39);
            this.btInBaoCao2.TabIndex = 104;
            this.btInBaoCao2.Text = "In báo cáo";
            this.btInBaoCao2.UseVisualStyleBackColor = true;
            this.btInBaoCao2.Click += new System.EventHandler(this.btInBaoCao2_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(6, 136);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 28;
            this.dataGridView2.Size = new System.Drawing.Size(1034, 398);
            this.dataGridView2.TabIndex = 102;
            this.dataGridView2.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView2_RowPrePaint);
            // 
            // UserControlThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.Controls.Add(this.tabControl1);
            this.Name = "UserControlThongKe";
            this.Size = new System.Drawing.Size(1060, 649);
            this.Load += new System.EventHandler(this.UserControlThongKe_Load);
            this.tabControl1.ResumeLayout(false);
            this.tPDoanhThu.ResumeLayout(false);
            this.tPDoanhThu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoanhThu)).EndInit();
            this.tPThuoc.ResumeLayout(false);
            this.tPThuoc.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tPDoanhThu;
        private System.Windows.Forms.TabPage tPThuoc;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThu;
        private System.Windows.Forms.Button btInBaoCao;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btApDung;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rBNam;
        private System.Windows.Forms.RadioButton rBThang;
        private System.Windows.Forms.Label lbTong;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTong2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btInBaoCao2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btApDung2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label7;
    }
}
