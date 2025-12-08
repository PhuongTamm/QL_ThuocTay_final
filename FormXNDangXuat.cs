using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace QL_ThuocTay
{
    public partial class FormXNDangXuat : Form
    {
        private TrangChu formTrangChu;

        //SqlConnection conn = new SqlConnection();

        public FormXNDangXuat(TrangChu frm)
        {
            InitializeComponent();
            formTrangChu = frm;
        }

        private void btKhong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btCo_Click(object sender, EventArgs e)
        {
            formTrangChu.Hide(); // ẩn form TrangChu
            DangNhap dn = new DangNhap();
            dn.Show();
            this.Close();
        }

        private void FormXNDangXuat_Load(object sender, EventArgs e)
        {
            //ChucNang cn = new ChucNang();
            //cn.ketnoi(conn);
        }
    }
}
