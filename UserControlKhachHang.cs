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
    public partial class UserControlKhachHang : UserControl
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public UserControlKhachHang()
        {
            InitializeComponent();
        }

        private void UserControlKhachHang_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadKhachHang();
        }

        private void LoadKhachHang(string keyword ="")
        {
            string sql = @"
                    SELECT KH_TEN AS [Tên khách hàng],
                           KH_GIOITINH AS [Giới tính],
                           KH_SDT AS [Số điện thoại]
                    FROM KHACHHANGTHANHVIEN 
                    WHERE KH_SDT LIKE '%" + keyword + "%' OR KH_TEN LIKE N'%" + keyword + "%' " +
                    "ORDER BY KH_TEN";
            cn.load_dataGridView(conn, sql, dgKhachHang);
            dgKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadKhachHang();    // Hiển thị danh sách ban đầu
            lbTenKH.Text = "";
            lbGioiTinh.Text = "";
            lbSDT.Text = "";
            txtTim.Clear();
        }

        private void dgKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgKhachHang.Rows[e.RowIndex];

                // Lấy dữ liệu từ các cột
                lbTenKH.Text = row.Cells[0].Value?.ToString();
                lbGioiTinh.Text = row.Cells[1].Value?.ToString();
                lbSDT.Text = row.Cells[2].Value?.ToString();
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            FormThemKH f = new FormThemKH();
            f.ShowDialog();
            LoadKhachHang(); // Cập nhật lại danh sách sau khi thêm
            lbTenKH.Text = "";
            lbGioiTinh.Text = "";
            lbSDT.Text = "";
            txtTim.Clear();
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbSDT.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Truyền dữ liệu sang form sửa
            string sdt = lbSDT.Text;
            string ten = lbTenKH.Text;
            string gioitinh = lbGioiTinh.Text;

            FormSuaKH f = new FormSuaKH(sdt, ten, gioitinh);
            f.ShowDialog();

            LoadKhachHang();
            lbTenKH.Text = "";
            lbGioiTinh.Text = "";
            lbSDT.Text = "";
            txtTim.Clear();
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            LoadKhachHang(keyword);
        }
    }
}
