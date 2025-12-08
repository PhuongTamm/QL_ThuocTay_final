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
    public partial class FormAccount : Form
    {
        SqlConnection conn = new SqlConnection();
        string ma;
        public FormAccount(string maNV)
        {
            InitializeComponent();
            ma = maNV;
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btCapNhat_Click(object sender, EventArgs e)
        {

        }

        private void FormAccount_Load(object sender, EventArgs e)
        {
            LoadThongTinNhanVien();
        }

        private void LoadThongTinNhanVien()
        {
            ChucNang cn = new ChucNang();
            cn.ketnoi(conn);
            string sql = @"
                    SELECT N.NV_MA, N.NV_TEN, N.NV_NGAYSINH, N.NV_GIOITINH, N.NV_DIACHI,
                           N.NV_EMAIL, N.NV_SDT, C.CV_TEN, N.NV_HINHANH
                    FROM NHANVIEN N
                    JOIN CHUCVU C ON N.CV_MA = C.CV_MA
                    WHERE N.NV_MA = @ma";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma", ma);
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                lbMaNV.Text = rd["NV_MA"].ToString();
                lbTenNV.Text = rd["NV_TEN"].ToString();
                lbNgaySinh.Text = Convert.ToDateTime(rd["NV_NGAYSINH"]).ToString("dd/MM/yyyy");
                lbGioiTinh.Text = rd["NV_GIOITINH"].ToString();
                lbDiaChi.Text = rd["NV_DIACHI"].ToString();
                lbEmail.Text = rd["NV_EMAIL"].ToString();
                lbSDT.Text = rd["NV_SDT"].ToString();
                lbChucVu.Text = rd["CV_TEN"].ToString();
                string imagePath = rd["NV_HINHANH"].ToString();

                //if (!string.IsNullOrEmpty(imagePath))
                //{
                //    if (System.IO.File.Exists(imagePath))
                //        pictureBox1.Image = new Bitmap(imagePath);
                //        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // cho vừa khung
                //}
                try
                {
                    if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                    {
                        pictureBox1.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        // ảnh mặc định nếu nhân viên chưa có hình
                        pictureBox1.Image = null;
                    }

                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch
                {
                    pictureBox1.Image = null;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            rd.Close();
        }

        private void btDoiMK_Click_1(object sender, EventArgs e)
        {
            FormDoiMK doiMK = new FormDoiMK(ma);
            this.Hide();
            doiMK.ShowDialog();
            this.Show();
            LoadThongTinNhanVien();
        }
    }
}
