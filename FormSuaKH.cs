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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QL_ThuocTay
{
    public partial class FormSuaKH : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        private string sdtCu; // lưu sđt gốc
        public FormSuaKH(string sdt, string ten, string gioiTinh)
        {
            InitializeComponent();
            sdtCu = sdt;
            txtSDT.Text = sdt;
            txtTenKH.Text = ten;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.AddRange(new string[] { "Nam", "Nữ" });
            comboBox1.SelectedItem = gioiTinh;

            // Gán lại giá trị ban đầu vào Tag (sau khi đã set SelectedItem)
            txtTenKH.Tag = ten.Trim();
            comboBox1.Tag = gioiTinh.Trim();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSuaKH_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            string tenMoi = txtTenKH.Text.Trim();
            string gioiTinhMoi = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tenMoi) || string.IsNullOrEmpty(gioiTinhMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenCu = txtTenKH.Tag?.ToString()?.Trim();
            string gioiTinhCu = comboBox1.Tag?.ToString()?.Trim();
            // Kiểm tra xem có thay đổi gì không
            bool khongThayDoi = string.Equals(tenMoi, tenCu, StringComparison.OrdinalIgnoreCase)
                                && string.Equals(gioiTinhMoi, gioiTinhCu, StringComparison.OrdinalIgnoreCase);
            if (khongThayDoi)
            {
                MessageBox.Show("Không có thay đổi nào để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sql = "UPDATE KHACHHANGTHANHVIEN SET KH_TEN = @ten, KH_GIOITINH = @gioitinh WHERE KH_SDT = @sdt";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ten", tenMoi);
                cmd.Parameters.AddWithValue("@gioitinh", gioiTinhMoi);
                cmd.Parameters.AddWithValue("@sdt", sdtCu);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
