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
    public partial class FormXemLichSuNhap : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public FormXemLichSuNhap()
        {
            InitializeComponent();
        }

        private void FormXemLichSuNhap_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadLichSuNhap();
        }

        private void LoadLichSuNhap(string keyword = "")
        {
            string sql = @"
            SELECT 
                pn.PN_MA AS [Mã phiếu],
                FORMAT(pn.PN_NGAYNHAP, 'dd/MM/yyyy HH:mm') AS [Ngày nhập],
                FORMAT(pn.PN_TRIGIA, 'N0') AS [Trị giá],
                t.T_TEN AS [Tên thuốc],
                lt.T_MA AS [Mã thuốc],
                lt.LT_MA AS [Mã lô],
                FORMAT(lt.LT_NGAYSX, 'dd/MM/yyyy') AS [Ngày SX],
                FORMAT(lt.LT_NGAYHD, 'dd/MM/yyyy') AS [Hạn dùng],
                lt.LT_SLNHAP AS [SL nhập],
                dvt.DVT_TEN AS [Đơn vị tính],
                FORMAT(lt.LT_DGNHAP, 'N0') AS [Đơn giá],
                lt.LT_SLTON AS [SL tồn],
                FORMAT(lt.LT_SLNHAP * lt.LT_DGNHAP, 'N0') AS [Thành tiền],
                nv.NV_MA AS [Nhân viên nhập]
            FROM PHIEUNHAP pn
            INNER JOIN LOTHUOC lt ON pn.PN_MA = lt.PN_MA
            INNER JOIN THUOC t ON lt.T_MA = t.T_MA
            INNER JOIN DONVITINH dvt ON t.DVT_MA = dvt.DVT_MA
            LEFT JOIN NHANVIEN nv ON pn.NV_MA = nv.NV_MA
            WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (pn.PN_MA LIKE '%" + keyword + "%' OR t.T_TEN LIKE N'%" + keyword + "%' OR nv.NV_MA LIKE N'%" + keyword + "%')";
            }
            sql += " ORDER BY pn.PN_NGAYNHAP DESC, pn.PN_MA, lt.LT_MA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();
            LoadLichSuNhap(keyword);
        }
    }
}
