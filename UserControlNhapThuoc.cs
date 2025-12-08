using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_ThuocTay
{
    public partial class UserControlNhapThuoc : UserControl
    {
        SqlConnection conn = new SqlConnection();
        string ma;
        ChucNang cn = new ChucNang();
        public UserControlNhapThuoc(string maNV)
        {
            InitializeComponent();
            ma = maNV;
        }

        private void UserControlNhapThuoc_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadThuoc();    

            lbNgayNhap.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            lbNguoiNhap.Text = ma;
            dtpNgaySX.Value = DateTime.Now;
            //dtpHSD.Value = DateTime.Now;
            dtpHSD.Value = DateTime.Now.AddYears(2);

            // Tạo columns cho dataGridView2 phiếu nhập
            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("MaLo", "Mã lô");
            dataGridView2.Columns.Add("MaThuoc", "Mã thuốc");
            dataGridView2.Columns.Add("TenThuoc", "Tên thuốc");
            dataGridView2.Columns.Add("NgaySX", "Ngày SX");
            dataGridView2.Columns.Add("HanSD", "Hạn SD");
            dataGridView2.Columns.Add("SoLuongNhap", "Số lượng nhập");
            dataGridView2.Columns.Add("DonGiaNhap", "Đơn giá nhập");
            dataGridView2.Columns.Add("DonViTinh", "Đơn vị tính");
            dataGridView2.Columns.Add("ThanhTien", "Thành tiền");
        }

        private void LoadThuoc(string keyword = "")
        {
            string sql = @"
                SELECT THUOC.T_MA AS [Mã thuốc],
                    THUOC.T_TEN As [Tên thuốc],
                    THUOC.T_SLTON as [Số lượng tồn],
                    THUOC.T_THANHPHAN as [Thành phần],
                    DONVITINH.DVT_TEN as [Đơn vị tính],
                    ISNULL(DG.DG_DONGIA, 0) as [Đơn giá bán]
                 FROM THUOC
                 INNER JOIN DONVITINH ON THUOC.DVT_MA = DONVITINH.DVT_MA
                 OUTER APPLY (
                    SELECT TOP 1 DG_DONGIA 
                    FROM DONGIA 
                    WHERE T_MA = THUOC.T_MA 
                    ORDER BY NGAY DESC, THOIGIAN DESC
                 ) AS DG
                 WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (THUOC.T_MA LIKE N'%" + keyword + "%' OR THUOC.T_TEN LIKE N'%" + keyword + "%')";
            }
            sql += " ORDER BY THUOC.T_MA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            LoadThuoc(keyword);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                lbMaThuoc.Text = row.Cells["Mã thuốc"].Value.ToString();
                lbTenThuoc.Text = row.Cells["Tên thuốc"].Value.ToString();
                lbSLTon.Text = row.Cells["Số lượng tồn"].Value?.ToString() ?? "0";  
                lbThanhPhan.Text = row.Cells["Thành phần"].Value?.ToString() ?? "";
                lbDVT.Text = row.Cells["Đơn vị tính"].Value?.ToString() ?? "";
                decimal donGia = Convert.ToDecimal(row.Cells["Đơn giá bán"].Value);
                lbDonGia.Text = donGia > 0 ? donGia.ToString("N0") + " VNĐ" : "Chưa có giá";
            }
            txtSLNhap.Focus();
            txtTim.Clear();
        }

        // Kiểm tra HSD > Ngày SX
        private void dtpHSD_ValueChanged(object sender, EventArgs e)
        {
            if (dtpHSD.Value <= dtpNgaySX.Value)
            {
                MessageBox.Show("Hạn sử dụng phải lớn hơn ngày sản xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpHSD.Value = dtpNgaySX.Value.AddDays(1);
            }
        }

        // hàm tính tổng tiền
        private void TinhTongTien()
        {
            decimal tong = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                    tong += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
            }
            lbTongTien.Text = tong.ToString("N0") + " VNĐ";
        }

        private void btThemThuoc_Click(object sender, EventArgs e)
        {
            FormThemThuoc f = new FormThemThuoc();
            f.ShowDialog();
            LoadThuoc();    //load datarid
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra các điều kiện
            if (string.IsNullOrEmpty(lbTenThuoc.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn thuốc cần nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtSLNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSLNhap.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtDonGiaNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }

            // Parse lại để lấy giá trị (đã validate ở Leave event)
            int soLuongNhap = int.Parse(txtSLNhap.Text);
            decimal donGiaNhap = decimal.Parse(txtDonGiaNhap.Text);

            string maLo = TaoMaLoMoi(); // Tạo mã lô tự động
            decimal thanhTien = soLuongNhap * donGiaNhap; // Tính thành tiền

            // Thêm vào dataGridView2 (phiếu nhập)
            dataGridView2.Rows.Add(
                maLo,
                lbMaThuoc.Text,
                lbTenThuoc.Text,
                dtpNgaySX.Value.ToString("MM/dd/yyyy"),
                dtpHSD.Value.ToString("MM/dd/yyyy"),
                soLuongNhap,
                donGiaNhap.ToString("N0"),
                lbDVT.Text,
                //comboBox1.Text,
                thanhTien.ToString("N0")
            );

            TinhTongTien();// Tính tổng tiền
            ClearFormNhap();// Reset form nhập
        }

        private string TaoMaLoMoi()
        {
            string maThuoc = lbMaThuoc.Text.Trim();
            List<int> danhSachSTT = new List<int>();

            string sql = $"SELECT MAX(LT_MA) FROM LOTHUOC WHERE T_MA = @MaThuoc";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string maxMaLo = result.ToString().Trim();
                        if (maxMaLo.StartsWith(maThuoc) && maxMaLo.Length > maThuoc.Length)
                        {
                            string soThuTu = maxMaLo.Substring(maThuoc.Length).Trim();
                            if (int.TryParse(soThuTu, out int sttDb))
                            {
                                danhSachSTT.Add(sttDb);
                            }
                        }
                    }
                }

                // Lấy tất cả mã lô từ dataGridView2
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["MaLo"].Value != null && !string.IsNullOrEmpty(row.Cells["MaLo"].Value.ToString()))
                    {
                        string maLo = row.Cells["MaLo"].Value.ToString().Trim();

                        if (maLo.StartsWith(maThuoc) && maLo.Length > maThuoc.Length)
                        {
                            string soThuTu = maLo.Substring(maThuoc.Length).Trim();
                            if (int.TryParse(soThuTu, out int sttGrid))
                            {
                                danhSachSTT.Add(sttGrid);
                            }
                        }
                    }
                }

                // Tìm số thứ tự lớn nhất và tạo mã mới
                int maxStt = danhSachSTT.Count > 0 ? danhSachSTT.Max() : 0;
                maxStt++; // Tăng lên 1 để tạo mã mới

                return maThuoc + maxStt.ToString("D4");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DEBUG - Lỗi: {ex.Message}");
                MessageBox.Show($"Lỗi khi tạo mã lô: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        //Hàm reset form nhập
        private void ClearFormNhap()
        {
            txtSLNhap.Clear();
            txtDonGiaNhap.Clear();
            LoadThuoc();
        }

        private void txtSLNhap_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSLNhap.Text))
            {
                if (!int.TryParse(txtSLNhap.Text, out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSLNhap.Clear();
                    txtSLNhap.Focus();
                }
            }
        }

        private void txtDonGiaNhap_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDonGiaNhap.Text))
            {
                if (!decimal.TryParse(txtDonGiaNhap.Text, out decimal donGia) || donGia <= 0)
                {
                    MessageBox.Show("Đơn giá nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDonGiaNhap.Clear();
                    txtDonGiaNhap.Focus();
                }
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
                TinhTongTien();
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            TinhTongTien();
        }

        // Hàm tạo mã phiếu nhập mới
        private string TaoMaPhieuNhapMoi()
        {
            string sql = "SELECT MAX(PN_MA) FROM PHIEUNHAP WHERE PN_MA LIKE 'PN%'";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string maxMa = result.ToString();
                    int number = int.Parse(maxMa.Substring(2)) + 1;
                    return "PN" + number.ToString("D3"); // PN001, PN002
                }
                else
                {
                    return "PN001";
                }
            }
        }

        private void btNhapThuoc_Click(object sender, EventArgs e)
        {
            //Kiểm tra có lô thuốc nào không
            if (dataGridView2.Rows.Count == 1)
            {
                MessageBox.Show("Vui lòng thêm thuốc nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy thông tin phiếu nhập
                string maPN = TaoMaPhieuNhapMoi();
                DateTime ngayNhap = DateTime.Now;

                // Lấy tổng trị giá từ label (bỏ chữ VNĐ)
                string tongText = lbTongTien.Text.Replace(" VNĐ", "").Replace(",", "");
                decimal triGia = decimal.Parse(tongText);

                // Bắt đầu transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert vào bảng PHIEUNHAP
                        string sqlInsertPN = @"INSERT INTO PHIEUNHAP (PN_MA, PN_NGAYNHAP, PN_TRIGIA, NV_MA) 
                                     VALUES (@maPN, @ngayNhap, @triGia, @maNV)";
                        using (SqlCommand cmd = new SqlCommand(sqlInsertPN, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@maPN", maPN);
                            cmd.Parameters.AddWithValue("@ngayNhap", ngayNhap);
                            cmd.Parameters.AddWithValue("@triGia", triGia);
                            cmd.Parameters.AddWithValue("@maNV", ma);
                            cmd.ExecuteNonQuery();
                        }

                        // Insert vào bảng LOTHUOC cho từng lô
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells["MaLo"].Value != null)
                            {
                                string maLo = row.Cells["MaLo"].Value.ToString();
                                string maThuoc = row.Cells["MaThuoc"].Value.ToString();
                                DateTime ngaySX = DateTime.Parse(row.Cells["NgaySX"].Value.ToString());
                                DateTime hanSD = DateTime.Parse(row.Cells["HanSD"].Value.ToString());

                                // Lấy số lượng nhập (bỏ dấu phẩy nếu có)
                                string slNhapStr = row.Cells["SoLuongNhap"].Value.ToString();
                                int slNhap = int.Parse(slNhapStr.Replace(",", ""));

                                // Lấy đơn giá nhập (bỏ dấu phẩy nếu có)
                                string dgNhapStr = row.Cells["DonGiaNhap"].Value.ToString();
                                decimal dgNhap = decimal.Parse(dgNhapStr.Replace(",", ""));

                                string sqlInsertLo = @"INSERT INTO LOTHUOC 
                                            (LT_MA, T_MA, LT_NGAYSX, LT_NGAYHD, LT_SLNHAP, LT_DGNHAP, LT_SLTON, PN_MA)
                                            VALUES (@maLo, @maThuoc, @ngaySX, @hanSD, @slNhap, @dgNhap, @slTon, @maPN)";

                                using (SqlCommand cmd = new SqlCommand(sqlInsertLo, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@maLo", maLo);
                                    cmd.Parameters.AddWithValue("@maThuoc", maThuoc);
                                    cmd.Parameters.AddWithValue("@ngaySX", ngaySX);
                                    cmd.Parameters.AddWithValue("@hanSD", hanSD);
                                    cmd.Parameters.AddWithValue("@slNhap", slNhap);
                                    cmd.Parameters.AddWithValue("@dgNhap", dgNhap);
                                    cmd.Parameters.AddWithValue("@slTon", slNhap); // SL tồn = SL nhập
                                    cmd.Parameters.AddWithValue("@maPN", maPN);
                                    cmd.ExecuteNonQuery();
                                }

                                // Cập nhật tồn kho trong bảng THUOC
                                string sqlUpdateThuoc = @"UPDATE THUOC SET T_SLTON = T_SLTON + @slNhap 
                                                WHERE T_MA = @maThuoc";
                                using (SqlCommand cmd = new SqlCommand(sqlUpdateThuoc, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@slNhap", slNhap);
                                    cmd.Parameters.AddWithValue("@maThuoc", maThuoc);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Commit transaction
                        transaction.Commit();
                        MessageBox.Show($"Nhập thuốc thành công!\nMã phiếu nhập: {maPN}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Hỏi có muốn in phiếu nhập không
                        DialogResult dialogResult = MessageBox.Show(
                            "Bạn có muốn in phiếu nhập ra PDF không?","In phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            XuatPhieuNhapPDF(maPN, ngayNhap, triGia);
                        }

                        // Reset form
                        dataGridView2.Rows.Clear();
                        TinhTongTien();
                        LoadThuoc(); // Cập nhật lại tồn kho hiển thị
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi nhập thuốc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xuất PDF phiếu nhập 
        private void XuatPhieuNhapPDF(string maPN, DateTime ngayNhap, decimal triGia)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"PhieuNhap_{maPN}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Lấy thông tin nhân viên
                    string tenNhanVien = "";
                    string sqlNV = "SELECT NV_TEN FROM NHANVIEN WHERE NV_MA = @MaNV";
                    using (SqlCommand cmdNV = new SqlCommand(sqlNV, conn))
                    {
                        cmdNV.Parameters.AddWithValue("@MaNV", ma);
                        object result = cmdNV.ExecuteScalar();
                        if (result != null)
                        {
                            tenNhanVien = result.ToString();
                        }
                    }
                    // Tạo PDF
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 20, 20, 30, 30);
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(saveDialog.FileName, FileMode.Create));
                    doc.Open();

                    // Font Unicode
                    string fontPath = @"C:\Windows\Fonts\arial.ttf";
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);

                    // Tiêu đề
                    var paraTitle = new iTextSharp.text.Paragraph("PHIẾU NHẬP THUỐC", fontTitle);
                    paraTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(paraTitle);

                    //Thông tin phiếu
                    doc.Add(new iTextSharp.text.Paragraph($"Mã phiếu: {maPN}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Ngày nhập: {ngayNhap:dd/MM/yyyy HH:mm}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Nhân viên: {ma} - {tenNhanVien}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Nơi nhập: {lbNoiNhap.Text}", fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Bảng chi tiết
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(8);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1.5f, 2, 1.5f, 1.5f, 1, 2, 2, 2 });

                    // Header bảng
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Mã lô", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Tên thuốc", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Ngày SX", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Hạn SD", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("SL", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Đơn giá", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("ĐVT", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Thành tiền", fontBold)));

                    // Dữ liệu
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells["MaLo"].Value != null)
                        {
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["MaLo"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["TenThuoc"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["NgaySX"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["HanSD"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["SoLuongNhap"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["DonGiaNhap"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["DonViTinh"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["ThanhTien"].Value.ToString(), fontNormal));
                        }
                    }

                    doc.Add(table);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Tổng tiền
                    var paraTongTien = new iTextSharp.text.Paragraph($"Tổng trị giá: {triGia:N0} VNĐ", fontBold);
                    paraTongTien.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    doc.Add(paraTongTien);

                    doc.Close();
                    writer.Close();

                    MessageBox.Show($"Đã xuất PDF thành công!\n{Path.GetFileName(saveDialog.FileName)}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btXem_Click(object sender, EventArgs e)
        {
            FormXemLichSuNhap f = new FormXemLichSuNhap();
            f.ShowDialog();
        }

    }
}
