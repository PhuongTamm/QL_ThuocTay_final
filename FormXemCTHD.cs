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

    public partial class FormXemCTHD : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public FormXemCTHD()
        {
            InitializeComponent();
        }

        private void FormXemCTHD_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            string sql = @"
                SELECT 
                    cthd.HD_MA AS [Mã HĐ],
                    cthd.LT_MA AS [Mã lô],
                    cthd.T_MA AS [Mã thuốc],
                    t.T_TEN AS [Tên thuốc],
                    cthd.CTHD_SOLUONG AS [Số lượng],
                    cthd.CTHD_DONGIA AS [Đơn giá],
                    (cthd.CTHD_SOLUONG * cthd.CTHD_DONGIA) AS [Thành tiền],
                    hd.HD_NGAYLAP AS [Ngày lập],
                    hd.KH_SDT AS [SĐT khách hàng],
                    hd.NV_MA AS [Mã NV],
                    hd.CN_MA AS [Mã CN]
                FROM CTHD cthd
                INNER JOIN HOADON hd ON cthd.HD_MA = hd.HD_MA
                INNER JOIN THUOC t ON cthd.T_MA = t.T_MA
                ORDER BY cthd.HD_MA DESC";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            string sql = $@"
                SELECT 
                    cthd.HD_MA AS [Mã HĐ],
                    cthd.LT_MA AS [Mã lô],
                    cthd.T_MA AS [Mã thuốc],
                    t.T_TEN AS [Tên thuốc],
                    cthd.CTHD_SOLUONG AS [Số lượng],
                    cthd.CTHD_DONGIA AS [Đơn giá],
                    (cthd.CTHD_SOLUONG * cthd.CTHD_DONGIA) AS [Thành tiền],
                    hd.HD_NGAYLAP AS [Ngày lập],
                    hd.KH_SDT AS [SĐT khách hàng],
                    hd.NV_MA AS [Mã NV],
                    hd.CN_MA AS [Mã CN]
                FROM CTHD cthd
                INNER JOIN HOADON hd ON cthd.HD_MA = hd.HD_MA
                INNER JOIN THUOC t ON cthd.T_MA = t.T_MA
                WHERE cthd.HD_MA LIKE '%{keyword}%'
                   OR cthd.T_MA LIKE '%{keyword}%'
                   OR t.T_TEN LIKE '%{keyword}%'
                   OR hd.KH_SDT LIKE '%{keyword}%'
                ORDER BY cthd.HD_MA DESC";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }
    }
}
