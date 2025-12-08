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
    public partial class FormXemDonGia : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public FormXemDonGia()
        {
            InitializeComponent();
        }

        private void FormXemDonGia_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            string sql = "SELECT NGAY AS [NGÀY], T_MA AS [MÃ THUỐC], DG_DONGIA AS [ĐƠN GIÁ], THOIGIAN AS [THỜI GIAN] FROM DONGIA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }
    }
}
