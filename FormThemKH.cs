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

namespace QL_ThuocTay
{
    public partial class FormThemKH : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public FormThemKH()
        {
            InitializeComponent();
        }

        private void FormThemKH_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            comboBox1.SelectedIndex = 0; // chọn item đầu tiên
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            string ten = txtTenKH.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string gioiTinh = comboBox1.Text.Trim();

            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(gioiTinh))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sdt.Length != 10 || !sdt.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sqlCheck = "SELECT COUNT(*) FROM KHACHHANGTHANHVIEN WHERE KH_SDT = @sdt";
            using (SqlCommand cmd = new SqlCommand(sqlCheck, conn))
            {
                cmd.Parameters.AddWithValue("@sdt", sdt);
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Khách hàng này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            string sqlInsert = "INSERT INTO KHACHHANGTHANHVIEN (KH_TEN, KH_GIOITINH, KH_SDT) VALUES (@ten, @gioitinh, @sdt)";
            using (SqlCommand cmd = new SqlCommand(sqlInsert, conn))
            {
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@gioitinh", gioiTinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Thêm khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
