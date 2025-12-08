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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace QL_ThuocTay
{
    public partial class FormXemLoThuocKho : Form
    {
        SqlConnection conn = new SqlConnection();
        ChucNang cn = new ChucNang();
        public FormXemLoThuocKho()
        {
            InitializeComponent();
        }

        private void FormXemLoThuocKho_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            string sql = $@"
                SELECT 
                    lt.LT_MA AS [Mã lô],
                    lt.T_MA AS [Mã thuốc],
                    t.T_TEN AS [Tên thuốc],
                    lt.LT_NGAYSX AS [Ngày sản xuất],
                    lt.LT_NGAYHD AS [Hạn dùng],
                    lt.LT_SLTON AS [Số lượng tồn],
                    dvt.DVT_TEN AS [Đơn vị tính],
                    lt.LT_DGNHAP AS [Đơn giá nhập],
                    dg.LAST_PRICE AS [Đơn giá bán]
                FROM LOTHUOC lt 
                INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                INNER JOIN DONVITINH dvt ON t.DVT_MA = dvt.DVT_MA
                OUTER APPLY (
                    SELECT TOP 1 DG_DONGIA AS LAST_PRICE
                    FROM DONGIA 
                    WHERE T_MA = lt.T_MA 
                    ORDER BY NGAY DESC, THOIGIAN DESC
                ) AS dg
                ORDER BY lt.T_MA, lt.LT_MA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTim.Text.Trim();
            string sql = $@"
                SELECT 
                    lt.LT_MA AS [Mã lô],
                    lt.T_MA AS [Mã thuốc],
                    t.T_TEN AS [Tên thuốc],
                    lt.LT_NGAYSX AS [Ngày sản xuất],
                    lt.LT_NGAYHD AS [Hạn dùng],
                    lt.LT_SLTON AS [Số lượng tồn],
                    dvt.DVT_TEN AS [Đơn vị tính],
                    lt.LT_DGNHAP AS [Đơn giá nhập],
                    dg.LAST_PRICE AS [Đơn giá bán]
                FROM LOTHUOC lt 
                INNER JOIN THUOC t ON lt.T_MA = t.T_MA
                INNER JOIN DONVITINH dvt ON t.DVT_MA = dvt.DVT_MA
                OUTER APPLY (
                    SELECT TOP 1 DG_DONGIA AS LAST_PRICE
                    FROM DONGIA 
                    WHERE T_MA = lt.T_MA 
                    ORDER BY NGAY DESC, THOIGIAN DESC
                ) AS dg
                WHERE lt.LT_MA LIKE '%{keyword}%' 
                   OR lt.T_MA LIKE '%{keyword}%'
                   OR t.T_TEN LIKE N'%{keyword}%'
                   OR dvt.DVT_TEN LIKE N'%{keyword}%'
                ORDER BY lt.T_MA, lt.LT_MA";
            cn.load_dataGridView(conn, sql, dataGridView1);
        }

        private void btIn_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"BaoCaoLoThuocKho_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Tính tổng số lượng tồn
                    int tongSoLuongTon = 0;
                    int tongSoLo = 0;
                    decimal tongGiaTri = 0;
                    decimal tongGiaTriBan = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["Số lượng tồn"].Value != null && row.Cells["Đơn giá nhập"].Value != null)
                        {
                            int soLuong = Convert.ToInt32(row.Cells["Số lượng tồn"].Value);
                            decimal donGia = Convert.ToDecimal(row.Cells["Đơn giá nhập"].Value);
                            decimal donGiaBan = row.Cells["Đơn giá bán"].Value != null ?
                                Convert.ToDecimal(row.Cells["Đơn giá bán"].Value) : 0;

                            tongSoLuongTon += soLuong;
                            tongSoLo++;
                            tongGiaTri += soLuong * donGia;
                            tongGiaTriBan += soLuong * donGiaBan;
                        }
                    }

                    // Tạo PDF
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 20, 20, 30, 30);
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    doc.Open();

                    // Font Unicode (tiếng Việt)
                    string fontPath = @"C:\Windows\Fonts\arial.ttf";
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                    iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                    // Tiêu đề
                    iTextSharp.text.Paragraph paraTitle = new iTextSharp.text.Paragraph("BÁO CÁO TỒN KHO TỔNG", fontTitle);
                    paraTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    paraTitle.SpacingAfter = 20f;
                    doc.Add(paraTitle);

                    // Thông tin báo cáo
                    doc.Add(new iTextSharp.text.Paragraph($"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Thống kê
                    doc.Add(new iTextSharp.text.Paragraph("THỐNG KÊ TỔNG QUAN", fontBold));

                    string thongKeText = $"Tổng số lô thuốc: {tongSoLo} lô\n" +
                                         $"Tổng số lượng tồn: {tongSoLuongTon.ToString("#,##0")} sản phẩm\n" +
                                         $"Tổng giá trị tồn kho (theo giá nhập): {tongGiaTri.ToString("#,##0")} VNĐ\n" +
                                         $"Tổng giá trị tồn kho (theo giá bán): {tongGiaTriBan.ToString("#,##0")} VNĐ";

                    doc.Add(new iTextSharp.text.Paragraph(thongKeText, fontNormal));
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Bảng dữ liệu
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(9);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1, 1, 2, 1, 1, 1, 1, 1, 1 });

                    // Header bảng
                    table.AddCell(new iTextSharp.text.Phrase("Mã lô", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Mã thuốc", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Tên thuốc", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Ngày SX", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Hạn dùng", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Số lượng tồn", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Đơn vị tính", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Đơn giá nhập", fontBold));
                    table.AddCell(new iTextSharp.text.Phrase("Đơn giá bán", fontBold));

                    // Dữ liệu
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["Mã lô"].Value != null)
                        {
                            try
                            {
                                // Lấy giá trị từ các cột
                                string maLo = row.Cells["Mã lô"].Value?.ToString()?.Trim() ?? "";
                                string maThuoc = row.Cells["Mã thuốc"].Value?.ToString()?.Trim() ?? "";
                                string tenThuoc = row.Cells["Tên thuốc"].Value?.ToString()?.Trim() ?? "";

                                // Định dạng ngày
                                string ngaySX = "";
                                if (row.Cells["Ngày sản xuất"].Value != null)
                                {
                                    DateTime ngay = Convert.ToDateTime(row.Cells["Ngày sản xuất"].Value);
                                    ngaySX = ngay.ToString("dd/MM/yyyy");
                                }

                                string hanDung = "";
                                if (row.Cells["Hạn dùng"].Value != null)
                                {
                                    DateTime ngayHD = Convert.ToDateTime(row.Cells["Hạn dùng"].Value);
                                    hanDung = ngayHD.ToString("dd/MM/yyyy");
                                }

                                string soLuong = row.Cells["Số lượng tồn"].Value?.ToString()?.Trim() ?? "0";
                                string donVi = row.Cells["Đơn vị tính"].Value?.ToString()?.Trim() ?? "";

                                string donGiaNhap = "0";
                                if (row.Cells["Đơn giá nhập"].Value != null)
                                {
                                    decimal giaNhap = Convert.ToDecimal(row.Cells["Đơn giá nhập"].Value);
                                    donGiaNhap = giaNhap.ToString("#,##0");
                                }

                                string donGia = "0";
                                if (row.Cells["Đơn giá bán"].Value != null)
                                {
                                    decimal gia = Convert.ToDecimal(row.Cells["Đơn giá bán"].Value);
                                    donGia = gia.ToString("#,##0");
                                }

                                // Thêm các ô vào bảng
                                table.AddCell(new iTextSharp.text.Phrase(maLo, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(maThuoc, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(tenThuoc, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(ngaySX, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(hanDung, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(soLuong, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(donVi, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(donGiaNhap, fontNormal));
                                table.AddCell(new iTextSharp.text.Phrase(donGia, fontNormal));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi xử lý dòng: {ex.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }
                        }
                    }

                    doc.Add(table);
                    doc.Add(iTextSharp.text.Chunk.NEWLINE);

                    // Kết thúc báo cáo
                    iTextSharp.text.Paragraph paraKetThuc = new iTextSharp.text.Paragraph("--- HẾT BÁO CÁO ---", fontSmall);
                    paraKetThuc.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    paraKetThuc.SpacingBefore = 20f;
                    doc.Add(paraKetThuc);

                    doc.Close();
                    writer.Close();

                    MessageBox.Show($"Đã xuất báo cáo thành công!\nFile: {Path.GetFileName(saveFileDialog.FileName)}",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file PDF sau khi xuất
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
