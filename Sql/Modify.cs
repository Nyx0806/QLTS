using QLTS.Models;
using QLTS.Models.TongDoanhThu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QLTS.Sql
{
    class Modify
    {
        public Modify()
        {
        }
        SqlCommand sqlCommand; //dùng để truy vấn các cau lệnh sql
        SqlDataReader dataReader; // đọc dữ liệu từ sql
        public List<TaiKhoan> TaiKhoans(string query)
        {
            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    taiKhoans.Add(new TaiKhoan(dataReader.GetString(0), dataReader.GetString(1)));
                }
                sqlConnection.Close();
            }
            return taiKhoans;
        }
        public List<HoaDon> HoaDons(string query)
        {
            List<HoaDon> hdon = new List<HoaDon>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    hdon.Add(new HoaDon(dataReader.GetString(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetDateTime(3), dataReader.GetDecimal(4)));
                }
                sqlConnection.Close();
            }
            return hdon;
        }
        public doanhthumax DoanhThuThangMax(string query) //nayf maxx
        {
            doanhthumax list = new doanhthumax();

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    list = new doanhthumax(dataReader.GetInt32(0), dataReader.GetDecimal(1));
                }
                sqlConnection.Close();
            }

            return list;
        }
        public doanhthulowest DoanhThuThangLowest(string query) //nayf low
        {
            doanhthulowest list = new doanhthulowest();

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    list = new doanhthulowest(dataReader.GetInt32(0), dataReader.GetDecimal(1));
                }
                sqlConnection.Close();
            }

            return list;
        }
        public void ThucThi(string query)
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();

                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteNonQuery(); // thực thi câu truy vấn

                sqlConnection.Close();
            }
        }
        public List<luunhanvien> luunhanviens(string query)
        {
            List<luunhanvien> luunhanviens = new List<luunhanvien>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    luunhanviens.Add(new luunhanvien(dataReader.GetString(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetDecimal(3), dataReader.GetString(4), dataReader.GetString(5), dataReader.GetString(6), dataReader.GetDateTime(7), dataReader.GetString(8)));
                }
                sqlConnection.Close();
            }
            return luunhanviens;
        }
        public static BitmapImage ByteArrayToBitmapImage(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze(); // Tăng hiệu suất và tránh lỗi threading
                return image;
            }
        }
        public List<mon> mons(string query)
        {
            List<mon> mons = new List<mon>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        string maMon = dataReader.GetString(0);
                        string tenMon = dataReader.GetString(1);
                        decimal gia = dataReader.GetDecimal(2);
                        string loai = dataReader.GetString(3);
                        int soLuong = 1;
                        // Đọc byte[] từ cột 4
                        byte[] imageData = (byte[])dataReader[5];
                        BitmapImage image = ByteArrayToBitmapImage(imageData);
                        string ghiChu = dataReader.GetString(4);
                        mons.Add(new mon(maMon, tenMon, gia, loai , soLuong , image, ghiChu));
                    }
                }
                sqlConnection.Close();
            }
            return mons;
        }
        public mon LayThongTinMon(string maSanPham)
        {
            try
            {
                string query = "SELECT * FROM SanPham WHERE maSP = @maSP";
                using (SqlConnection conn = Connection.GetSqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maSP", maSanPham);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new mon(
                                    reader.GetString(0), // MaSP
                                    reader.GetString(1), // TenSP
                                    reader.GetDecimal(2), // Gia
                                    reader.GetString(3), // Loai
                                    1, // SoLuong mặc định là 1
                                    Modify.ByteArrayToBitmapImage((byte[])reader[5]), // Ảnh
                                    reader.GetString(4) // Ghi chú
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin món: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null; // Trả về null nếu không tìm thấy món
        }
        public string TaoMaHoaDon()
        {
            string ngay = DateTime.Now.ToString("yyyyMMdd");
            string query = "SELECT COUNT(*) FROM HoaDon WHERE maHD LIKE @Pattern";

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@Pattern", $"HD{ngay}%");
                int count = (int)cmd.ExecuteScalar();
                return $"HD{ngay}{(count + 1):D4}"; // Tạo mã dạng HD202403190001
            }
        }
        public string TaoMaKHNgauNhien()
        {
            string maKH;
            Random rand = new Random();
            bool maHopLe = false;

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                do
                {
                    string ngay = DateTime.Now.ToString("yyyyMMdd"); // Định dạng ngày
                    int soNgauNhien = rand.Next(100, 999); // Tạo số ngẫu nhiên từ 100-999
                    maKH = $"KH{ngay}{soNgauNhien}";

                    string queryCheck = "SELECT COUNT(*) FROM KhachHang WHERE maKH = @maKH";
                    using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
                    {
                        cmd.Parameters.AddWithValue("@maKH", maKH);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        maHopLe = count == 0; // Nếu chưa tồn tại, mã hợp lệ
                    }
                } while (!maHopLe);
            }
            return maKH;
        }
        public bool ThemHoaDon(string maHD, string maKH, string soBan, decimal tongTien)
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                string query = "INSERT INTO HoaDon (maHD, maKH, banSo, tongTien, ngayLap) VALUES (@maHD, @maKH, @soBan, @tongTien, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@maHD", maHD);
                    cmd.Parameters.AddWithValue("@maKH", maKH);
                    cmd.Parameters.AddWithValue("@soBan", soBan ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@tongTien", tongTien);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm hóa đơn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
        }
        public bool ThemChiTietHoaDon(string maHD, List<mon> danhSachMon)
        {
            if (danhSachMon == null || danhSachMon.Count == 0)
            {
                MessageBox.Show("Danh sách món trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();

                try
                {
                    int stt = 1;
                    foreach (var mon in danhSachMon)
                    {
                        string maHDChiTiet = $"{maHD}-{stt:D3}";

                        string query = "INSERT INTO ChiTietHoaDon (maHDChiTiet, maHD, maSP, soLuong, donGia, ngayGhi) " +
                                       "VALUES (@MaHDChiTiet, @MaHD, @MaSP, @SL, @DonGia, GETDATE())";

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaHDChiTiet", maHDChiTiet);
                            cmd.Parameters.AddWithValue("@MaHD", maHD);
                            cmd.Parameters.AddWithValue("@MaSP", mon.MaSP);
                            cmd.Parameters.AddWithValue("@SL", mon.SoLuong);
                            cmd.Parameters.AddWithValue("@DonGia", mon.ThanhTien);
                            cmd.ExecuteNonQuery();
                        }
                        stt++;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        public int LayDiemTichLuy(string sdtKhach)
        {
            int diemTichLuy = 0;
            string query = "SELECT ISNULL(tichDiem, 0) FROM KhachHang WHERE sdt = @SDT";

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@SDT", sdtKhach);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        diemTichLuy = Convert.ToInt32(result);
                    }
                }
            }
            return diemTichLuy;
        }
        public bool CapNhatDiemTichLuy(string sdtKhach, int diemMuonDung, int diemCongThem)
        {
            if (string.IsNullOrEmpty(sdtKhach))
            {
                MessageBox.Show("Số điện thoại khách hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra số điểm hiện có
                        string checkQuery = "SELECT ISNULL(tichDiem, 0) FROM KhachHang WHERE sdt = @SDT";
                        int diemHienTai;
                        
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@SDT", sdtKhach);
                            object result = checkCmd.ExecuteScalar();
                            
                            if (result == null || result == DBNull.Value)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Không tìm thấy thông tin khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                            
                            diemHienTai = Convert.ToInt32(result);
                        }

                        // Kiểm tra điểm sử dụng
                        if (diemMuonDung > diemHienTai)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        // Tính toán điểm mới
                        int diemMoi = Math.Max(0, diemHienTai - diemMuonDung + diemCongThem);

                        // Cập nhật điểm
                        string updateQuery = "UPDATE KhachHang SET tichDiem = @DiemMoi WHERE sdt = @SDT";
                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, sqlConnection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@DiemMoi", diemMoi);
                            updateCmd.Parameters.AddWithValue("@SDT", sdtKhach);
                            
                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi khi cập nhật điểm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
        }
        public string LayMaKH(string tenKhach, string sdtKhach)
        {
            if (string.IsNullOrEmpty(sdtKhach)) return null;

            string queryCheck = "SELECT maKH FROM KhachHang WHERE sdt = @sdt";
            string maKH = null;

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
                {
                    cmd.Parameters.AddWithValue("@sdt", sdtKhach);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        maKH = result.ToString();
                    }
                    else
                    {
                        maKH = TaoMaKHNgauNhien();
                        string queryInsert = "INSERT INTO KhachHang (maKH, tenKH, sdt, tichDiem) VALUES (@maKH, @tenKH, @sdt, 0)";
                        using (SqlCommand insertCmd = new SqlCommand(queryInsert, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@maKH", maKH);
                            insertCmd.Parameters.AddWithValue("@tenKH", tenKhach);
                            insertCmd.Parameters.AddWithValue("@sdt", sdtKhach);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            return maKH;
        }
    }
}
