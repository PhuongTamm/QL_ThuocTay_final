using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_ThuocTay
{
    public partial class FormDoiMK : Form
    {
        SqlConnection conn = new SqlConnection();

        string ma;
        public FormDoiMK(string maNV)
        {
            InitializeComponent();
            ma = maNV;
        }

        private void FormDoiMK_Load(object sender, EventArgs e)
        {
            ChucNang cn = new ChucNang();
            cn.ketnoi(conn);
            txtMKMoi.PasswordChar = '*';
            txtNhapLaiMK.PasswordChar = '*';
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btCapNhatMK_Click(object sender, EventArgs e)
        {
            string mkMoi = txtMKMoi.Text.Trim();
            string mkNhapLai = txtNhapLaiMK.Text.Trim();

            // Kiểm tra các điều kiện
            if (string.IsNullOrEmpty(mkMoi) || string.IsNullOrEmpty(mkNhapLai))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mkMoi != mkNhapLai)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hash mật khẩu mới để so với mật khẩu cũ
            string mkMoiHash = ComputeSha256Hash(mkMoi);
            string mkCuHash = LayMatKhauCu(ma);
            if (mkCuHash == mkMoiHash)
            {
                MessageBox.Show("Mật khẩu trùng với mật khẩu cũ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật mật khẩu mới
            string sql = "UPDATE NHANVIEN SET NV_MATKHAU = @mkMoi WHERE NV_MA = @ma";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@mkMoi", mkMoiHash);
                cmd.Parameters.AddWithValue("@ma", ma);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật mật khẩu thành công!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private string LayMatKhauCu(string ma)
        {
            string sql = "SELECT NV_MATKHAU FROM NHANVIEN WHERE NV_MA = @ma";
            if (conn.State != ConnectionState.Open)
                conn.Open(); // mở kết nối nếu đang đóng
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", ma);
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString().Trim() : "";
            }
        }

        private string ComputeSha256Hash(string raw)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                //byte[] bytes = Encoding.UTF8.GetBytes(raw);
                byte[] bytes = Encoding.Unicode.GetBytes(raw);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2")); // HEX in hoa
                return sb.ToString();
            }
        }
    }
}
