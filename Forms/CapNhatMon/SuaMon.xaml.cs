using Microsoft.Win32;
using QLTS.Models;
using QLTS.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLTS.Forms.CapNhatMon
{
    /// <summary>
    /// Interaction logic for SuaMon.xaml
    /// </summary>
    public partial class SuaMon : Window
    {
        private byte[] imageBytes = null;
        private string idsp;
        Modify modify = new Modify();
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public SuaMon(string idsp)
        {
            InitializeComponent();
            this.idsp = idsp;
            cmbLoai.Items.Add("Trà Sữa");
            cmbLoai.Items.Add("Ăn Vặt");
            cmbTrangThai.Items.Add("Còn Bán");
            cmbTrangThai.Items.Add("Hết Hàng");
            ThongTinMonAn();
        }
        private void ThongTinMonAn()
        {
            string query = "select * from SanPham where maSP = '" + idsp + "'";
            List<mon> list = modify.mons(query);
            txtMaSP.Text = list[0].MaSP;
            txtTenSP.Text = list[0].TenSP;
            txtGia.Text = list[0].Gia.ToString();
            cmbLoai.SelectedItem = list[0].Loai;
            cmbTrangThai.SelectedItem = list[0].TinhTrang;
            if (list[0].Anh != null)
            {
                imageBytes = BitmapImageToByteArray(list[0].Anh);
                UploadedImage.Source = LoadImageFromBytes(imageBytes);
            }
        }
        public byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder(); // Hoặc PngBitmapEncoder nếu muốn PNG
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                data = stream.ToArray();
            }

            return data;
        }
        private BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                return bitmap;
            }
        }
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png)|*.jpg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                // Display image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                UploadedImage.Source = bitmap;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();
            string loai = cmbLoai.SelectedItem?.ToString();
            decimal gia;

            if (!decimal.TryParse(txtGia.Text.Trim(), out gia))
            {
                MessageBox.Show("Giá không hợp lệ!");
                return;
            }

            if (string.IsNullOrWhiteSpace(maSP) || string.IsNullOrWhiteSpace(tenSP) || string.IsNullOrWhiteSpace(loai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // **Insert new product if maSP is unique**
                string query = @"UPDATE SanPham 
                 SET tenSP = @tenSP, 
                     gia = @gia, 
                     loai = @loai, 
                     anh = @Anh, 
                     tinhTrang = @tinhTrang 
                 WHERE maSP = @maSP";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maSP", maSP);
                    cmd.Parameters.AddWithValue("@tenSP", tenSP);
                    cmd.Parameters.AddWithValue("@gia", gia);
                    cmd.Parameters.AddWithValue("@loai", loai);
                    cmd.Parameters.Add("@Anh", System.Data.SqlDbType.VarBinary, imageBytes?.Length ?? 0).Value = (object)imageBytes ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@tinhTrang", cmbTrangThai.Text);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Lưu sản phẩm thành công!");
            Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
