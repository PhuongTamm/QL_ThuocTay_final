using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace QL_ThuocTay
{
    public partial class UserControlHoaDon : UserControl
    {
        SqlConnection conn = new SqlConnection();
        string ma;
        ChucNang cn = new ChucNang();

        public UserControlHoaDon(string maNV)
        {
            InitializeComponent();
            ma = maNV;
        }

        private void UserControlHoaDon_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            string sql = "SELECT CN_MA, CN_TEN, CN_SDT, CN_DIACHI FROM CHINHANH";
            cn.loadComboBox(comboBox1, sql, "CN_MA", "CN_TEN", conn);

            lbNgayLap.Text = DateTime.Now.ToString("MM/dd/yyyy"); //hiển thị ngày giờ hiện tại lập hóa đơn
            lbNguoiLap.Text = ma;   //hiển thị mã nhân viên hiện tại

            // Tạo columns cho dataGridView2
            dataGridView2.Columns.Clear();  //Đảm bảo không bị trùng lặp columns nếu code chạy nhiều lần
            dataGridView2.Columns.Add("MaThuoc", "Mã thuốc");
            dataGridView2.Columns.Add("TenThuoc", "Tên thuốc");
            dataGridView2.Columns.Add("SoLuong", "Số lượng");
            dataGridView2.Columns.Add("DonViTinh", "Đơn vị tính");
            dataGridView2.Columns.Add("DonGia", "Đơn giá");
            dataGridView2.Columns.Add("ThanhTien", "Thành tiền");

            txtTim.Clear();// Clear ô tìm kiếm 
            // Chọn chi nhánh đầu tiên để kích hoạt sự kiện ko cho chuyển chi nhánh khi thêm hóa đơn
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private string maChiNhanhHienTai = ""; // Lưu chi nhánh hiện tại

        //load lô thuốc theo combobox chi nhánh
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                string maChiNhanhMoi = comboBox1.SelectedValue.ToString();
                // Kiểm tra ko cho chuyển chi nhánh khi đang thêm thuốc vào hóa đơn, khi xóa hết thì có thể chuyển
                if (dataGridView2.Rows.Count > 1 && maChiNhanhMoi != maChiNhanhHienTai)
                {
                    MessageBox.Show("Không thể chuyển chi nhánh!!!\nVui lòng xóa hết thuốc trong hóa đơn trước khi chuyển.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox1.SelectedValue = maChiNhanhHienTai; // Giữ lại chi nhánh cũ
                    return;
                }
                maChiNhanhHienTai = maChiNhanhMoi;
                HienThiThongTinChiNhanh();
                LoadLoThuocTheoChiNhanh(maChiNhanhHienTai);
            }
        }

        private void HienThiThongTinChiNhanh()
        {
            // Lấy DataRowView từ item được chọn
            if (comboBox1.SelectedItem is DataRowView selectedRow)
            {
                lbMaCN.Text = selectedRow["CN_MA"].ToString();
                lbSDTCN.Text = selectedRow["CN_SDT"].ToString();
                lbDiaChi.Text = selectedRow["CN_DIACHI"].ToString();
            }
        }

        //hiển thị tổng các lô thuốc tương ứng của từng thuốc có trong chi nhánh
        private void LoadLoThuocTheoChiNhanh(string maChiNhanh)
        {
            txtTim.Clear(); // Clear ô tìm kiếm khi load chi nhánh mới
            string sql = $@"
                SELECT 
                    lt.T_MA AS [Mã thuốc],
                    t.T_TEN AS [Tên thuốc],
                    ISNULL(SUM(ctpx.CTPX_SLTON), 0) AS [Số lượng tồn],
                    dvt.DVT_TEN AS [Đơn vị tính],
                    ISNULL(dg.LAST_PRICE, 0) AS [Đơn giá]
                FROM LOTHUOC lt
                INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                INNER JOIN DONVITINH dvt ON t.DVT_MA = dvt.DVT_MA
                INNER JOIN CTPX ctpx ON lt.LT_MA = ctpx.LT_MA AND lt.T_MA = ctpx.T_MA
                INNER JOIN PHIEUXUAT px ON ctpx.PX_MA = px.PX_MA 
                OUTER APPLY (
                    SELECT TOP 1 DG_DONGIA AS LAST_PRICE
                    FROM DONGIA 
                    WHERE T_MA = lt.T_MA 
                    ORDER BY NGAY DESC, THOIGIAN DESC
                ) AS dg
                WHERE px.CN_MA = '{maChiNhanh}'
                GROUP BY lt.T_MA, t.T_TEN, dvt.DVT_TEN, dg.LAST_PRICE
                ORDER BY lt.T_MA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTim.Clear();// Clear ô tìm kiếm 
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                lbTenThuoc.Text = row.Cells[1].Value?.ToString();  
                lbSLTon.Text = row.Cells[2].Value?.ToString();     
                lbDVT.Text = row.Cells[3].Value?.ToString();       
                lbDonGia.Text = row.Cells[4].Value?.ToString();
                txtSL.Focus();
            }
        }

        private void btXemLT_Click(object sender, EventArgs e)
        {
            FormXemLoThuoc f = new FormXemLoThuoc();
            f.ShowDialog();
            //LoadThuoc();
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();
            if (!string.IsNullOrEmpty(sdt))
            {
                // Kiểm tra ràng buộc SDT
                if (sdt.Length != 10 || !sdt.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSDT.Clear();
                }
            }
            
        }

        private void txtKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sdt = txtSDT.Text.Trim();
                if (!string.IsNullOrEmpty(sdt))
                {
                    // Kiểm tra ràng buộc SDT
                    if (sdt.Length != 10 || !sdt.All(char.IsDigit))
                    {
                        MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra khách hàng đã tồn tại chưa
                    string sqlCheck = "SELECT KH_TEN, KH_GIOITINH FROM KHACHHANGTHANHVIEN WHERE KH_SDT = @sdt";
                    SqlCommand cmd = new SqlCommand(sqlCheck, conn);
                    cmd.Parameters.AddWithValue("@sdt", sdt);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Khách hàng đã tồn tại -> mở form sửa
                        string ten = reader["KH_TEN"].ToString();
                        string gioiTinh = reader["KH_GIOITINH"].ToString();
                        reader.Close();

                        FormSuaKH formSua = new FormSuaKH(sdt, ten, gioiTinh);
                        formSua.ShowDialog();
                    }
                    else
                    {
                        // Khách hàng chưa tồn tại -> mở form thêm
                        reader.Close();
                        FormThemKH formThem = new FormThemKH();
                        formThem.txtSDT.Text = sdt; // Truyền SDT vào form thêm, sửa trong design thành public
                        formThem.ShowDialog();
                    }
                }
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã chọn thuốc chưa
            if (string.IsNullOrEmpty(lbTenThuoc.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn thuốc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra số lượng nhập
            if (!int.TryParse(txtSL.Text, out int soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSL.Clear();
                txtSL.Focus();
                return;
            }

            if (dataGridView1.CurrentRow != null && !string.IsNullOrEmpty(txtSL.Text))
            {
                DataGridViewRow row = dataGridView1.CurrentRow;
                int soLuong = int.Parse(txtSL.Text);

                // Kiểm tra số lượng tồn
                int soLuongTon = Convert.ToInt32(row.Cells["Số lượng tồn"].Value);
                if (soLuong > soLuongTon)
                {
                    MessageBox.Show($"Số lượng tồn không đủ! Chỉ còn {soLuongTon}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string maThuoc = row.Cells["Mã thuốc"].Value.ToString();
                string tenThuoc = row.Cells["Tên thuốc"].Value.ToString();
                string dvt = row.Cells["Đơn vị tính"].Value.ToString();
                decimal donGia = Convert.ToDecimal(row.Cells["Đơn giá"].Value);

                // Kiểm tra nếu thuốc đã có trong hóa đơn thì cộng dồn
                bool daTonTai = false;
                foreach (DataGridViewRow rowHD in dataGridView2.Rows)
                {
                    if (rowHD.Cells["MaThuoc"].Value?.ToString() == maThuoc)
                    {
                        // Cộng dồn số lượng
                        int soLuongCu = Convert.ToInt32(rowHD.Cells["SoLuong"].Value);
                        int soLuongMoi = soLuongCu + soLuong;

                        // Kiểm tra lại tổng số lượng với tồn kho
                        if (soLuongMoi > soLuongTon)
                        {
                            MessageBox.Show($"Tổng số lượng vượt quá tồn kho! Chỉ còn {soLuongTon}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Cập nhật số lượng và thành tiền
                        rowHD.Cells["SoLuong"].Value = soLuongMoi;
                        rowHD.Cells["ThanhTien"].Value = soLuongMoi * donGia;
                        daTonTai = true;
                        break;
                    }
                }

                // Nếu thuốc chưa có trong hóa đơn thì thêm mới
                if (!daTonTai)
                {
                    decimal thanhTien = soLuong * donGia;
                    dataGridView2.Rows.Add(maThuoc, tenThuoc, soLuong, dvt, donGia, thanhTien);
                }
                txtSL.Clear(); // Reset số lượng
                TinhTongTien(); // Tính lại tổng tiền
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
                TinhTongTien();// Tính lại tổng tiền
            }
        }

        private void btClearAll_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            TinhTongTien();
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

        // Hàm tạo mã hóa đơn tự tăng
        private string TaoMaHoaDonMoi()
        {
            string sql = "SELECT MAX(HD_MA) FROM HOADON WHERE HD_MA LIKE 'HD%'";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string maxMa = result.ToString();
                    int number = int.Parse(maxMa.Substring(2)) + 1;
                    return "HD" + number.ToString("D3"); // HD010 -> HD011
                }
                else
                {
                    return "HD001";
                }
            }
        }

        private void btThanhToan_Click(object sender, EventArgs e)
        {
            // Kiểm tra điều kiện
            if (dataGridView2.Rows.Count == 1)
            {
                MessageBox.Show("Vui lòng chọn thuốc để lập hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTienKH.Text))
            {
                MessageBox.Show("Vui lòng nhập tiền khách đưa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienKH.Focus();
                return;
            }

            // KHÔNG bắt buộc SDT - cho phép khách vãng lai
            string sdtKH = string.IsNullOrEmpty(txtSDT.Text.Trim()) ? null : txtSDT.Text.Trim();
            try
            {
                // Lấy thông tin hóa đơn
                string maNV = ma; // từ constructor
                string maCN = comboBox1.SelectedValue?.ToString();
                DateTime ngayLap = DateTime.Now;

                // Tạo mã hóa đơn mới
                string maHD = TaoMaHoaDonMoi();

                // Bắt đầu transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert vào bảng HOADON
                        string sqlInsertHD = @"INSERT INTO HOADON (HD_MA, HD_NGAYLAP, NV_MA, CN_MA, KH_SDT) 
                                     VALUES (@maHD, @ngayLap, @maNV, @maCN, @sdtKH)";
                        using (SqlCommand cmd = new SqlCommand(sqlInsertHD, conn, transaction))
                        {
                            cmd.Parameters.Clear(); // Xóa tất cả parameters cũ
                            cmd.Parameters.AddWithValue("@maHD", maHD);
                            cmd.Parameters.AddWithValue("@ngayLap", ngayLap);
                            cmd.Parameters.AddWithValue("@maNV", maNV);
                            cmd.Parameters.AddWithValue("@maCN", maCN);
                            //cmd.Parameters.AddWithValue("@sdtKH", sdtKH);
                            cmd.Parameters.AddWithValue("@sdtKH", string.IsNullOrEmpty(sdtKH) ? (object)DBNull.Value : sdtKH); // Xử lý NULL cho SDT 
                            cmd.ExecuteNonQuery();
                        }

                        // Duyệt qua từng thuốc trong hóa đơn
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            // Kiểm tra tất cả cells cần thiết - bỏ qua dòng trống
                            if (row.Cells["MaThuoc"].Value == null ||
                                row.Cells["SoLuong"].Value == null ||
                                row.Cells["DonGia"].Value == null)
                                continue;

                            string maThuoc = row.Cells["MaThuoc"].Value.ToString();
                            int soLuongBan = Convert.ToInt32(row.Cells["SoLuong"].Value);
                            decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value);
                            int soLuongConLai = soLuongBan; // Trừ số lượng từ các lô thuốc

                            // Lấy danh sách lô thuốc có sẵn, sắp xếp hết hạn trước, mã phiếu xuất trước, mã lô nhỏ trước
                            string sqlGetLoThuoc = @"
                                SELECT TOP 100 PERCENT 
                                    lt.LT_MA, 
                                    lt.T_MA, 
                                    ctpx.PX_MA, 
                                    ctpx.CTPX_SLTON, 
                                    lt.LT_NGAYHD,
                                    px.PX_MA  -- Thêm để ORDER BY chính xác
                                FROM LOTHUOC lt
                                INNER JOIN CTPX ctpx ON lt.LT_MA = ctpx.LT_MA AND lt.T_MA = ctpx.T_MA
                                INNER JOIN PHIEUXUAT px ON ctpx.PX_MA = px.PX_MA
                                WHERE lt.T_MA = @maThuoc 
                                    AND px.CN_MA = @maCN
                                    AND ctpx.CTPX_SLTON > 0
                                ORDER BY 
                                    lt.LT_NGAYHD ASC,      
                                    px.PX_MA ASC,          
                                    lt.LT_MA ASC";        
                            using (SqlCommand cmd = new SqlCommand(sqlGetLoThuoc, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@maThuoc", maThuoc);
                                cmd.Parameters.AddWithValue("@maCN", maCN);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    List<Tuple<string, string, string, int, DateTime>> danhSachLo = new List<Tuple<string, string, string, int, DateTime>>();
                                    while (reader.Read())
                                    {
                                        string lt_ma = reader["LT_MA"].ToString();
                                        string t_ma = reader["T_MA"].ToString();
                                        string px_ma = reader["PX_MA"].ToString();
                                        int slTon = Convert.ToInt32(reader["CTPX_SLTON"]);
                                        DateTime ngayHD = Convert.ToDateTime(reader["LT_NGAYHD"]);
                                        danhSachLo.Add(new Tuple<string, string, string, int, DateTime>(lt_ma, t_ma, px_ma, slTon, ngayHD));
                                    }
                                    reader.Close();

                                    Dictionary<string, int> soLuongTruTheoLo = new Dictionary<string, int>(); // Dictionary để lưu số lượng đã trừ cho mỗi lô (cho CTHD)
                                    // Trừ số lượng từ các lô theo thứ tự đã sắp xếp
                                    foreach (var lo in danhSachLo)
                                    {
                                        if (soLuongConLai <= 0) break;
                                        string lt_ma = lo.Item1;
                                        string t_ma = lo.Item2;
                                        string px_ma = lo.Item3;
                                        int slTon = lo.Item4;
                                        int soLuongTru = Math.Min(soLuongConLai, slTon);

                                        // Cập nhật số lượng tồn trong CTPX
                                        string sqlUpdateCTPX = @"
                                            UPDATE CTPX 
                                            SET CTPX_SLTON = CTPX_SLTON - @soLuongTru
                                            WHERE LT_MA = @lt_ma AND T_MA = @t_ma AND PX_MA = @px_ma";

                                        using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdateCTPX, conn, transaction))
                                        {
                                            cmdUpdate.Parameters.AddWithValue("@soLuongTru", soLuongTru);
                                            cmdUpdate.Parameters.AddWithValue("@lt_ma", lt_ma);
                                            cmdUpdate.Parameters.AddWithValue("@t_ma", t_ma);
                                            cmdUpdate.Parameters.AddWithValue("@px_ma", px_ma);
                                            cmdUpdate.ExecuteNonQuery();
                                        }

                                        // Lưu số lượng đã trừ cho lô này (cộng dồn nếu cùng lô)
                                        if (soLuongTruTheoLo.ContainsKey(lt_ma))
                                        {
                                            soLuongTruTheoLo[lt_ma] += soLuongTru;
                                        }
                                        else
                                        {
                                            soLuongTruTheoLo[lt_ma] = soLuongTru;
                                        }

                                        soLuongConLai -= soLuongTru;
                                    }

                                    // Kiểm tra nếu không đủ số lượng
                                    if (soLuongConLai > 0)
                                    {
                                        throw new Exception($"Không đủ số lượng tồn cho thuốc {maThuoc}");
                                    }

                                    // Insert vào CTHD (1 record cho mỗi lô thuốc đã trừ)
                                    foreach (var kvp in soLuongTruTheoLo)
                                    {
                                        string lt_ma = kvp.Key;
                                        int soLuongDaTru = kvp.Value;

                                        string sqlInsertCTHD = @"
                                            INSERT INTO CTHD (LT_MA, T_MA, HD_MA, CTHD_SOLUONG, CTHD_DONGIA)
                                            VALUES (@lt_ma, @t_ma, @hd_ma, @soLuong, @donGia)";

                                        using (SqlCommand cmdCTHD = new SqlCommand(sqlInsertCTHD, conn, transaction))
                                        {
                                            cmdCTHD.Parameters.AddWithValue("@lt_ma", lt_ma);
                                            cmdCTHD.Parameters.AddWithValue("@t_ma", maThuoc); // Sử dụng maThuoc từ vòng lặp
                                            cmdCTHD.Parameters.AddWithValue("@hd_ma", maHD);
                                            cmdCTHD.Parameters.AddWithValue("@soLuong", soLuongDaTru);
                                            cmdCTHD.Parameters.AddWithValue("@donGia", donGia);
                                            cmdCTHD.ExecuteNonQuery();
                                        }

                                    }
                                }
                            }
                        }

                        // Commit transaction
                        transaction.Commit();

                        MessageBox.Show($"Thanh toán thành công!\nMã hóa đơn: {maHD}", "Thành công",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Hỏi có muốn in hóa đơn?
                        DialogResult dialogResult = MessageBox.Show("Bạn có muốn in hóa đơn không?", "In hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            InHoaDonPDF(maHD, maCN);
                        }

                        // Reset form
                        dataGridView2.Rows.Clear();
                        TinhTongTien();
                        txtSDT.Clear();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearFormNhap();
        }

        private void ClearFormNhap()
        {
            txtTienKH.Clear();
            lbTienThua.Text = "";
            txtSL.Clear();
            lbTenThuoc.Text = "";
            lbSLTon.Text = "";
            lbDVT.Text = "";
            lbDonGia.Text = "";
            LoadLoThuocTheoChiNhanh(maChiNhanhHienTai);
        }

        //hàm in hóa đơn
        private void InHoaDonPDF(string maHD, string maCN)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = $"HoaDon_{maHD}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

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

                    // Font Unicode (tiếng Việt)
                    string fontPath = @"C:\Windows\Fonts\arial.ttf";
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                    // Tiêu đề
                    var paraHoaDonBanThuoc = new iTextSharp.text.Paragraph("HÓA ĐƠN BÁN THUỐC!", fontTitle);
                    paraHoaDonBanThuoc.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(paraHoaDonBanThuoc);

                    doc.Add(new iTextSharp.text.Paragraph($"Mã hóa đơn: {maHD}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Chi nhánh: {maCN} - {comboBox1.Text}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Nhân viên: {ma} - {tenNhanVien}", fontNormal));
                    doc.Add(new iTextSharp.text.Paragraph($"Khách hàng: {txtSDT.Text}", fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Bảng chi tiết
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3, 1, 1, 2, 2 });

                    // Header bảng
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Tên thuốc", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("SL", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("ĐVT", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Đơn giá", fontBold)));
                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Thành tiền", fontBold)));

                    // Dữ liệu từ dataGridView2
                    decimal tongTien = 0;
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells["TenThuoc"].Value != null)
                        {
                            string tenThuoc = row.Cells["TenThuoc"].Value.ToString();
                            string soLuong = row.Cells["SoLuong"].Value.ToString();
                            string dvt = row.Cells["DonViTinh"].Value.ToString();
                            string donGia = Convert.ToDecimal(row.Cells["DonGia"].Value).ToString("N0");
                            string thanhTien = Convert.ToDecimal(row.Cells["ThanhTien"].Value).ToString("N0");
                            tongTien += Convert.ToDecimal(row.Cells["ThanhTien"].Value);

                            table.AddCell(new iTextSharp.text.Phrase(tenThuoc, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(soLuong, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(dvt, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(donGia, fontNormal));
                            table.AddCell(new iTextSharp.text.Phrase(thanhTien, fontNormal));
                        }
                    }

                    doc.Add(table);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // tính tổng tiền - căn phải
                    var paraTongTien = new iTextSharp.text.Paragraph($"Tổng cộng: {tongTien:N0} VNĐ", fontBold);
                    paraTongTien.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    doc.Add(paraTongTien);

                    if (!string.IsNullOrEmpty(txtTienKH.Text) && decimal.TryParse(txtTienKH.Text, out decimal tienKhach))
                    {
                        var paraTienKhach = new iTextSharp.text.Paragraph($"Tiền khách đưa: {tienKhach:N0} VNĐ", fontNormal);
                        paraTienKhach.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(paraTienKhach);

                        var paraTienThua = new iTextSharp.text.Paragraph($"Tiền thối lại: {(tienKhach - tongTien):N0} VNĐ", fontNormal);
                        paraTienThua.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(paraTienThua);
                    }

                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Phần chân trang - CĂN GIỮA
                    var paraCamOn = new iTextSharp.text.Paragraph("Cảm ơn quý khách!", fontNormal);
                    paraCamOn.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(paraCamOn);

                    var paraHenGapLai = new iTextSharp.text.Paragraph("Hẹn gặp lại!", fontSmall);
                    paraHenGapLai.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(paraHenGapLai);

                    doc.Close();
                    writer.Close();
                    MessageBox.Show($"Đã xuất PDF thành công!\n{Path.GetFileName(saveDialog.FileName)}",
                                  "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(saveDialog.FileName); // Mở file PDF
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        //xem ds các CTHD
        private void btXem2_Click(object sender, EventArgs e)
        {
            FormXemCTHD f = new FormXemCTHD();
            f.ShowDialog();
        }

        // tính tiền thừa
        private void txtTienKH_KeyDown(object sender, KeyEventArgs e)
        {
            //nhập vào phải là số
            if (e.KeyCode == Keys.Enter)
            {
                if (!decimal.TryParse(txtTienKH.Text, out decimal tienKhach))
                {
                    MessageBox.Show("Vui lòng nhập số tiền hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTienKH.Focus();
                    txtTienKH.SelectAll();
                    return;
                }

                // Lấy tổng tiền từ label
                string tongText = lbTongTien.Text.Replace(" VNĐ", "").Replace(",", "");
                decimal tongTien = decimal.Parse(tongText);

                // 4. Kiểm tra tiền khách đưa có đủ không
                if (tienKhach < tongTien)
                {
                    MessageBox.Show($"Tiền khách đưa không đủ!\nCần thêm: {(tongTien - tienKhach):N0} VNĐ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTienKH.Focus();
                    txtTienKH.SelectAll();
                    return;
                }
                // Tính tiền thừa
                decimal tienThua = tienKhach - tongTien;
                lbTienThua.Text = tienThua.ToString("N0") + " VNĐ";
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        //tìm thuốc theo mã, tên và DVT
        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadLoThuocTheoChiNhanh(maChiNhanhHienTai); // load lại toàn bộ khi ô tìm trống
                return;
            }
            string sql = $@"
                SELECT 
                    lt.T_MA AS [Mã thuốc],
                    t.T_TEN AS [Tên thuốc],
                    ISNULL(SUM(ctpx.CTPX_SLTON), 0) AS [Số lượng tồn],
                    dvt.DVT_TEN AS [Đơn vị tính],
                    ISNULL(dg.LAST_PRICE, 0) AS [Đơn giá]
                FROM LOTHUOC lt
                INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                INNER JOIN DONVITINH dvt ON t.DVT_MA = dvt.DVT_MA
                INNER JOIN CTPX ctpx ON lt.LT_MA = ctpx.LT_MA AND lt.T_MA = ctpx.T_MA
                INNER JOIN PHIEUXUAT px ON ctpx.PX_MA = px.PX_MA 
                OUTER APPLY (
                    SELECT TOP 1 DG_DONGIA AS LAST_PRICE
                    FROM DONGIA 
                    WHERE T_MA = lt.T_MA 
                    ORDER BY NGAY DESC, THOIGIAN DESC
                ) AS dg
                WHERE px.CN_MA = '{maChiNhanhHienTai}'
                    AND (lt.T_MA LIKE '%{keyword}%' 
                         OR t.T_TEN LIKE N'%{keyword}%'
                         OR dvt.DVT_TEN LIKE N'%{keyword}%')
                GROUP BY lt.T_MA, t.T_TEN, dvt.DVT_TEN, dg.LAST_PRICE
                ORDER BY lt.T_MA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }


        private void txtTienKH_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTienKH.Text))
            {
                if (!int.TryParse(txtTienKH.Text, out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số tiền nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTienKH.Clear();
                    txtTienKH.Focus();
                }
            }
        }

        private void txtSL_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSL.Text))
            {
                if (!int.TryParse(txtSL.Text, out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSL.Clear();
                    txtSL.Focus();
                }
            }
        }
    }
}