using QLTS.Sql;
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
using System.Windows.Navigation;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
namespace QLTS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string LogoPath
        {
            get
            {
                string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                string imagePath = Path.Combine(imageFolder, "trangchu.png");
                return File.Exists(imagePath) ? imagePath : "";
            }
        }
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = text_MatKhau.Password;
            PasswordTextBox.Visibility = Visibility.Visible;
            text_MatKhau.Visibility = Visibility.Collapsed;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            text_MatKhau.Password = PasswordTextBox.Text;
            PasswordTextBox.Visibility = Visibility.Collapsed;
            text_MatKhau.Visibility = Visibility.Visible;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (text_MatKhau.Visibility == Visibility.Visible)
            {
                PasswordTextBox.Text = text_MatKhau.Password;
            }
        }
        private string GetVaiTroUser(string tenDangNhap)
        {
            string vaiTro = "Nhân viên"; // Mặc định là nhân viên nếu không tìm thấy trong DB
            string query = "SELECT loai FROM TaiKhoan WHERE tenTk = @TenDangNhap";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        vaiTro = result.ToString().Trim();
                    }
                }
            }
            return vaiTro;
        }

        Modify modify = new Modify();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tenDangNhap = Text_TenDangNhap.Text;
            string matKhau = text_MatKhau.Visibility == Visibility.Visible ? text_MatKhau.Password : PasswordTextBox.Text;
            string vaiTro = GetVaiTroUser(tenDangNhap);
            QLTS.Forms.Trangchu.Trangchu trangChu = new QLTS.Forms.Trangchu.Trangchu(vaiTro);
            if (tenDangNhap.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Text_TenDangNhap.Focus();
            }
            else if (matKhau.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                text_MatKhau.Focus();
            }
            else
            {
                string query = "SELECT * FROM TaiKhoan WHERE tenTK = '" + tenDangNhap + "' AND matkhau = '" + matKhau + "'";
                if (modify.TaiKhoans(query).Count > 0)
                {
                    this.Close();
                    trangChu.Show();
                }
                 else
                    {
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    Text_TenDangNhap.Clear();
                    text_MatKhau.Clear();
                    Text_TenDangNhap.Focus();
                }
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
