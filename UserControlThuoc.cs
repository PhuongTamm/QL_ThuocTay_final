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
    public partial class UserControlThuoc : UserControl
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();

        public UserControlThuoc()
        {
            InitializeComponent();
        }

        private void UserControlThuoc_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadThuoc();
        }

        private void LoadThuoc(string keyword = "")
        {
            string sql = @"
                SELECT THUOC.T_MA AS [Mã thuốc],
                    THUOC.T_TEN As [Tên thuốc],
                    THUOC.T_SLTON as [Số lượng tồn],
                    THUOC.T_THANHPHAN as [Thành phần],
                    DONVITINH.DVT_TEN as [Đơn vị tính],
                    ISNULL(DG.DG_DONGIA, 0) as [Đơn giá bán],
                    THUOC.T_HINHANH as [Hình ảnh]
                 FROM THUOC
                 INNER JOIN DONVITINH ON THUOC.DVT_MA = DONVITINH.DVT_MA
                 OUTER APPLY (
                    SELECT TOP 1 DG_DONGIA 
                    FROM DONGIA 
                    WHERE T_MA = THUOC.T_MA 
                    ORDER BY NGAY DESC, THOIGIAN DESC
                 ) AS DG
                 WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (THUOC.T_MA LIKE N'%" + keyword + "%' OR THUOC.T_TEN LIKE N'%" + keyword + "%')";
            }
            sql += " ORDER BY THUOC.T_MA";
            cn.load_dataGridView(conn, sql, dgThuoc);
            dgThuoc.Columns["Hình ảnh"].Visible = false; // Ẩn cột đường dẫn ảnh
        }

        private void txtTim_TextChanged_1(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            LoadThuoc(keyword);
        }

        private void dgThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgThuoc.Rows[e.RowIndex];

                lbMaThuoc.Text = row.Cells[0].Value?.ToString();
                lbTenThuoc.Text = row.Cells[1].Value?.ToString();
                lbSLTon.Text = row.Cells[2].Value?.ToString();
                lbThanhPhan.Text = row.Cells[3].Value?.ToString();
                lbDVT.Text = row.Cells[4].Value?.ToString();
                lbDonGia.Text = row.Cells[5].Value?.ToString();

                // nếu có ảnh trong DB (lưu dạng đường dẫn)
                string imgPath = row.Cells[6].Value?.ToString();
                if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                {
                    pictureBox1.Image = Image.FromFile(imgPath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void btLamMoi_Click(object sender, EventArgs e)
        {
            LoadThuoc();    // Hiển thị danh sách ban đầu

            lbMaThuoc.Text = "";
            lbTenThuoc.Text = "";
            lbSLTon.Text = "";
            lbThanhPhan.Text = "";
            lbDVT.Text = "";
            lbDonGia.Text = "";
            pictureBox1.Image = null;
            txtTim.Clear();
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            FormThemThuoc f = new FormThemThuoc(); 
            f.ShowDialog();
            LoadThuoc();
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lbMaThuoc.Text))
            {
                MessageBox.Show("Vui lòng chọn thuốc cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Truyền dữ liệu sang form sửa
            string maThuoc = lbMaThuoc.Text;
            string tenThuoc = lbTenThuoc.Text;
            string thanhPhan = lbThanhPhan.Text;
            //string dvt = lbDVT.Tag?.ToString(); // Lấy mã DVT từ Tag thay vì Text để truyền mã dvt thay vì tên
            string donGia = lbDonGia.Text;

            FormSuaThuoc f = new FormSuaThuoc(maThuoc, tenThuoc, thanhPhan, donGia);
            f.ShowDialog();

            LoadThuoc();
            lbMaThuoc.Text = "";
            lbTenThuoc.Text = "";
            lbDVT.Text = "";
            lbDonGia.Text = "";
            lbSLTon.Text = "";
            lbThanhPhan.Text = "";
            pictureBox1.Image = null;
            txtTim.Clear();
        }

        private void btXem_Click(object sender, EventArgs e)
        {
            FormXemDonGia f = new FormXemDonGia();
            f.ShowDialog();
        }

        private void btXem2_Click(object sender, EventArgs e)
        {
            FormXemLoThuocKho f = new FormXemLoThuocKho();
            f.ShowDialog();
        }
    }
}
