using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace QL_ThuocTay
{
    public partial class DangNhap : Form
    {
        SqlConnection conn = new SqlConnection();
        public DangNhap()
        {
            InitializeComponent();
        }

        private void buttonDN_Click(object sender, EventArgs e)
        {
            Check_Login();
        }


        private void DangNhap_Load(object sender, EventArgs e)
        {
            ChucNang cn = new ChucNang();
            cn.ketnoi(conn);
            txtMK.PasswordChar = '*';

        }

        private void Check_Login()
        {
            string maNV = txtMa.Text.Trim();
            string matKhau = txtMK.Text;

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            string hashedInput = ComputeSha256Hash(matKhau);
            string sql = @"SELECT CV_MA, NV_TRANGTHAI FROM NHANVIEN WHERE NV_MA = @ma AND NV_MATKHAU = @mk";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maNV);
                cmd.Parameters.AddWithValue("@mk", hashedInput);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Thông tin đăng nhập không đúng!");
                        return;
                    }

                    string trangThai = reader["NV_TRANGTHAI"].ToString().Trim();
                    string chucVu = reader["CV_MA"].ToString().Trim();
                    if (trangThai.Equals("ngừng hoạt động", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Tài khoản này đã bị ngừng hoạt động!");
                        return;
                    }

                    //mở form Trang Chủ
                    TrangChu frm = new TrangChu(maNV, chucVu);
                    frm.Show();
                    this.Hide();
                }
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

        private void buttonDN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Check_Login();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
