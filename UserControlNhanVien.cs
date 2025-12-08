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
    public partial class UserControlNhanVien : UserControl
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();

        public UserControlNhanVien()
        {
            InitializeComponent();
        }
        private void UserControlNhanVien_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadNhanVien();    // Hiển thị danh sách ban đầu
        }
        private void LoadNhanVien(string keyword = "")
        {
            string sql = @"
                SELECT NV_MA AS [Mã NV],
                       NV_TEN AS [Tên nhân viên],
                       NV_NGAYSINH AS [Ngày sinh],
                       NV_GIOITINH AS [Giới tính],
                       NV_DIACHI AS [Địa chỉ],
                       NV_EMAIL AS [Email],
                       NV_SDT AS [Số điện thoại],
                       CV_TEN AS [Chức vụ],
                       NV_HINHANH AS [Hình ảnh],
                       NV_TRANGTHAI AS [Trạng thái]
                FROM NHANVIEN
                LEFT JOIN CHUCVU ON NHANVIEN.CV_MA = CHUCVU.CV_MA";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " WHERE NV_MA LIKE N'%" + keyword + "%' OR NV_TEN LIKE N'%" + keyword + "%'";
            }

            cn.load_dataGridView(conn, sql, dgNhanVien);
            dgNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgNhanVien.Columns["Hình ảnh"].Visible = false;
        }

        private void dgNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgNhanVien.Rows[e.RowIndex];

                lbMaNV.Text = row.Cells[0].Value?.ToString();
                lbTenNV.Text = row.Cells[1].Value?.ToString();
                lbNgaySinh.Text = row.Cells[2].Value?.ToString();
                lbGioiTinh.Text = row.Cells[3].Value?.ToString();
                lbDiaChi.Text = row.Cells[4].Value?.ToString();
                lbEmail.Text = row.Cells[5].Value?.ToString();
                lbSDT.Text = row.Cells[6].Value?.ToString();
                lbChucVu.Text = row.Cells[7].Value?.ToString();

                // nếu có ảnh trong DB (lưu dạng đường dẫn)
                string imgPath = row.Cells[8].Value?.ToString();
                if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                {
                    pictureBox1.Image = Image.FromFile(imgPath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.Image = null;
                }

                lbTrangThai.Text = row.Cells[9].Value?.ToString();
                if (lbTrangThai.Text == "Đang hoạt động")
                {
                    lbTrangThai.ForeColor = Color.Green;
                }
                else if (lbTrangThai.Text == "Ngừng hoạt động")
                {
                    lbTrangThai.ForeColor = Color.Red;
                }
                else
                {
                    lbTrangThai.ForeColor = Color.Black;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadNhanVien();    // Hiển thị danh sách ban đầu

            lbMaNV.Text = "";
            lbTenNV.Text = "";
            lbChucVu.Text = "";
            lbNgaySinh.Text = "";
            lbGioiTinh.Text = "";
            lbDiaChi.Text = "";
            lbEmail.Text = "";
            lbSDT.Text = "";
            lbTrangThai.Text = "";
            lbTrangThai.Text = "";
            pictureBox1.Image = null;
            txtTim.Clear();
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            LoadNhanVien(keyword);
        }
    }
}
