using QLTS.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using System.IO;

namespace QLTS.Forms.CapNhatMon
{
    /// <summary>
    /// Interaction logic for ThemMon.xaml
    /// </summary>
    public partial class ThemMon : Window
    {
        private byte[] imageBytes = null; // Lưu ảnh được chọn
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public ThemMon()
        {
            InitializeComponent();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();

                // Xử lý khi chọn mục
                switch (selectedText)
                {
                    case "Ăn Vặt":
                        break;

                    case "Trà Sữa":
                        break;

                    default:

                        break;
                }
            }
        }
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                // Hiển thị ảnh
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
            string loai = ((ComboBoxItem)cmbLoai.SelectedItem)?.Content.ToString();
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

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                string query = @"INSERT INTO SanPham (maSP, tenSP, gia, loai, anh ,tinhTrang) 
                         VALUES (@maSP, @tenSP, @gia, @loai, @Anh ,@tinhTrang )";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@maSP", maSP);
                    cmd.Parameters.AddWithValue("@tenSP", tenSP);
                    cmd.Parameters.AddWithValue("@gia", gia);
                    cmd.Parameters.AddWithValue("@loai", loai);
                    cmd.Parameters.AddWithValue("@Anh", (object)imageBytes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@tinhTrang", "Còn Bán"); // Mặc định

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
