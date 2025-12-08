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
    public partial class FormSuaThuoc : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        string maThuocCu;
        string oldImagePath = "";

        public FormSuaThuoc(string maThuoc, string tenThuoc, string thanhPhan, string donGia)
        {
            InitializeComponent();
            maThuocCu = maThuoc;

            // Gán dữ liệu ban đầu vào controls
            txtMa.Text = maThuoc;
            txtTenThuoc.Text = tenThuoc;
            txtThanhPhan.Text = thanhPhan;
            txtDonGia.Text = donGia;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList; 

            //Gán giá trị ban đầu vào Tag để so sánh sau này
            txtTenThuoc.Tag = tenThuoc.Trim();
            txtThanhPhan.Tag = thanhPhan.Trim();
            txtDonGia.Tag = donGia.Trim();

        }

        private void FormSuaThuoc_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            string sqlDVT = "SELECT DVT_MA, DVT_TEN FROM DONVITINH";    //load ds DVT
            cn.loadComboBox(comboBox1, sqlDVT, "DVT_MA", "DVT_TEN", conn);

            // load hình ảnh theo mã thuốc
            string sql = "SELECT T_HINHANH, DVT_MA FROM THUOC WHERE T_MA = @ma";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma", maThuocCu);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                // SET ĐÚNG GIÁ TRỊ CHO COMBOBOX
                string dvtMa = dr["DVT_MA"].ToString();
                comboBox1.SelectedValue = dvtMa;

                oldImagePath = dr["T_HINHANH"].ToString();
                // Hiển thị ảnh nếu có
                if (!string.IsNullOrEmpty(oldImagePath) && File.Exists(oldImagePath))
                {
                    pictureBox1.Image = Image.FromFile(oldImagePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            dr.Close();
        }

        private void btThemAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Ảnh (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
                pictureBox1.Tag = ofd.FileName; // Lưu tạm đường dẫn ảnh mới
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            string tenMoi = txtTenThuoc.Text.Trim();
            string thanhPhanMoi = txtThanhPhan.Text.Trim();
            string dvtMoi = comboBox1.SelectedValue?.ToString();
            string donGiaStrMoi = txtDonGia.Text.Trim();
            string imagePathMoi = pictureBox1.Tag?.ToString() ?? oldImagePath;

            // Kiểm tra thông tin bắt buộc
            if (string.IsNullOrEmpty(tenMoi) || string.IsNullOrEmpty(dvtMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!!!.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra đơn giá, chỉ cập nhật nếu > 0
            decimal donGiaMoi = 0;
            bool coDonGiaMoi = false;
            if (!string.IsNullOrWhiteSpace(donGiaStrMoi))
            {
                if (!decimal.TryParse(donGiaStrMoi, out donGiaMoi))
                {
                    MessageBox.Show("Đơn giá phải là số hợp lệ!!!.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                coDonGiaMoi = (donGiaMoi > 0);

                if (donGiaMoi <= 0)
                {
                    MessageBox.Show("Đơn giá phải lớn hơn 0!!!.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                coDonGiaMoi = true;
            }

            //kiểm tra có thay đổi thì cập nhật
            // Lấy thông tin cũ từ Tag
            string tenCu = txtTenThuoc.Tag?.ToString() ?? "";
            string thanhPhanCu = txtThanhPhan.Tag?.ToString() ?? "";
            string donGiaStrCu = txtDonGia.Tag?.ToString() ?? "0";
            decimal donGiaCu = 0;
            decimal.TryParse(donGiaStrCu, out donGiaCu);

            // Lấy DVT cũ từ database (vì không lưu trong Tag)
            string dvtCu = "";
            string sqlGetDVTCu = "SELECT DVT_MA FROM THUOC WHERE T_MA = @ma";
            SqlCommand cmdGetDVTCu = new SqlCommand(sqlGetDVTCu, conn);
            cmdGetDVTCu.Parameters.AddWithValue("@ma", maThuocCu);
            object resultDVT = cmdGetDVTCu.ExecuteScalar();
            if (resultDVT != null)
            {
                dvtCu = resultDVT.ToString();
            }

            // Kiểm tra xem có thay đổi gì không 
            bool thayDoiThongTin = !string.Equals(tenMoi, tenCu, StringComparison.OrdinalIgnoreCase)
                                    || !string.Equals(thanhPhanMoi, thanhPhanCu, StringComparison.OrdinalIgnoreCase)
                                    || !string.Equals(dvtMoi, dvtCu, StringComparison.OrdinalIgnoreCase)
                                    || !string.Equals(imagePathMoi, oldImagePath, StringComparison.OrdinalIgnoreCase);

            bool thayDoiDonGia = coDonGiaMoi && donGiaMoi != donGiaCu;

            // Nếu không có thay đổi gì
            if (!thayDoiThongTin && !thayDoiDonGia)
            {
                MessageBox.Show("Không có thay đổi nào để cập nhật!!!.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Bắt đầu transaction
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                // Cập nhật thông tin thuốc
                string sqlUpdateThuoc = @"UPDATE THUOC 
                                     SET T_TEN = @ten, 
                                         T_THANHPHAN = @thanhphan, 
                                         DVT_MA = @dvt, 
                                         T_HINHANH = @hinhanh 
                                     WHERE T_MA = @ma";

                using (SqlCommand cmdUpdateThuoc = new SqlCommand(sqlUpdateThuoc, conn, tran))
                {
                    cmdUpdateThuoc.Parameters.AddWithValue("@ten", tenMoi);
                    cmdUpdateThuoc.Parameters.AddWithValue("@thanhphan", thanhPhanMoi);
                    cmdUpdateThuoc.Parameters.AddWithValue("@dvt", dvtMoi);
                    cmdUpdateThuoc.Parameters.AddWithValue("@hinhanh", imagePathMoi);
                    cmdUpdateThuoc.Parameters.AddWithValue("@ma", maThuocCu);

                    int rowsThuoc = cmdUpdateThuoc.ExecuteNonQuery();
                    if (rowsThuoc == 0)
                    {
                        tran.Rollback();
                        MessageBox.Show("Không tìm thấy thuốc để cập nhật!!!.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Xử lý đơn giá - CHỈ CẬP NHẬT NẾU CÓ ĐƠN GIÁ MỚI VÀ > 0
                if (coDonGiaMoi && donGiaMoi != donGiaCu)
                {
                    string ngayHienTai = DateTime.Now.ToString("yyyy-MM-dd");
                    string thoiGianHienTai = DateTime.Now.ToString("HH:mm:ss");

                    // Đảm bảo ngày tồn tại
                    string sqlCheckNgay = "IF NOT EXISTS (SELECT 1 FROM NGAY WHERE NGAY = @ngay) INSERT INTO NGAY (NGAY) VALUES (@ngay)";
                    using (SqlCommand cmdCheckNgay = new SqlCommand(sqlCheckNgay, conn, tran))
                    {
                        cmdCheckNgay.Parameters.AddWithValue("@ngay", ngayHienTai);
                        cmdCheckNgay.ExecuteNonQuery();
                    }

                    // THÊM MỚI ĐƠN GIÁ VỚI THỜI GIAN HIỆN TẠI
                    string sqlInsertDonGia = "INSERT INTO DONGIA (NGAY, THOIGIAN, T_MA, DG_DONGIA) VALUES (@ngay, @thoigian, @ma, @dongia)";
                    using (SqlCommand cmdInsertDonGia = new SqlCommand(sqlInsertDonGia, conn, tran))
                    {
                        cmdInsertDonGia.Parameters.AddWithValue("@ngay", ngayHienTai);
                        cmdInsertDonGia.Parameters.AddWithValue("@thoigian", thoiGianHienTai);
                        cmdInsertDonGia.Parameters.AddWithValue("@ma", maThuocCu);
                        cmdInsertDonGia.Parameters.AddWithValue("@dongia", donGiaMoi);
                        cmdInsertDonGia.ExecuteNonQuery();
                    }
                                }
                tran.Commit();
                MessageBox.Show("Cập nhật thông tin thuốc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("Lỗi khi cập nhật thuốc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
