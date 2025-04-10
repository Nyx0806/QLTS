using QLTS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace QLTS.Forms.NhanVien
{
    /// <summary>
    /// Interaction logic for ThemNV.xaml
    /// </summary>
    public partial class ThemNV : Window
    {
        public ObservableCollection<Employee> Employees { get; set; }
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public ThemNV(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            this.Employees = employees;
            this.DataContext = this;
        }

        private void BtnThemNhanVien_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtma.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtluong.Text) ||
                cmbChucVu.SelectedItem == null || cbGioitinh.SelectedItem == null || dpNgaySinh.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtluong.Text, out decimal luong))
            {
                MessageBox.Show("Lương không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtSDT.Text, out int sdt))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string query = "INSERT INTO NhanVien (maNV, hoten, chucvu, luong, Email, sdt, TrangThai, ngaysinh, gioitinh) " +
                           "VALUES (@maNV, @hoten, @chucvu, @luong, @Email, @sdt, @TrangThai, @ngaysinh, @gioitinh)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maNV", txtma.Text);
                        cmd.Parameters.AddWithValue("@hoten", txtHoTen.Text);
                        cmd.Parameters.AddWithValue("@chucvu", cmbChucVu.Text);
                        cmd.Parameters.AddWithValue("@luong", luong);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@sdt", sdt);
                        cmd.Parameters.AddWithValue("@TrangThai", "Đang Làm"); // Mặc định
                        cmd.Parameters.AddWithValue("@ngaysinh", dpNgaySinh.SelectedDate);
                        cmd.Parameters.AddWithValue("@gioitinh", cbGioitinh.Text == "Nam" ? 1 : 0);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm nhân viên thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Thêm nhân viên thất bại!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
