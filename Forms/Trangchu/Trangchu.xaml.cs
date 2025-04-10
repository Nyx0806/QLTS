using QLTS.Models.TongDoanhThu;
using QLTS.Properties;
using System;
using System.Collections.Generic;
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
using System.IO;
namespace QLTS.Forms.Trangchu
{
    /// <summary>
    /// Interaction logic for Trangchu.xaml
    /// </summary>
    public partial class Trangchu : Window
    {
        public string ImagePath { get; set; } // Thuộc tính lưu đường dẫn ảnh
        public string VaiTro { get; set; } // "Admin" hoặc "NhanVien"
        public Trangchu(string vaiTro)
        {
            InitializeComponent();
            ImagePath = GetImagePath("user_icon.png"); // Gán ảnh từ thư mục Images
            DataContext = this; // Kết nối XAML với thuộc tính ImagePath
            VaiTro = vaiTro;
            PhanQuyenTaiKhoan();
        }
        private void Mo(Grid panel1, UserControl activeform, UserControl childform)
        {
            if (activeform != null)
            {
                panel1.Children.Remove(activeform); // Xóa giao diện cũ
            }
            activeform = childform; // Gán giao diện mới
            panel1.Children.Add(childform); // Thêm vào Grid
        }
        UserControl activeform = null;
        private string GetImagePath(string fileName)
        {
            string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            string imagePath = Path.Combine(imageFolder, fileName);

            if (!File.Exists(imagePath))
            {
                string defaultImagePath = Path.Combine(imageFolder, "default.jpg");
                return File.Exists(defaultImagePath) ? defaultImagePath : "";
            }
            return imagePath;
        }
        private void PhanQuyenTaiKhoan()
        {
            if (VaiTro == "Nhân viên")
            {
                btnDoanhThu.Visibility = Visibility.Collapsed;
                btnNhanVien.Visibility = Visibility.Collapsed;
                btnCapNhatMon.Visibility = Visibility.Collapsed;
            }
        }
        private void ResetButtonColors()
        {
            Color defaultColor = Color.FromRgb(242, 193, 147); // Màu ban đầu #F2C193
            btnDatMon.Background = new SolidColorBrush(defaultColor);
            btnDoanhThu.Background = new SolidColorBrush(defaultColor);
            btnNhanVien.Background = new SolidColorBrush(defaultColor);
            btnCapNhatMon.Background = new SolidColorBrush(defaultColor);
            btnVoCa.Background = new SolidColorBrush(defaultColor);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Button_Click_DatMon(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnDatMon.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new QLTS.Forms.DatMon.DatMon());
        }

        private void Button_Click_NhanVien(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnNhanVien.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new QLTS.Forms.NhanVien.NhanVien());
        }

        private void Button_Click_DoanhThu(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnDoanhThu.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new QLTS.Forms.DoanhThu.DoanhThu());
        }
        private void Button_Seting(object sender, RoutedEventArgs e)
        {
            //Seting seting = new Seting();
            //seting.Show();
        }

        private void Button_Click_CapNhatMon(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnCapNhatMon.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89)); // Đổi màu khi bấm
            Mo(chinhchu, activeform, new QLTS.Forms.CapNhatMon.CapNhatMon());
        }

        private void Button_Click_CaLam(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            btnVoCa.Background = new SolidColorBrush(Color.FromRgb(206, 152, 89));
            Mo(chinhchu, activeform, new QLTS.Forms.CaLam.VoCa(VaiTro));
        }
    }
}
