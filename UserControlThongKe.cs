using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace QL_ThuocTay
{
    public partial class UserControlThongKe : UserControl
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();

        public UserControlThongKe()
        {
            InitializeComponent();
        }

        private void UserControlThongKe_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);   
            LoadChiNhanh();
            SetupChart();
            SetupDateTimePicker();
        }

        private void LoadChiNhanh()
        {
            // Thêm các chi nhánh cố định
            comboBox1.Items.Add("Kho tổng");
            comboBox1.Items.Add("CN01");
            comboBox1.Items.Add("CN02");
            comboBox1.Items.Add("CN03");
            comboBox1.SelectedIndex = 0;

            // Thêm các chi nhánh cố định
            comboBox2.Items.Add("Kho tổng");
            comboBox2.Items.Add("CN01");
            comboBox2.Items.Add("CN02");
            comboBox2.Items.Add("CN03");
            comboBox2.SelectedIndex = 0;
        }
        private void SetupDateTimePicker()
        {
            // Mặc định hiển thị tháng/năm hiện tại
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void SetupChart()
        {
            chartDoanhThu.Series.Clear();
            chartDoanhThu.Titles.Clear();

            // Thêm series cho biểu đồ
            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SteelBlue;
            chartDoanhThu.Series.Add(series);

            // Đặt tiêu đề
            chartDoanhThu.Titles.Add("BIỂU ĐỒ DOANH THU");
            //chartDoanhThu.Titles[0].Font = new Font("Arial", 12);
            //, FontStyle.Bold

            // Định dạng trục
            chartDoanhThu.ChartAreas[0].AxisX.Title = "Thời gian";
            chartDoanhThu.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
            chartDoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";
        }

        private void rBThang_CheckedChanged(object sender, EventArgs e)
        {
            if (rBThang.Checked)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "MM/yyyy";
                dateTimePicker1.ShowUpDown = true;
            }
        }

        private void tPDoanhThu_Click(object sender, EventArgs e)
        {

        }

        private void rBNam_CheckedChanged(object sender, EventArgs e)
        {
            if (rBNam.Checked)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy";
                dateTimePicker1.ShowUpDown = true;
            }
        }

        private void btApDung_Click(object sender, EventArgs e)
        {
            try
            {
                ThongKeDoanhThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThongKeDoanhThu()
        {
            // Đảm bảo kết nối đang mở
            if (conn.State == ConnectionState.Closed)
            {
                cn.ketnoi(conn);
            }
            string chiNhanh = comboBox1.SelectedItem.ToString();
            bool isTheoThang = rBThang.Checked;
            int thang = dateTimePicker1.Value.Month;
            int nam = dateTimePicker1.Value.Year;
            DataTable data = new DataTable();
            if (isTheoThang)
            {
                // Thống kê theo từng ngày trong tháng
                data = GetDoanhThuTheoNgay(thang, nam, chiNhanh);
                HienThiBieuDoTheoNgay(data);
            }
            else
            {
                // Thống kê theo từng tháng trong năm
                data = GetDoanhThuTheoThang(nam, chiNhanh);
                HienThiBieuDoTheoThang(data);
            }
            dataGridView1.DataSource = data; // Hiển thị lên DataGridView
            TinhTongDoanhThu(data); // Tính tổng
        }

        private DataTable GetDoanhThuTheoNgay(int thang, int nam, string chiNhanh)
        {
            DataTable data = new DataTable();
            data.Columns.Add("Ngày", typeof(int));
            data.Columns.Add("Doanh thu", typeof(decimal));
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "";

                if (chiNhanh == "Kho tổng")
                {
                    query = @"
                        SELECT DAY(h.HD_NGAYLAP) AS Ngay, 
                               ISNULL(SUM(ct.CTHD_SOLUONG * ct.CTHD_DONGIA), 0) AS DoanhThu
                        FROM HOADON h
                        INNER JOIN CTHD ct ON h.HD_MA = ct.HD_MA
                        WHERE MONTH(h.HD_NGAYLAP) = @Thang 
                          AND YEAR(h.HD_NGAYLAP) = @Nam
                        GROUP BY DAY(h.HD_NGAYLAP)
                        ORDER BY Ngay";
                }
                else
                {
                    query = @"
                        SELECT DAY(h.HD_NGAYLAP) AS Ngay, 
                               ISNULL(SUM(ct.CTHD_SOLUONG * ct.CTHD_DONGIA), 0) AS DoanhThu
                        FROM HOADON h
                        INNER JOIN CTHD ct ON h.HD_MA = ct.HD_MA
                        WHERE MONTH(h.HD_NGAYLAP) = @Thang 
                          AND YEAR(h.HD_NGAYLAP) = @Nam
                          AND h.CN_MA = @CN_MA
                        GROUP BY DAY(h.HD_NGAYLAP)
                        ORDER BY Ngay";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    if (chiNhanh != "Kho tổng")
                    {
                        cmd.Parameters.AddWithValue("@CN_MA", chiNhanh);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable tempTable = new DataTable();
                        adapter.Fill(tempTable);
                        // Chuyển đổi sang DataTable theo cấu trúc mong muốn
                        foreach (DataRow row in tempTable.Rows)
                        {
                            DataRow newRow = data.NewRow();
                            newRow["Ngày"] = row["Ngay"];
                            newRow["Doanh thu"] = row["DoanhThu"];
                            data.Rows.Add(newRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return data;
        }

        private DataTable GetDoanhThuTheoThang(int nam, string chiNhanh)
        {
            DataTable data = new DataTable();
            data.Columns.Add("Tháng", typeof(int));
            data.Columns.Add("Doanh thu", typeof(decimal));
            try
            {
                // Đảm bảo kết nối
                if (conn.State == ConnectionState.Closed)
                {
                    cn.ketnoi(conn);
                }

                string query = "";

                if (chiNhanh == "Kho tổng")
                {
                    query = @"
                        SELECT MONTH(h.HD_NGAYLAP) AS Thang, 
                               ISNULL(SUM(ct.CTHD_SOLUONG * ct.CTHD_DONGIA), 0) AS DoanhThu
                        FROM HOADON h
                        INNER JOIN CTHD ct ON h.HD_MA = ct.HD_MA
                        WHERE YEAR(h.HD_NGAYLAP) = @Nam
                        GROUP BY MONTH(h.HD_NGAYLAP)
                        ORDER BY Thang";
                }
                else
                {
                    query = @"
                        SELECT MONTH(h.HD_NGAYLAP) AS Thang, 
                               ISNULL(SUM(ct.CTHD_SOLUONG * ct.CTHD_DONGIA), 0) AS DoanhThu
                        FROM HOADON h
                        INNER JOIN CTHD ct ON h.HD_MA = ct.HD_MA
                        WHERE YEAR(h.HD_NGAYLAP) = @Nam
                          AND h.CN_MA = @CN_MA
                        GROUP BY MONTH(h.HD_NGAYLAP)
                        ORDER BY Thang";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    if (chiNhanh != "Kho tổng")
                    {
                        cmd.Parameters.AddWithValue("@CN_MA", chiNhanh);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable tempTable = new DataTable();
                        adapter.Fill(tempTable);

                        // Chuyển đổi sang DataTable theo cấu trúc mong muốn
                        foreach (DataRow row in tempTable.Rows)
                        {
                            DataRow newRow = data.NewRow();
                            newRow["Tháng"] = row["Thang"];
                            newRow["Doanh thu"] = row["DoanhThu"];
                            data.Rows.Add(newRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return data;
        }

        private void HienThiBieuDoTheoNgay(DataTable data)
        {
            chartDoanhThu.Series[0].Points.Clear();
            chartDoanhThu.ChartAreas[0].AxisX.Title = "Ngày";

            // Hiển thị tất cả các ngày trong tháng (kể cả không có doanh thu)
            int soNgayTrongThang = DateTime.DaysInMonth(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month);

            for (int ngay = 1; ngay <= soNgayTrongThang; ngay++)
            {
                decimal doanhThu = 0;

                // Tìm doanh thu cho ngày này
                foreach (DataRow row in data.Rows)
                {
                    if (Convert.ToInt32(row["Ngày"]) == ngay)
                    {
                        doanhThu = Convert.ToDecimal(row["Doanh thu"]);
                        break;
                    }
                }

                chartDoanhThu.Series[0].Points.AddXY(ngay, doanhThu);
            }

            // Đặt tiêu đề biểu đồ
            string chiNhanh = comboBox1.SelectedItem.ToString();
            int thang = dateTimePicker1.Value.Month;
            int nam = dateTimePicker1.Value.Year;
            chartDoanhThu.Titles[0].Text = $"DOANH THU {chiNhanh} - THÁNG {thang}/{nam}";
        }

        private void HienThiBieuDoTheoThang(DataTable data)
        {
            chartDoanhThu.Series[0].Points.Clear();
            chartDoanhThu.ChartAreas[0].AxisX.Title = "Tháng";

            // Hiển thị tất cả 12 tháng (kể cả không có doanh thu)
            for (int thang = 1; thang <= 12; thang++)
            {
                decimal doanhThu = 0;

                // Tìm doanh thu cho tháng này
                foreach (DataRow row in data.Rows)
                {
                    if (Convert.ToInt32(row["Tháng"]) == thang)
                    {
                        doanhThu = Convert.ToDecimal(row["Doanh thu"]);
                        break;
                    }
                }

                chartDoanhThu.Series[0].Points.AddXY($"Tháng {thang}", doanhThu);
            }

            // Đặt tiêu đề biểu đồ
            string chiNhanh = comboBox1.SelectedItem.ToString();
            int nam = dateTimePicker1.Value.Year;
            chartDoanhThu.Titles[0].Text = $"DOANH THU {chiNhanh} - NĂM {nam}";
        }

        private void TinhTongDoanhThu(DataTable data)
        {
            decimal tong = 0;
            foreach (DataRow row in data.Rows)
            {
                tong += Convert.ToDecimal(row["Doanh thu"]);
            }
            lbTong.Text = $"Tổng cộng: {tong.ToString("#,##0")} VNĐ";
        }

        private void btInBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string chiNhanh = comboBox1.SelectedItem.ToString();
                    string loaiThongKe = rBThang.Checked ? "Theo tháng" : "Theo năm";
                    string thoiGian = rBThang.Checked ?
                        $"Tháng {dateTimePicker1.Value.Month}/{dateTimePicker1.Value.Year}" :
                        $"Năm {dateTimePicker1.Value.Year}";

                    decimal tongDoanhThu = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && row.Cells[1].Value != null)
                        {
                            tongDoanhThu += Convert.ToDecimal(row.Cells[1].Value);
                        }
                    }

                    // Tạo PDF 
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 20, 20, 30, 30);
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    doc.Open();

                    // Font Unicode 
                    string fontPath = @"C:\Windows\Fonts\arial.ttf";
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                    // Tiêu đề
                    var paraTitle = new iTextSharp.text.Paragraph("BÁO CÁO DOANH THU", fontTitle);
                    paraTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    paraTitle.SpacingAfter = 20f;
                    doc.Add(paraTitle);

                    // Thông tin báo cáo
                    doc.Add(new iTextSharp.text.Paragraph($"Kho: {chiNhanh}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Loại thống kê: {loaiThongKe}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Thời gian: {thoiGian}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Bảng dữ liệu
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1, 2 });

                    // Header bảng - DÙNG fontBold, KHÔNG CẦN fontHeader
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(rBThang.Checked ? "Ngày" : "Tháng", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Doanh thu (VNĐ)", fontBold)));

                    // Dữ liệu
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && row.Cells[0].Value != null && row.Cells[1].Value != null)
                        {
                            string ngayThang = row.Cells[0].Value.ToString();
                            decimal doanhThu = Convert.ToDecimal(row.Cells[1].Value);

                            table.AddCell(new iTextSharp.text.Phrase(ngayThang, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(doanhThu.ToString("#,##0"), fontNormal));
                        }
                    }

                    doc.Add(table);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Tổng cộng 
                    var paraTongCong = new iTextSharp.text.Paragraph($"Tổng cộng: {tongDoanhThu.ToString("#,##0")} VNĐ", fontBold);
                    paraTongCong.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    doc.Add(paraTongCong);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Kết thúc báo cáo
                    var paraKetThuc = new iTextSharp.text.Paragraph("--- HẾT BÁO CÁO ---", fontSmall);
                    paraKetThuc.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    paraKetThuc.SpacingBefore = 20f;
                    doc.Add(paraKetThuc);

                    doc.Close();
                    writer.Close();

                    MessageBox.Show($"Đã xuất báo cáo thành công!\nFile: {Path.GetFileName(saveFileDialog.FileName)}",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /////////////////Thống kê thuốc sắp hết hạn và đã hết hạn//////////////////

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedItem != null)
                {
                    LoadThuocSapVaHetHan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThuocSapVaHetHan()
        {
            try
            {
                string chiNhanh = comboBox2.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(chiNhanh))
                {
                    return;
                }

                DataTable data = GetThuocSapVaHetHan(chiNhanh);
                dataGridView2.DataSource = data;

                if (data.Rows.Count > 0)
                {
                    int soLoHetHan = 0;
                    int soLoSapHetHan = 0;
                    int tongSoLuongHetHan = 0;
                    int tongSoLuongSapHetHan = 0;

                    foreach (DataRow row in data.Rows)
                    {
                        string trangThai = row["Trạng thái"].ToString();
                        int soLuong = Convert.ToInt32(row["Số lượng"]);

                        if (trangThai == "Đã hết hạn")
                        {
                            soLoHetHan++;
                            tongSoLuongHetHan += soLuong;
                        }
                        else if (trangThai == "Sắp hết hạn")
                        {
                            soLoSapHetHan++;
                            tongSoLuongSapHetHan += soLuong;
                        }
                    }
                }
                else
                {
                    lbTong2.Text = "Không có thuốc sắp hết hạn hoặc đã hết hạn";
                    lbTong2.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thuốc sắp/đã hết hạn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thống kê thuốc sắp hết hạn (dưới 30 ngày) và đã hết hạn
        private DataTable GetThuocSapVaHetHan(string chiNhanh)
        {
            DataTable data = new DataTable();

            try
            {
                // Đảm bảo kết nối mở
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "";
                if (chiNhanh == "Kho tổng")
                {
                    // Lấy thuốc sắp hết hạn và đã hết hạn trong kho tổng (từ bảng LOTHUOC)
                    query = @"
                        SELECT 
                            lt.LT_MA AS [Mã lô],
                            t.T_MA AS [Mã thuốc],
                            t.T_TEN AS [Tên thuốc],
                            lt.LT_SLTON AS [Số lượng],
                            lt.LT_NGAYHD AS [Hạn sử dụng],
                            DATEDIFF(DAY, GETDATE(), lt.LT_NGAYHD) AS [Còn lại (ngày)],
                            N'Kho tổng' AS [Chi nhánh],
                            CASE 
                                WHEN lt.LT_NGAYHD < GETDATE() THEN N'Đã hết hạn'
                                WHEN lt.LT_NGAYHD BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE()) THEN N'Sắp hết hạn'
                            END AS [Trạng thái]
                        FROM LOTHUOC lt
                        INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                        WHERE lt.LT_SLTON > 0 
                          AND (lt.LT_NGAYHD < GETDATE() 
                               OR lt.LT_NGAYHD BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE()))
                        ORDER BY 
                            CASE 
                                WHEN lt.LT_NGAYHD < GETDATE() THEN 1
                                WHEN lt.LT_NGAYHD BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE()) THEN 2
                                ELSE 3
                            END,
                            lt.LT_NGAYHD ASC";
                }
                else
                {
                    // Lấy thuốc sắp hết hạn và đã hết hạn trong chi nhánh (từ bảng CTPX)
                    query = @"
                        SELECT 
                            lt.LT_MA AS [Mã lô],
                            t.T_MA AS [Mã thuốc],
                            t.T_TEN AS [Tên thuốc],
                            ctpx.CTPX_SLTON AS [Số lượng],
                            lt.LT_NGAYHD AS [Hạn sử dụng],
                            DATEDIFF(DAY, GETDATE(), lt.LT_NGAYHD) AS [Còn lại (ngày)],
                            px.CN_MA AS [Chi nhánh],
                            CASE 
                                WHEN lt.LT_NGAYHD < GETDATE() THEN N'Đã hết hạn'
                                WHEN lt.LT_NGAYHD BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE()) THEN N'Sắp hết hạn'
                            END AS [Trạng thái]
                        FROM LOTHUOC lt
                        INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                        INNER JOIN CTPX ctpx ON lt.LT_MA = ctpx.LT_MA AND lt.T_MA = ctpx.T_MA
                        INNER JOIN PHIEUXUAT px ON ctpx.PX_MA = px.PX_MA
                        WHERE px.CN_MA = @CN_MA
                          AND ctpx.CTPX_SLTON > 0 
                          AND (lt.LT_NGAYHD < GETDATE() 
                               OR lt.LT_NGAYHD BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE()))
                        ORDER BY 
                            CASE 
                                WHEN lt.LT_NGAYHD < GETDATE() THEN 1
                                WHEN lt.LT_NGAYHD BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE()) THEN 2
                                ELSE 3
                            END,
                            lt.LT_NGAYHD ASC";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (chiNhanh != "Kho tổng")
                    {
                        cmd.Parameters.AddWithValue("@CN_MA", chiNhanh);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn thuốc sắp/đã hết hạn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return data;
        }


        private void btApDung2_Click(object sender, EventArgs e)
        {
            try
            {
                LoadThuocSapVaHetHan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            if (row.Cells["Trạng thái"].Value != null)
            {
                string trangThai = row.Cells["Trạng thái"].Value.ToString();
                // Chỉ tô màu ô "Trạng thái"
                if (trangThai == "Đã hết hạn")
                {
                    row.Cells["Trạng thái"].Style.ForeColor = Color.DarkRed;
                }
                else if (trangThai == "Sắp hết hạn")
                {
                    row.Cells["Trạng thái"].Style.ForeColor = Color.DarkOrange;
                }
            }
        }

        private void btInBaoCao2_Click(object sender, EventArgs e)
        {
            try
            {
                //if (dataGridView2.Rows.Count == 1)
                //{
                //    MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"BaoCaoThuocHetHan_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string chiNhanh = comboBox2.SelectedItem?.ToString() ?? "Không xác định";

                    // Tính thống kê
                    int soLoHetHan = 0;
                    int soLoSapHetHan = 0;
                    int tongSoLuongHetHan = 0;
                    int tongSoLuongSapHetHan = 0;

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["Trạng thái"].Value != null && row.Cells["Số lượng"].Value != null)
                        {
                            string trangThai = row.Cells["Trạng thái"].Value.ToString();
                            int soLuong = Convert.ToInt32(row.Cells["Số lượng"].Value);

                            if (trangThai == "Đã hết hạn")
                            {
                                soLoHetHan++;
                                tongSoLuongHetHan += soLuong;
                            }
                            else if (trangThai == "Sắp hết hạn")
                            {
                                soLoSapHetHan++;
                                tongSoLuongSapHetHan += soLuong;
                            }
                        }
                    }

                    // Tạo PDF
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 20, 20, 30, 30);
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    doc.Open();

                    // Font Unicode (tiếng Việt)
                    string fontPath = @"C:\Windows\Fonts\arial.ttf";
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontRed = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
                    iTextSharp.text.Font fontOrange = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(255, 140, 0)); // Màu cam

                    // Tiêu đề
                    var paraTitle = new iTextSharp.text.Paragraph("BÁO CÁO THUỐC SẮP/ĐÃ HẾT HẠN", fontTitle);
                    paraTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    paraTitle.SpacingAfter = 20f;
                    doc.Add(paraTitle);

                    // Thông tin báo cáo
                    doc.Add(new iTextSharp.text.Paragraph($"Kho: {chiNhanh}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNormal));

                    // Thống kê
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);
                    doc.Add(new iTextSharp.text.Paragraph("THỐNG KÊ TỔNG QUAN", fontBold));

                    string thongKeText = "";
                    if (soLoHetHan > 0)
                    {
                        thongKeText += $"ĐÃ HẾT HẠN: {soLoHetHan} loại ({tongSoLuongHetHan} sản phẩm)\n";
                    }
                    if (soLoSapHetHan > 0)
                    {
                        thongKeText += $"SẮP HẾT HẠN: {soLoSapHetHan} loại ({tongSoLuongSapHetHan} sản phẩm)\n";
                    }

                    if (string.IsNullOrEmpty(thongKeText))
                    {
                        thongKeText = "Không có thuốc sắp hoặc đã hết hạn";
                    }

                    doc.Add(new iTextSharp.text.Paragraph(thongKeText, fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Bảng dữ liệu
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(7);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1.2f, 1, 2, 0.8f, 1, 1, 1.2f }); // Điều chỉnh tỷ lệ cột

                    // Header bảng
                    table.AddCell(new iTextSharp.text.Phrase("Mã lô", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Mã thuốc", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Tên thuốc", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Số lượng", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Hạn sử dụng", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Còn lại (ngày)", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Trạng thái", fontBold));

                    // Dữ liệu
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (!row.IsNewRow && row.Cells[0].Value != null)
                        {
                            // Lấy giá trị từ các cột
                            string maLo = row.Cells["Mã lô"].Value?.ToString() ?? "";
                            string maThuoc = row.Cells["Mã thuốc"].Value?.ToString() ?? "";
                            string tenThuoc = row.Cells["Tên thuốc"].Value?.ToString() ?? "";
                            string soLuong = row.Cells["Số lượng"].Value?.ToString() ?? "0";
                            string hanSuDung = row.Cells["Hạn sử dụng"].Value != null ?
                                Convert.ToDateTime(row.Cells["Hạn sử dụng"].Value).ToString("dd/MM/yyyy") : "";
                            string conLaiNgay = row.Cells["Còn lại (ngày)"].Value?.ToString() ?? "";
                            string trangThai = row.Cells["Trạng thái"].Value?.ToString() ?? "";

                            // Thêm các ô vào bảng
                            table.AddCell(new iTextSharp.text.Phrase(maLo, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(maThuoc, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(tenThuoc, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(soLuong, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(hanSuDung, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(conLaiNgay, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(trangThai, fontNormal));
                        }
                    }

                    doc.Add(table);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Chú thích
                    var paraChuThich = new iTextSharp.text.Paragraph("CHÚ THÍCH:", fontBold);
                    paraChuThich.SpacingBefore = 10f;
                    doc.Add(paraChuThich);

                    iTextSharp.text.List chuThichList = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED);
                    chuThichList.Add(new iTextSharp.text.ListItem("ĐÃ HẾT HẠN: Hạn sử dụng đã qua ngày hiện tại", fontNormal));
                    chuThichList.Add(new iTextSharp.text.ListItem("SẮP HẾT HẠN: Hạn sử dụng trong vòng 30 ngày tới", fontNormal));
                    doc.Add(chuThichList);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Kết thúc báo cáo
                    var paraKetThuc = new iTextSharp.text.Paragraph("--- HẾT BÁO CÁO ---", fontSmall);
                    paraKetThuc.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    paraKetThuc.SpacingBefore = 20f;
                    doc.Add(paraKetThuc);

                    doc.Close();
                    writer.Close();

                    MessageBox.Show($"Đã xuất báo cáo thành công!\nFile: {Path.GetFileName(saveFileDialog.FileName)}",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file PDF sau khi xuất
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
