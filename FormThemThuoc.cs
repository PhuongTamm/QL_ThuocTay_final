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
    public partial class FormThemThuoc : Form
    {
        SqlConnection conn = new SqlConnection();
        private string imagePath = "";
        ChucNang cn = new ChucNang();

        public FormThemThuoc()
        {
            InitializeComponent();
        }
        private void FormThemThuoc_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            cn.loadComboBox(comboBox1, "SELECT DVT_MA, DVT_TEN FROM DONVITINH", "DVT_MA", "DVT_TEN", conn);
        }

        private void btThemAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagePath = ofd.FileName;
                pictureBox1.Image = Image.FromFile(imagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        //Hàm sinh mã thuốc tự tăng dựa chữ cái đầu
        private string SinhMaThuoc(string tenThuoc)
        {
            if (string.IsNullOrWhiteSpace(tenThuoc))
                return "";
            string chucai = tenThuoc.Trim().Substring(0, 1).ToUpper();
            int maxNumber = 0;
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string sql = $"SELECT T_MA FROM THUOC WHERE T_MA LIKE '{chucai}%' ORDER BY T_MA DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string ma = dr.GetString(0);
                    if (int.TryParse(ma.Substring(1), out int n) && n > maxNumber)
                        maxNumber = n;
                }
                dr.Close();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return chucai + (maxNumber + 1).ToString("D3");
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            string tenThuoc = txtTenThuoc.Text.Trim();
            string thanhPhan = txtThanhPhan.Text.Trim();
            string dvt = comboBox1.SelectedValue?.ToString();
            string donGiaStr = txtDonGia.Text.Trim();

            if (string.IsNullOrEmpty(tenThuoc) || string.IsNullOrEmpty(dvt))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal donGia = 0;
            bool coDonGia = false;
            if (!string.IsNullOrWhiteSpace(donGiaStr))
            {
                if (!decimal.TryParse(donGiaStr, out donGia))
                {
                    MessageBox.Show("Đơn giá phải là số hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                coDonGia = (donGia > 0);

                if (donGia <= 0)
                {
                    MessageBox.Show("Đơn giá phải lớn hơn 0!!!.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                coDonGia = true;
            }

            string maThuoc = SinhMaThuoc(tenThuoc);

            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                // Thêm thuốc
                string sqlThuoc = @"INSERT INTO THUOC (T_MA, T_TEN, T_THANHPHAN, T_SLTON, T_HINHANH, DVT_MA)
                            VALUES (@T_MA, @T_TEN, @T_THANHPHAN, 0, @T_HINHANH, @DVT_MA)";
                SqlCommand cmdThuoc = new SqlCommand(sqlThuoc, conn, tran);
                cmdThuoc.Parameters.AddWithValue("@T_MA", maThuoc);
                cmdThuoc.Parameters.AddWithValue("@T_TEN", tenThuoc);
                cmdThuoc.Parameters.AddWithValue("@T_THANHPHAN", thanhPhan);
                cmdThuoc.Parameters.AddWithValue("@T_HINHANH", imagePath ?? "");
                cmdThuoc.Parameters.AddWithValue("@DVT_MA", dvt);
                cmdThuoc.ExecuteNonQuery();

                // CHỈ THÊM NGÀY VÀ ĐƠN GIÁ NẾU CÓ ĐƠN GIÁ
                if (coDonGia)
                {
                    string ngay = DateTime.Now.ToString("yyyy-MM-dd");
                    string thoiGian = DateTime.Now.ToString("HH:mm:ss");

                    // Kiểm tra ngày đã có chưa
                    SqlCommand checkNgay = new SqlCommand("SELECT COUNT(*) FROM NGAY WHERE NGAY = @NGAY", conn, tran);
                    checkNgay.Parameters.AddWithValue("@NGAY", ngay);
                    int exists = (int)checkNgay.ExecuteScalar();

                    if (exists == 0) //nếu chưa có thì thêm vào bảng NGAY
                    {
                        SqlCommand insertNgay = new SqlCommand("INSERT INTO NGAY (NGAY) VALUES (@NGAY)", conn, tran);
                        insertNgay.Parameters.AddWithValue("@NGAY", ngay);
                        insertNgay.ExecuteNonQuery();
                    }

                    // Thêm giá bán
                    SqlCommand cmdDG = new SqlCommand(@"INSERT INTO DONGIA (NGAY, T_MA, DG_DONGIA, THOIGIAN)
                                    VALUES (@NGAY, @T_MA, @DG_DONGIA, @THOIGIAN)", conn, tran);
                    cmdDG.Parameters.AddWithValue("@NGAY", ngay);
                    cmdDG.Parameters.AddWithValue("@T_MA", maThuoc);
                    cmdDG.Parameters.AddWithValue("@DG_DONGIA", donGia);
                    cmdDG.Parameters.AddWithValue("@THOIGIAN", thoiGian);
                    cmdDG.ExecuteNonQuery();
                }

                tran.Commit();  //ghi chính thức vào csdl
                MessageBox.Show("Thêm thuốc thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();    //hoàn tác nếu xảy ra lỗi
                MessageBox.Show("Lỗi khi thêm thuốc: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btThemCSV_Click(object sender, EventArgs e)
        { 
        }
    }
}
