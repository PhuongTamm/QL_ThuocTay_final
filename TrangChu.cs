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
    public partial class TrangChu : Form
    {
        SqlConnection conn = new SqlConnection();

        string chucVu;
        string maNV;

        public TrangChu(string nv, string cv)
        {
            InitializeComponent();
            chucVu = cv;
            maNV = nv;
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
            ChucNang cn = new ChucNang();
            cn.ketnoi(conn);
            string sql = @"
                    SELECT NV_TEN, CV_TEN 
                    FROM NHANVIEN N 
                    JOIN CHUCVU C ON N.CV_MA = C.CV_MA 
                    WHERE N.NV_MA = @ma";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma", maNV);
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                lbTenNV.Text = rd["NV_TEN"].ToString();
                lbCV.Text = rd["CV_TEN"].ToString();
            }
            rd.Close();

            PhanQuyen(chucVu);
        }

        private void PhanQuyen(string chucVu)
        {
            // Mặc định ẩn hết
            btHoaDon.Visible = false;
            btKhachHang.Visible = false;
            btThuoc.Visible = false;
            btNhanVien.Visible = false;
            btNhapThuoc.Visible = false;
            btPhanPhoi.Visible = false;
            btThongKe.Visible = false;

            // Hiện theo quyền
            switch (chucVu.ToLower())
            {
                case "cv01": // Admin
                    btHoaDon.Visible = true;
                    btKhachHang.Visible = true;
                    btThuoc.Visible = true;
                    btNhanVien.Visible = true;
                    btNhapThuoc.Visible = true;
                    btPhanPhoi.Visible = true;
                    btThongKe.Visible = true;
                    break;

                case "cv02": // Quản lý kho tổng
                    btThuoc.Visible = true;
                    btNhapThuoc.Visible = true;
                    btPhanPhoi.Visible = true;
                    btThongKe.Visible = true;
                    break;

                case "cv03": // Nhân viên bán hàng
                    btHoaDon.Visible = true;
                    btKhachHang.Visible = true;
                    //btThuoc.Visible = true;
                    break;

                case "cv04": // Quản lý chi nhánh
                    btHoaDon.Visible = true;
                    btKhachHang.Visible = true;
                    //btThuoc.Visible = true;
                    btNhanVien.Visible = true;
                    btThongKe.Visible = true;
                    break;

                default:
                    MessageBox.Show("Chức vụ không hợp lệ hoặc chưa được phân quyền!");
                    break;
            }
        }

        private void btDangXuat_Click(object sender, EventArgs e)
        {
            FormXNDangXuat xn = new FormXNDangXuat(this);
            xn.ShowDialog();
        }

        //logo click
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            TrangChu_Load(sender, e);
        }

        // Hàm load UserControl vào panelContent
        private void LoadContent(UserControl uc)
        {
            pnContent.Controls.Clear(); // xóa giao diện cũ
            uc.Dock = DockStyle.Fill;      // chiếm toàn bộ vùng hiển thị
            pnContent.Controls.Add(uc); // thêm giao diện mới
        }

        private void btAccount_Click(object sender, EventArgs e)
        {
            FormAccount xn = new FormAccount(maNV);
            xn.ShowDialog();
        }

        private void btHoaDon_Click(object sender, EventArgs e)
        {
            UserControlHoaDon uc = new UserControlHoaDon(maNV);
            LoadContent(uc);
        }

        private void btKhachHang_Click(object sender, EventArgs e)
        {
            UserControlKhachHang uc = new UserControlKhachHang();
            LoadContent(uc);
        }

        private void btThuoc_Click(object sender, EventArgs e)
        {
            UserControlThuoc uc = new UserControlThuoc();
            LoadContent(uc);
        }

        private void btNhanVien_Click(object sender, EventArgs e)
        {
            UserControlNhanVien uc = new UserControlNhanVien();
            LoadContent(uc);
        }

        private void btNhapThuoc_Click(object sender, EventArgs e)
        {
            UserControlNhapThuoc uc = new UserControlNhapThuoc(maNV);
            LoadContent(uc);
        }

        private void btPhanPhoi_Click(object sender, EventArgs e)
        {
            UserControlPhanPhoi uc = new UserControlPhanPhoi(maNV);
            LoadContent(uc);
        }

        private void btThongKe_Click(object sender, EventArgs e)
        {
            UserControlThongKe uc = new UserControlThongKe();
            LoadContent(uc);
        }

    }
}
