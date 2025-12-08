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
    public partial class FormXemLichSuXuat : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public FormXemLichSuXuat()
        {
            InitializeComponent();
        }

        private void FormXemLichSuXuat_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadLichSuXuat();
        }

        private void LoadLichSuXuat(string keyword = "")
        {
            string sql = @"
                SELECT 
                    px.PX_MA AS [Mã phiếu xuất],
                    FORMAT(px.PX_NGAYXUAT, 'dd/MM/yyyy HH:mm') AS [Ngày xuất],
                    lt.PN_MA AS [Mã phiếu nhập],
                    t.T_TEN AS [Tên thuốc],
                    lt.LT_MA AS [Mã lô],
                    FORMAT(lt.LT_NGAYSX, 'MM/dd/yyyy') AS [Ngày SX],
                    FORMAT(lt.LT_NGAYHD, 'MM/dd/yyyy') AS [Hạn SD],
                    ctpx.CTPX_SOLUONG AS [Số lượng],
                    ctpx.CTPX_SLTON AS [Số lượng tồn],
                    dvt.DVT_TEN AS [Đơn vị tính],
                    lt.LT_DGNHAP AS [Đơn giá nhập],
                    FORMAT(ctpx.CTPX_SOLUONG * lt.LT_DGNHAP, 'N0') AS [Thành tiền],
                    cn.CN_MA AS [Mã chi nhánh],
                    nv.NV_MA AS [Mã nhân viên]
                FROM PHIEUXUAT px
                INNER JOIN CTPX ctpx ON px.PX_MA = ctpx.PX_MA
                INNER JOIN LOTHUOC lt ON ctpx.LT_MA = lt.LT_MA AND ctpx.T_MA = lt.T_MA
                INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                INNER JOIN DONVITINH dvt ON t.DVT_MA = dvt.DVT_MA
                INNER JOIN CHINHANH cn ON px.CN_MA = cn.CN_MA
                LEFT JOIN NHANVIEN nv ON px.NV_MA = nv.NV_MA
                WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += @" AND (px.PX_MA LIKE '%" + keyword + "%' OR lt.LT_MA LIKE '%" + keyword + "%'  OR lt.PN_MA LIKE '%" + keyword + "%' OR cn.CN_MA LIKE '%" + keyword + "%')";
            }

            sql += " ORDER BY px.PX_NGAYXUAT DESC, px.PX_MA, lt.LT_MA";

            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();
            LoadLichSuXuat(keyword);
        }
    }
}
