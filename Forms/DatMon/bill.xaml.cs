using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using QLTS.Models;
using System.Collections.ObjectModel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iText.Layout.Element;
using Microsoft.Win32;
using iText.Kernel.Font;
using iText.IO.Image;

namespace QLTS.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for bill.xaml
    /// </summary>
    public partial class bill : Window
    {
        private DatMon datMonInstance;
        private ObservableCollection<mon> DanhSachBill;
        private string ghiChu;

        public bill(DatMon datMonInstance, string maHoaDon, string ngayLap, string tenKhach, string sdt, string soBan, decimal giamGia, string ghiChu, ObservableCollection<mon> danhSachMon)
        {
            InitializeComponent();
            this.DataContext = this; // Gán binding cho ImagePath
            this.datMonInstance = datMonInstance;
            this.DanhSachBill = danhSachMon;
            this.ghiChu = ghiChu;

            // Gán thông tin hóa đơn
            tbl_mahoadon.Text = maHoaDon;
            tbl_ngaylap.Text = ngayLap;
            tbl_tenkh.Text = tenKhach;
            tbl_sdt.Text = sdt;
            tbl_ban.Text = soBan;

            // Thêm ghi chú nếu có
            if (!string.IsNullOrWhiteSpace(ghiChu))
            {
                tbl_ghichu.Text = ghiChu;
                lbl_ghichu.Visibility = Visibility.Visible;
                tbl_ghichu.Visibility = Visibility.Visible;
            }
            else
            {
                lbl_ghichu.Visibility = Visibility.Collapsed;
                tbl_ghichu.Visibility = Visibility.Collapsed;
            }

            // Hiển thị danh sách món ăn
            dataGridBill.ItemsSource = DanhSachBill;

            // Tính tổng tiền
            decimal tongTienGoc = DanhSachBill.Sum(mon => mon.ThanhTien);
            decimal tongTienSauGiamGia = tongTienGoc - giamGia;

            // Hiển thị tổng tiền
            lblTongTienGoc.Text = $"Tổng tiền: {tongTienGoc:N0} VNĐ";
            lblGiamGia.Text = $"Giảm giá: {giamGia:N0} VNĐ";
            lblTongTienSauGiamGia.Text = $"Thành tiền: {tongTienSauGiamGia:N0} VNĐ";
        }


        public string ImagePath
        {
            get
            {
                string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                string imagePath = Path.Combine(imageFolder, "anhhoadon.png");
                return File.Exists(imagePath) ? imagePath : "";
            }
        }

        private void XuatHoaDonPDF()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "HoaDon.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                if (File.Exists(filePath))
                {
                    filePath = Path.Combine(Path.GetDirectoryName(filePath), $"HoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                }

                // Tạo font tiếng Việt
                string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts", "arial.ttf");
                if (!File.Exists(fontPath))
                {
                    MessageBox.Show("Font Arial không tìm thấy, vui lòng kiểm tra lại!", "Lỗi Font", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                PdfFont boldFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

                using (PdfWriter writer = new PdfWriter(filePath))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf))
                {
                    // Thiết lập font mặc định
                    document.SetFont(font);
                    document.SetMargins(50, 50, 50, 50);

                    // Logo và địa chỉ
                    ImageData imageData = ImageDataFactory.Create(ImagePath);
                    Image logo = new Image(imageData).SetWidth(200).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                    document.Add(logo);

                    document.Add(new Paragraph("Địa chỉ: 130 Điện Biên Phủ, Thanh Khê, Đà Nẵng")
                        .SetFontSize(11)
                        .SetFont(boldFont)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    // Tiêu đề
                    document.Add(new Paragraph("HÓA ĐƠN THANH TOÁN")
                        .SetFontSize(20)
                        .SetFont(boldFont)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    // Thông tin hóa đơn
                    Table infoTable = new Table(2).UseAllAvailableWidth();
                    infoTable.AddCell(new Cell().Add(new Paragraph("Mã Hóa Đơn:").SetFont(boldFont)));
                    infoTable.AddCell(new Cell().Add(new Paragraph(tbl_mahoadon.Text)));
                    infoTable.AddCell(new Cell().Add(new Paragraph("Ngày Lập:").SetFont(boldFont)));
                    infoTable.AddCell(new Cell().Add(new Paragraph(tbl_ngaylap.Text)));
                    infoTable.AddCell(new Cell().Add(new Paragraph("Tên Khách Hàng:").SetFont(boldFont)));
                    infoTable.AddCell(new Cell().Add(new Paragraph(tbl_tenkh.Text)));
                    infoTable.AddCell(new Cell().Add(new Paragraph("Số điện thoại:").SetFont(boldFont)));
                    infoTable.AddCell(new Cell().Add(new Paragraph(tbl_sdt.Text)));
                    infoTable.AddCell(new Cell().Add(new Paragraph("Bàn:").SetFont(boldFont)));
                    infoTable.AddCell(new Cell().Add(new Paragraph(tbl_ban.Text)));

                    // Thêm ghi chú nếu có
                    if (!string.IsNullOrWhiteSpace(ghiChu))
                    {
                        infoTable.AddCell(new Cell().Add(new Paragraph("Ghi chú:").SetFont(boldFont)));
                        infoTable.AddCell(new Cell().Add(new Paragraph(ghiChu)));
                    }

                    document.Add(infoTable);

                    // Bảng danh sách món
                    Table menuTable = new Table(4).UseAllAvailableWidth();
                    menuTable.AddHeaderCell(new Cell().Add(new Paragraph("Tên món").SetFont(boldFont)));
                    menuTable.AddHeaderCell(new Cell().Add(new Paragraph("SL").SetFont(boldFont)));
                    menuTable.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá").SetFont(boldFont)));
                    menuTable.AddHeaderCell(new Cell().Add(new Paragraph("Thành tiền").SetFont(boldFont)));

                    foreach (var item in DanhSachBill)
                    {
                        menuTable.AddCell(new Cell().Add(new Paragraph(item.TenSP)));
                        menuTable.AddCell(new Cell().Add(new Paragraph(item.SoLuong.ToString())));
                        menuTable.AddCell(new Cell().Add(new Paragraph(item.Gia.ToString("N0") + " VNĐ")));
                        menuTable.AddCell(new Cell().Add(new Paragraph(item.ThanhTien.ToString("N0") + " VNĐ")));
                    }
                    document.Add(menuTable);

                    // Tổng tiền
                    Table totalTable = new Table(2).UseAllAvailableWidth();
                    totalTable.AddCell(new Cell().Add(new Paragraph(lblTongTienGoc.Text)));
                    totalTable.AddCell(new Cell());
                    totalTable.AddCell(new Cell().Add(new Paragraph(lblGiamGia.Text).SetFontColor(ColorConstants.RED)));
                    totalTable.AddCell(new Cell());
                    totalTable.AddCell(new Cell().Add(new Paragraph(lblTongTienSauGiamGia.Text).SetFontSize(16).SetFont(boldFont)));
                    document.Add(totalTable);

                    MessageBox.Show($"Hóa đơn đã lưu tại: {filePath}", "Xuất PDF thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            // Gọi phương thức reset bàn từ DatMon.xaml.cs
            if (datMonInstance != null)
            {
                datMonInstance.ResetBan();
            }
            this.Close();
        }

        private void BtnInHoaDon_Click(object sender, RoutedEventArgs e)
        {
            XuatHoaDonPDF();
        }
    }
}