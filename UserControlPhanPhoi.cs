using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_ThuocTay
{
    public partial class UserControlPhanPhoi : UserControl
    {
        SqlConnection conn = new SqlConnection();
        string ma;
        ChucNang cn = new ChucNang();
        public UserControlPhanPhoi(string maNV)
        {
            InitializeComponent();
            ma = maNV;
        }

        private void UserControlPhanPhoi_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadLoThuoc();   
            string sql = "SELECT CN_MA, CN_TEN, CN_SDT, CN_DIACHI FROM CHINHANH";
            cn.loadComboBox(comboBox2, sql, "CN_MA", "CN_TEN", conn);

            lbNgayNhap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            lbNguoiNhap.Text = ma;

            // Tạo columns cho dataGridView2 phiếu nhập
            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("MaLo", "Mã lô");
            dataGridView2.Columns.Add("MaThuoc", "Mã thuốc");
            dataGridView2.Columns.Add("TenThuoc", "Tên thuốc");
            dataGridView2.Columns.Add("NgaySX", "Ngày SX");
            dataGridView2.Columns.Add("HanSD", "Hạn SD");
            dataGridView2.Columns.Add("SoLuongChuyen", "Số lượng chuyển");
            dataGridView2.Columns.Add("DonGiaBan", "Đơn giá bán");
            dataGridView2.Columns.Add("DonGiaNhap", "Đơn giá nhập");
            dataGridView2.Columns.Add("DonViTinh", "Đơn vị tính");
            dataGridView2.Columns.Add("ThanhTien", "Thành tiền");
        }

        private void LoadLoThuoc(string keyword = "")
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
            LoadLoThuoc(keyword);
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
                //lấy đơn giá bán
                decimal donGia = Convert.ToDecimal(row.Cells["Đơn giá bán"].Value);
                lbDonGia.Text = donGia > 0 ? donGia.ToString("N0") + " VNĐ" : "Chưa có giá";
            }
            txtSLNhap.Focus();
            txtTim.Clear();
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

        // Hàm lấy danh sách lô thuốc theo FEFO
        private DataTable LayDanhSachLoThuoc(string maThuoc)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    LT_MA AS MaLo,
                    LT_NGAYHD AS HanSD,
                    LT_DGNHAP AS DonGiaNhap,
                    LT_SLTON AS SoLuongTon,
                    LT_NGAYSX AS NgaySX
                FROM LOTHUOC
                WHERE T_MA = @MaThuoc 
                    AND LT_SLTON > 0
                ORDER BY LT_NGAYHD ASC, LT_MA ASC"; // Ngày hết hạn sớm nhất trước, nếu cùng thì mã lô nhỏ hơn trước

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        // Hàm lấy số lượng đã chuyển của một lô trong datagridview2
        private int LaySoLuongDaChuyenCuaLo(string maLo)
        {
            int soLuongDaChuyen = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["MaLo"].Value?.ToString() == maLo)
                {
                    if (row.Cells["SoLuongChuyen"].Value != null)
                    {
                        if (int.TryParse(row.Cells["SoLuongChuyen"].Value.ToString(), out int sl))
                        {
                            soLuongDaChuyen += sl;
                        }
                    }
                }
            }
            return soLuongDaChuyen;
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra các điều kiện
            if (string.IsNullOrEmpty(lbTenThuoc.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn thuốc cần phân phối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtSLNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSLNhap.Focus();
                return;
            }

            // Parse lại để lấy giá trị (đã validate ở Leave event)
            int soLuongCanChuyen = int.Parse(txtSLNhap.Text);
            int tongTonKho = int.Parse(lbSLTon.Text);

            // Kiểm tra số lượng tồn tổng
            if (tongTonKho <= 0)
            {
                MessageBox.Show("Thuốc này hiện đã hết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maThuoc = lbMaThuoc.Text;

            // Tính tổng số lượng đã chuyển của thuốc này trong datagridview2
            int tongDaChuyen = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["MaThuoc"].Value?.ToString() == maThuoc)
                {
                    if (row.Cells["SoLuongChuyen"].Value != null)
                    {
                        if (int.TryParse(row.Cells["SoLuongChuyen"].Value.ToString(), out int sl))
                        {
                            tongDaChuyen += sl;
                        }
                    }
                }
            }

            // Kiểm tra tổng số lượng cần chuyển (đã chuyển + mới chuyển) có vượt quá tồn kho không
            if (tongDaChuyen + soLuongCanChuyen > tongTonKho)
            {
                MessageBox.Show($"Số lượng chuyển vượt quá tồn kho!\n", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy danh sách lô thuốc theo FEFO
            DataTable dsLoThuoc = LayDanhSachLoThuoc(maThuoc);

            if (dsLoThuoc.Rows.Count == 0)
            {
                MessageBox.Show("Không có lô thuốc nào tồn kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuongConLai = soLuongCanChuyen;

            // Duyệt qua từng lô theo thứ tự FEFO
            foreach (DataRow lo in dsLoThuoc.Rows)
            {
                if (soLuongConLai <= 0) break;

                string maLoThuoc = lo["MaLo"].ToString();
                int soLuongTonLo = Convert.ToInt32(lo["SoLuongTon"]);
                decimal donGiaNhapLo = Convert.ToDecimal(lo["DonGiaNhap"]);
                DateTime ngaySX = Convert.ToDateTime(lo["NgaySX"]);
                DateTime hanSD = Convert.ToDateTime(lo["HanSD"]);

                /// Lấy số lượng đã chuyển của lô này trong datagridview2
                int soLuongDaChuyen = LaySoLuongDaChuyenCuaLo(maLoThuoc);
                int soLuongConLaiTrongLo = soLuongTonLo - soLuongDaChuyen;

                if (soLuongConLaiTrongLo > 0)
                {
                    // Số lượng sẽ lấy từ lô này
                    int soLuongLayTuLo = Math.Min(soLuongConLai, soLuongConLaiTrongLo);

                    // Kiểm tra xem lô này đã có trong datagridview2 chưa
                    bool loDaTonTai = false;
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells["MaLo"].Value?.ToString() == maLoThuoc)
                        {
                            // Cộng dồn số lượng vào dòng đã có
                            int soLuongCu = Convert.ToInt32(row.Cells["SoLuongChuyen"].Value);
                            int soLuongMoi = soLuongCu + soLuongLayTuLo;
                            row.Cells["SoLuongChuyen"].Value = soLuongMoi.ToString();

                            // Cập nhật thành tiền
                            decimal thanhTienMoi = soLuongMoi * donGiaNhapLo;
                            row.Cells["ThanhTien"].Value = thanhTienMoi.ToString("N0");

                            loDaTonTai = true;
                            break;
                        }
                    }

                    if (!loDaTonTai)
                    {
                        // Thêm dòng mới vào datagridview2
                        decimal thanhTien = soLuongLayTuLo * donGiaNhapLo;

                        dataGridView2.Rows.Add(
                            maLoThuoc,
                            maThuoc,
                            lbTenThuoc.Text,
                            ngaySX.ToString("MM/dd/yyyy"),
                            hanSD.ToString("MM/dd/yyyy"),
                            soLuongLayTuLo.ToString(),  // Cột "SoLuongChuyen"
                            lbDonGia.Text.Replace(" VNĐ", ""),
                            donGiaNhapLo.ToString("N0"),
                            lbDVT.Text,
                            thanhTien.ToString("N0")
                        );
                    }
                    soLuongConLai -= soLuongLayTuLo;
                }
            }
            if (soLuongConLai > 0)
            {
                MessageBox.Show($"Chỉ có thể chuyển được {soLuongCanChuyen - soLuongConLai}/{soLuongCanChuyen} số lượng!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            TinhTongTien(); // Tính tổng tiền
            ClearFormNhap(); // Reset form nhập
        }

        private void ClearFormNhap()
        {
            txtSLNhap.Clear();
            LoadLoThuoc();
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

        // Hàm tạo mã phiếu xuất mới
        private string TaoMaPhieuXuatMoi()
        {
            string sql = "SELECT MAX(PX_MA) FROM PHIEUXUAT WHERE PX_MA LIKE 'PX%'";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string maxMa = result.ToString();
                    int number = int.Parse(maxMa.Substring(2)) + 1;
                    return "PX" + number.ToString("D3"); 
                }
                else
                {
                    return "PX001";
                }
            }
        }

        private void btXuatThuoc_Click(object sender, EventArgs e)
        {
            //Kiểm tra các điều kiện
            if (dataGridView2.Rows.Count == 1)
            {
                MessageBox.Show("Vui lòng chọn thuốc để phân phối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn chi nhánh nhận thuốc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Tạo mã phiếu xuất mới
                string maPhieuXuat = TaoMaPhieuXuatMoi();

                // Lấy thông tin từ form
                DateTime ngayXuat = DateTime.Now;
                string maNhanVien = ma;
                string maChiNhanh = comboBox2.SelectedValue.ToString();
                string tenChiNhanh = comboBox2.Text;

                // Lấy tổng tiền từ label 
                string tongText = lbTongTien.Text.Replace(" VNĐ", "").Replace(",", "");
                decimal tongTien = decimal.Parse(tongText);

                // Bắt đầu transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Tạo phiếu xuất mới
                        string sqlInsertPhieuXuat = @"
                            INSERT INTO PHIEUXUAT (PX_MA, PX_NGAYXUAT, NV_MA, CN_MA)
                            VALUES (@MaPhieuXuat, @NgayXuat, @MaNhanVien, @MaChiNhanh)";

                        using (SqlCommand cmd = new SqlCommand(sqlInsertPhieuXuat, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                            cmd.Parameters.AddWithValue("@NgayXuat", ngayXuat);
                            cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                            cmd.Parameters.AddWithValue("@MaChiNhanh", maChiNhanh);
                            cmd.ExecuteNonQuery();
                        }

                        // Duyệt qua từng dòng trong dataGridView2 để tạo CTPX và cập nhật số lượng
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells["MaLo"].Value == null) continue;

                            string maLo = row.Cells["MaLo"].Value.ToString();
                            string maThuoc = row.Cells["MaThuoc"].Value.ToString();
                            int soLuongChuyen = Convert.ToInt32(row.Cells["SoLuongChuyen"].Value);

                            // Tạo chi tiết phiếu xuất 
                            string sqlInsertCTPX = @"
                                INSERT INTO CTPX (LT_MA, T_MA, PX_MA, CTPX_SOLUONG, CTPX_SLTON)
                                VALUES (@MaLo, @MaThuoc, @MaPhieuXuat, @SoLuong, @SoLuongTon)";

                            using (SqlCommand cmd = new SqlCommand(sqlInsertCTPX, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaLo", maLo);
                                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                                cmd.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                                cmd.Parameters.AddWithValue("@SoLuong", soLuongChuyen);
                                cmd.Parameters.AddWithValue("@SoLuongTon", soLuongChuyen);
                                cmd.ExecuteNonQuery();
                            }

                            // Cập nhật số lượng tồn trong LOTHUOC
                            string sqlUpdateLoThuoc = @"
                                UPDATE LOTHUOC 
                                SET LT_SLTON = LT_SLTON - @SoLuongChuyen
                                WHERE LT_MA = @MaLo AND T_MA = @MaThuoc";

                            using (SqlCommand cmd = new SqlCommand(sqlUpdateLoThuoc, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SoLuongChuyen", soLuongChuyen);
                                cmd.Parameters.AddWithValue("@MaLo", maLo);
                                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        //Cập nhật tổng số lượng tồn trong bảng THUOC cho từng mã thuốc
                        Dictionary<string, int> tongSoLuongThuoc = new Dictionary<string, int>();

                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells["MaThuoc"].Value == null) continue;

                            string maThuoc = row.Cells["MaThuoc"].Value.ToString();
                            int soLuongChuyen = Convert.ToInt32(row.Cells["SoLuongChuyen"].Value);

                            if (tongSoLuongThuoc.ContainsKey(maThuoc))
                            {
                                tongSoLuongThuoc[maThuoc] += soLuongChuyen;
                            }
                            else
                            {
                                tongSoLuongThuoc[maThuoc] = soLuongChuyen;
                            }
                        }

                        // Cập nhật số lượng tồn trong THUOC
                        foreach (var item in tongSoLuongThuoc)
                        {
                            string sqlUpdateThuoc = @"
                                UPDATE THUOC 
                                SET T_SLTON = T_SLTON - @SoLuongChuyen
                                WHERE T_MA = @MaThuoc";

                            using (SqlCommand cmd = new SqlCommand(sqlUpdateThuoc, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SoLuongChuyen", item.Value);
                                cmd.Parameters.AddWithValue("@MaThuoc", item.Key);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Commit transaction
                        transaction.Commit();

                        // Thông báo thành công
                        MessageBox.Show($"Đã tạo phiếu phân phối {maPhieuXuat} thành công!\n", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Hỏi có muốn in phiếu xuất không
                        DialogResult dialogResult = MessageBox.Show("Bạn có muốn in phiếu xuất ra PDF không?", "In phiếu xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            XuatPhieuXuatPDF(maPhieuXuat, ngayXuat, maChiNhanh, tenChiNhanh, tongTien);
                        }

                        // 8. Reset form
                        dataGridView2.Rows.Clear();
                        TinhTongTien();
                        ClearFormNhap();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi khi tạo phiếu xuất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phiếu xuất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xuất PDF phiếu xuất
        private void XuatPhieuXuatPDF(string maPX, DateTime ngayXuat, string maChiNhanh, string tenChiNhanh, decimal tongTien)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"PhieuXuat_{maPX}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

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
                    var paraTitle = new iTextSharp.text.Paragraph("PHIẾU XUẤT THUỐC", fontTitle);
                    paraTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(paraTitle);

                    // Thông tin phiếu
                    doc.Add(new iTextSharp.text.Paragraph($"Mã phiếu: {maPX}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Ngày xuất: {ngayXuat:dd/MM/yyyy HH:mm}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Nhân viên: {ma} - {tenNhanVien}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Chi nhánh nhận: {maChiNhanh} - {tenChiNhanh}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Kho xuất: Kho tổng", fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Bảng chi tiết
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(9);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1.5f, 1.5f, 2, 1.5f, 1.5f, 1, 2, 2, 2 });

                    // Header bảng
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Mã lô", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Mã thuốc", fontBold)));
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
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["MaThuoc"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["TenThuoc"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["NgaySX"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["HanSD"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["SoLuongChuyen"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["DonGiaNhap"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["DonViTinh"].Value.ToString(), fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(row.Cells["ThanhTien"].Value.ToString(), fontNormal));
                        }
                    }

                    doc.Add(table);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Tổng tiền
                    var paraTongTien = new iTextSharp.text.Paragraph($"Tổng trị giá: {tongTien:N0} VNĐ", fontBold);
                    paraTongTien.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    doc.Add(paraTongTien);

                    doc.Close();
                    writer.Close();

                    MessageBox.Show($"Đã xuất PDF thành công!\n{Path.GetFileName(saveDialog.FileName)}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            FormXemLichSuXuat f = new FormXemLichSuXuat();
            f.ShowDialog();
        }
    }
}
