using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_ThuocTay
{
    internal class ChucNang
    {
        public void ketnoi(SqlConnection conn)
        {
            string chuoiketnoi = "Server = DESKTOP-I3L1TB8\\SQLEXPRESS ; Database = QLTHUOCTAY ; Integrated Security = True";

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = chuoiketnoi;
                conn.Open();
            }
        }


        public void load_dataGridView(SqlConnection conn, string sql, DataGridView dg)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "name");
            dg.DataSource = ds;
            dg.DataMember = "name";
        }

        public void loadComboBox(ComboBox cb, string sql, string valueMember, string displayMember, SqlConnection conn)
        {
            try
            {
                // Đảm bảo kết nối mở
                if (conn.State == ConnectionState.Closed)
                    ketnoi(conn);

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cb.DataSource = dt;
                cb.ValueMember = valueMember;
                cb.DisplayMember = displayMember;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message);
            }
        }
    }
}
