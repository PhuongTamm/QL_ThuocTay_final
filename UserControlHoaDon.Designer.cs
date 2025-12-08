namespace QL_ThuocTay
{
    partial class UserControlHoaDon
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
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTienKH = new System.Windows.Forms.TextBox();
            this.lbTienThua = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btClearAll = new System.Windows.Forms.Button();
            this.btXoa = new System.Windows.Forms.Button();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbNguoiLap = new System.Windows.Forms.Label();
            this.lbTongTien = new System.Windows.Forms.Label();
            this.lbNgayLap = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btXemLT = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lbMaCN = new System.Windows.Forms.Label();
            this.lbSDTCN = new System.Windows.Forms.Label();
            this.lbTenThuoc = new System.Windows.Forms.Label();
            this.lbSLTon = new System.Windows.Forms.Label();
            this.lbDonGia = new System.Windows.Forms.Label();
            this.lbDVT = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbDiaChi = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btThem = new System.Windows.Forms.Button();
            this.txtSL = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btXem2 = new System.Windows.Forms.Button();
            this.txtTim = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btThanhToan = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 29);
            this.label1.TabIndex = 28;
            this.label1.Text = "Lập hóa đơn";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataGridView1.Location = new System.Drawing.Point(10, 277);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(526, 363);
            this.dataGridView1.TabIndex = 29;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTienKH);
            this.panel1.Controls.Add(this.lbTienThua);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.btClearAll);
            this.panel1.Controls.Add(this.btXoa);
            this.panel1.Controls.Add(this.txtSDT);
            this.panel1.Controls.Add(this.dataGridView2);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lbNguoiLap);
            this.panel1.Controls.Add(this.lbTongTien);
            this.panel1.Controls.Add(this.lbNgayLap);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(535, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(522, 597);
            this.panel1.TabIndex = 30;
            // 
            // txtTienKH
            // 
            this.txtTienKH.Location = new System.Drawing.Point(382, 530);
            this.txtTienKH.Name = "txtTienKH";
            this.txtTienKH.Size = new System.Drawing.Size(130, 26);
            this.txtTienKH.TabIndex = 10;
            this.txtTienKH.TextChanged += new System.EventHandler(this.txtTienKH_TextChanged);
            this.txtTienKH.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTienKH_KeyDown);
            // 
            // lbTienThua
            // 
            this.lbTienThua.AutoSize = true;
            this.lbTienThua.Location = new System.Drawing.Point(340, 568);
            this.lbTienThua.Name = "lbTienThua";
            this.lbTienThua.Size = new System.Drawing.Size(0, 20);
            this.lbTienThua.TabIndex = 9;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(255, 568);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 20);
            this.label16.TabIndex = 9;
            this.label16.Text = "Tiền thừa:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(255, 536);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 20);
            this.label14.TabIndex = 9;
            this.label14.Text = "Tiền khách đưa:";
            // 
            // btClearAll
            // 
            this.btClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btClearAll.Location = new System.Drawing.Point(413, 9);
            this.btClearAll.Name = "btClearAll";
            this.btClearAll.Size = new System.Drawing.Size(99, 38);
            this.btClearAll.TabIndex = 7;
            this.btClearAll.Text = "Clear All";
            this.btClearAll.UseVisualStyleBackColor = true;
            this.btClearAll.Click += new System.EventHandler(this.btClearAll_Click);
            // 
            // btXoa
            // 
            this.btXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btXoa.Image = global::QL_ThuocTay.Properties.Resources.icons8_bin_24;
            this.btXoa.Location = new System.Drawing.Point(357, 9);
            this.btXoa.Name = "btXoa";
            this.btXoa.Size = new System.Drawing.Size(50, 38);
            this.btXoa.TabIndex = 6;
            this.btXoa.UseVisualStyleBackColor = true;
            this.btXoa.Click += new System.EventHandler(this.btXoa_Click);
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(145, 500);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(148, 26);
            this.txtSDT.TabIndex = 5;
            this.txtSDT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKH_KeyDown);
            this.txtSDT.Leave += new System.EventHandler(this.txtSDT_Leave);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(7, 53);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 28;
            this.dataGridView2.Size = new System.Drawing.Size(512, 436);
            this.dataGridView2.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 506);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(132, 20);
            this.label12.TabIndex = 2;
            this.label12.Text = "SDT khách hàng:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 568);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Tổng tiền:";
            // 
            // lbNguoiLap
            // 
            this.lbNguoiLap.AutoSize = true;
            this.lbNguoiLap.Location = new System.Drawing.Point(446, 506);
            this.lbNguoiLap.Name = "lbNguoiLap";
            this.lbNguoiLap.Size = new System.Drawing.Size(0, 20);
            this.lbNguoiLap.TabIndex = 2;
            // 
            // lbTongTien
            // 
            this.lbTongTien.AutoSize = true;
            this.lbTongTien.Location = new System.Drawing.Point(92, 568);
            this.lbTongTien.Name = "lbTongTien";
            this.lbTongTien.Size = new System.Drawing.Size(0, 20);
            this.lbTongTien.TabIndex = 2;
            // 
            // lbNgayLap
            // 
            this.lbNgayLap.AutoSize = true;
            this.lbNgayLap.Location = new System.Drawing.Point(87, 536);
            this.lbNgayLap.Name = "lbNgayLap";
            this.lbNgayLap.Size = new System.Drawing.Size(0, 20);
            this.lbNgayLap.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 536);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 2;
            this.label13.Text = "Ngày lập:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 506);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Người lập hóa đơn:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Thông tin hóa đơn";
            // 
            // btXemLT
            // 
            this.btXemLT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btXemLT.Location = new System.Drawing.Point(458, 203);
            this.btXemLT.Name = "btXemLT";
            this.btXemLT.Size = new System.Drawing.Size(65, 35);
            this.btXemLT.TabIndex = 35;
            this.btXemLT.Text = "Xem";
            this.btXemLT.UseVisualStyleBackColor = true;
            this.btXemLT.Click += new System.EventHandler(this.btXemLT_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(268, 210);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(184, 20);
            this.label10.TabIndex = 34;
            this.label10.Text = "Xem danh sách lô thuốc:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(269, 106);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(183, 28);
            this.comboBox1.TabIndex = 31;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lbMaCN
            // 
            this.lbMaCN.AutoSize = true;
            this.lbMaCN.Location = new System.Drawing.Point(268, 146);
            this.lbMaCN.Name = "lbMaCN";
            this.lbMaCN.Size = new System.Drawing.Size(0, 20);
            this.lbMaCN.TabIndex = 1;
            // 
            // lbSDTCN
            // 
            this.lbSDTCN.AutoSize = true;
            this.lbSDTCN.Location = new System.Drawing.Point(377, 146);
            this.lbSDTCN.Name = "lbSDTCN";
            this.lbSDTCN.Size = new System.Drawing.Size(0, 20);
            this.lbSDTCN.TabIndex = 1;
            // 
            // lbTenThuoc
            // 
            this.lbTenThuoc.AutoSize = true;
            this.lbTenThuoc.Location = new System.Drawing.Point(104, 83);
            this.lbTenThuoc.Name = "lbTenThuoc";
            this.lbTenThuoc.Size = new System.Drawing.Size(0, 20);
            this.lbTenThuoc.TabIndex = 1;
            // 
            // lbSLTon
            // 
            this.lbSLTon.AutoSize = true;
            this.lbSLTon.Location = new System.Drawing.Point(123, 114);
            this.lbSLTon.Name = "lbSLTon";
            this.lbSLTon.Size = new System.Drawing.Size(0, 20);
            this.lbSLTon.TabIndex = 1;
            // 
            // lbDonGia
            // 
            this.lbDonGia.AutoSize = true;
            this.lbDonGia.Location = new System.Drawing.Point(88, 176);
            this.lbDonGia.Name = "lbDonGia";
            this.lbDonGia.Size = new System.Drawing.Size(0, 20);
            this.lbDonGia.TabIndex = 1;
            // 
            // lbDVT
            // 
            this.lbDVT.AutoSize = true;
            this.lbDVT.Location = new System.Drawing.Point(107, 146);
            this.lbDVT.Name = "lbDVT";
            this.lbDVT.Size = new System.Drawing.Size(0, 20);
            this.lbDVT.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 32;
            this.label5.Text = "Tên thuốc:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 20);
            this.label7.TabIndex = 32;
            this.label7.Text = "Số lượng tồn:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 20);
            this.label8.TabIndex = 32;
            this.label8.Text = "Đơn giá:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 146);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 20);
            this.label9.TabIndex = 32;
            this.label9.Text = "Đơn vị tính:";
            // 
            // lbDiaChi
            // 
            this.lbDiaChi.AutoSize = true;
            this.lbDiaChi.Location = new System.Drawing.Point(268, 176);
            this.lbDiaChi.Name = "lbDiaChi";
            this.lbDiaChi.Size = new System.Drawing.Size(0, 20);
            this.lbDiaChi.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btThem);
            this.groupBox1.Controls.Add(this.txtSL);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Location = new System.Drawing.Point(10, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 61);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thêm vào hóa đơn";
            // 
            // btThem
            // 
            this.btThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btThem.Location = new System.Drawing.Point(142, 19);
            this.btThem.Name = "btThem";
            this.btThem.Size = new System.Drawing.Size(67, 32);
            this.btThem.TabIndex = 36;
            this.btThem.Text = "Thêm";
            this.btThem.UseVisualStyleBackColor = true;
            this.btThem.Click += new System.EventHandler(this.btThem_Click);
            // 
            // txtSL
            // 
            this.txtSL.Location = new System.Drawing.Point(88, 25);
            this.txtSL.Name = "txtSL";
            this.txtSL.Size = new System.Drawing.Size(48, 26);
            this.txtSL.TabIndex = 39;
            this.txtSL.TextChanged += new System.EventHandler(this.txtSL_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 31);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 20);
            this.label15.TabIndex = 38;
            this.label15.Text = "Số lượng:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(266, 244);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(186, 20);
            this.label11.TabIndex = 34;
            this.label11.Text = "Xem danh sách hóa đơn:";
            // 
            // btXem2
            // 
            this.btXem2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btXem2.Location = new System.Drawing.Point(458, 238);
            this.btXem2.Name = "btXem2";
            this.btXem2.Size = new System.Drawing.Size(65, 33);
            this.btXem2.TabIndex = 36;
            this.btXem2.Text = "Xem";
            this.btXem2.UseVisualStyleBackColor = true;
            this.btXem2.Click += new System.EventHandler(this.btXem2_Click);
            // 
            // txtTim
            // 
            this.txtTim.Location = new System.Drawing.Point(340, 45);
            this.txtTim.Name = "txtTim";
            this.txtTim.Size = new System.Drawing.Size(183, 26);
            this.txtTim.TabIndex = 11;
            this.txtTim.TextChanged += new System.EventHandler(this.txtTim_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(252, 51);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 20);
            this.label17.TabIndex = 37;
            this.label17.Text = "Tìm thuốc:";
            // 
            // btThanhToan
            // 
            this.btThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btThanhToan.Location = new System.Drawing.Point(750, 606);
            this.btThanhToan.Name = "btThanhToan";
            this.btThanhToan.Size = new System.Drawing.Size(119, 34);
            this.btThanhToan.TabIndex = 38;
            this.btThanhToan.Text = "Thanh Toán";
            this.btThanhToan.UseVisualStyleBackColor = true;
            this.btThanhToan.Click += new System.EventHandler(this.btThanhToan_Click);
            // 
            // UserControlHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.Controls.Add(this.btThanhToan);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtTim);
            this.Controls.Add(this.btXem2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btXemLT);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbDiaChi);
            this.Controls.Add(this.lbSDTCN);
            this.Controls.Add(this.lbDVT);
            this.Controls.Add(this.lbDonGia);
            this.Controls.Add(this.lbSLTon);
            this.Controls.Add(this.lbTenThuoc);
            this.Controls.Add(this.lbMaCN);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "UserControlHoaDon";
            this.Size = new System.Drawing.Size(1060, 649);
            this.Load += new System.EventHandler(this.UserControlHoaDon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lbMaCN;
        private System.Windows.Forms.Label lbSDTCN;
        private System.Windows.Forms.Label lbTenThuoc;
        private System.Windows.Forms.Label lbSLTon;
        private System.Windows.Forms.Label lbDonGia;
        private System.Windows.Forms.Label lbDVT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbDiaChi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btXemLT;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbNguoiLap;
        private System.Windows.Forms.Label lbTongTien;
        private System.Windows.Forms.Label lbNgayLap;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Button btXoa;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btThem;
        private System.Windows.Forms.TextBox txtSL;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btClearAll;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btXem2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbTienThua;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtTienKH;
        private System.Windows.Forms.TextBox txtTim;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btThanhToan;
    }
}
