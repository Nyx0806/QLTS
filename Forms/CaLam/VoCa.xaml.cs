using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using QLTS.Sql;
using System.IO;

namespace QLTS.Forms.CaLam
{
    public partial class VoCa : UserControl
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;
        private string VaiTro { get; set; } // Thêm thuộc tính vai trò
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
        public VoCa(string vaiTro)
        {
            InitializeComponent();
            this.DataContext = this;
            VaiTro = vaiTro;
            PhanQuyenTaiKhoan(); // Gọi hàm ẩn nút lương nếu cần
        }
        public string LogoPath
        {
            get
            {
                string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                string imagePath = Path.Combine(imageFolder, "calam.png");
                return File.Exists(imagePath) ? imagePath : "";
            }
        }
        private void PhanQuyenTaiKhoan()
        {
            if (VaiTro == "Nhân viên")
            {
                btnLuong.Visibility = Visibility.Collapsed; // Ẩn nút Lương
            }
        }

        private void BtnVoCa_Click(object sender, RoutedEventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string maQL = txtMaQL.Text.Trim();

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(maQL))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã nhân viên và mã quản lý.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra nhân viên
                SqlCommand cmdNV = new SqlCommand("SELECT hoten FROM NhanVien WHERE maNV = @maNV", conn);
                cmdNV.Parameters.AddWithValue("@maNV", maNV);
                var hoten = cmdNV.ExecuteScalar();

                if (hoten == null)
                {
                    MessageBox.Show("Mã nhân viên không tồn tại.");
                    return;
                }

                // Kiểm tra mã quản lý
                SqlCommand cmdQL = new SqlCommand("SELECT chucvu FROM NhanVien WHERE maNV = @maQL AND chucvu = N'Quản Lí'", conn);
                cmdQL.Parameters.AddWithValue("@maQL", maQL);
                var chucvu = cmdQL.ExecuteScalar();

                if (chucvu == null)
                {
                    MessageBox.Show("Mã quản lý không hợp lệ.");
                    return;
                }

                // Kiểm tra nhân viên đã vô ca chưa
                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM ChamCong WHERE maNV = @maNV AND thoiGianRa IS NULL", conn);
                cmdCheck.Parameters.AddWithValue("@maNV", maNV);
                int count = (int)cmdCheck.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Nhân viên này đã vô ca và chưa kết thúc ca.");
                    return;
                }

                // Lưu giờ vô ca
                DateTime gioVao = DateTime.Now;
                SqlCommand cmdInsert = new SqlCommand("INSERT INTO ChamCong (maNV, thoiGianVao) VALUES (@maNV, @gioVao)", conn);
                cmdInsert.Parameters.AddWithValue("@maNV", maNV);
                cmdInsert.Parameters.AddWithValue("@gioVao", gioVao);
                cmdInsert.ExecuteNonQuery();

                MessageBox.Show("Vô ca thành công!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mo(voca, activeform, new HoatDong());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Mo(voca, activeform, new Luong());
        }
    }
}
