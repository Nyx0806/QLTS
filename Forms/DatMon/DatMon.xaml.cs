using QLTS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using QLTS.Sql;

namespace QLTS.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for DatMon.xaml
    /// </summary>
    public partial class DatMon : UserControl
    {
        private readonly Modify modify = new Modify();
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;
        Dictionary<string, (string TenKhach, string SDT)> thongTinKhachHangBan = new Dictionary<string, (string, string)>();
        private string soBanHienTai = "";
        public ObservableCollection<mon> DanhSachMon { get; set; } = new ObservableCollection<mon>();
        Dictionary<string, ObservableCollection<mon>> banHoaDon = new Dictionary<string, ObservableCollection<mon>>();
        Dictionary<string, bool> trangThaiBan = new Dictionary<string, bool>();
        
        public Button banDangChon = null;
        private bool laDonMangVe = false;
        UserControl activeform = null;

        public DatMon()
        {
            InitializeComponent();
            dataGridMon.ItemsSource = DanhSachMon;
            KhoiTaoBanAn();
        }

        private void KhoiTaoBanAn()
        {
            string query = "SELECT banSo, trangthai, loai FROM Ban";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string soBan = reader["banSo"]?.ToString().Trim() ?? "";
                            string trangThai = reader["trangthai"]?.ToString().Trim() ?? "";
                            string loai = reader["loai"]?.ToString().Trim() ?? "";

                            if (string.IsNullOrEmpty(soBan)) continue;

                            Button btnBan = new Button
                            {
                                Content = $"{soBan}\n{loai}",
                                Background = trangThai == "Đang sử dụng" ? Brushes.LightGreen :
                                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A")),
                                FontSize = 16,
                                FontWeight = FontWeights.Bold,
                                Width = 100,
                                Height = 80,
                                Margin = new Thickness(5)
                            };

                            btnBan.Click += (s, e) => ChonBan(btnBan, soBan);
                            banAnContainer.Children.Add(btnBan);
                            if (!banHoaDon.ContainsKey(soBan))
                            {
                                banHoaDon[soBan] = new ObservableCollection<mon>();
                            }

                            if (!trangThaiBan.ContainsKey(soBan))
                            {
                                trangThaiBan[soBan] = trangThai == "Đang sử dụng";
                            }
                        }
                    }
                }
            }
        }
        private void CapNhatThongTinKhachHang()
        {
            if (!string.IsNullOrEmpty(soBanHienTai))
            {
                // Luôn cập nhật thông tin cho bàn hiện tại
                thongTinKhachHangBan[soBanHienTai] = (txtTenKhach.Text.Trim(), txtSDT.Text.Trim());
            }
        }
        private void ChonBan(Button btnBan, string soBan)
        {
            // Lưu thông tin khách hàng của bàn hiện tại trước khi chuyển
            if (!string.IsNullOrEmpty(soBanHienTai) && thongTinKhachHangBan.ContainsKey(soBanHienTai))
            {
                thongTinKhachHangBan[soBanHienTai] = (txtTenKhach.Text.Trim(), txtSDT.Text.Trim());
            }

            // Cập nhật bàn hiện tại
            soBanHienTai = soBan;

            if (!trangThaiBan.ContainsKey(soBan)) return;

            if (!trangThaiBan[soBan])
            {
                btnBan.Background = Brushes.LightGreen;
                trangThaiBan[soBan] = true;
                CapNhatTrangThaiBan(soBan, "Đang sử dụng");
            }

            if (banDangChon != null)
            {
                string banCu = banDangChon.Content.ToString().Split('\n')[0].Trim();
                if (trangThaiBan.ContainsKey(banCu) && !trangThaiBan[banCu])
                {
                    banDangChon.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A"));
                }
            }

            banDangChon = btnBan;

            // Đảm bảo từ điển có mục cho bàn này
            if (!banHoaDon.ContainsKey(soBan))
            {
                banHoaDon[soBan] = new ObservableCollection<mon>();
            }

            // Đặt lại cờ laDonMangVe khi chọn bàn
            laDonMangVe = false;

            // Xóa danh sách món hiện tại
            DanhSachMon.Clear();

            // Thêm món từ bàn được chọn
            foreach (var item in banHoaDon[soBan])
            {
                DanhSachMon.Add(item);
            }

            // Đặt lại ItemsSource của DataGrid
            dataGridMon.ItemsSource = DanhSachMon;
            dataGridMon.Items.Refresh();
            CapNhatTongTien();

            // Kiểm tra và đặt lại thông tin khách hàng khi chuyển bàn
            if (thongTinKhachHangBan.ContainsKey(soBan))
            {
                txtTenKhach.Text = thongTinKhachHangBan[soBan].TenKhach;
                txtSDT.Text = thongTinKhachHangBan[soBan].SDT;

                // Ẩn placeholder labels nếu có nội dung
                lblTenKhach.Visibility = string.IsNullOrWhiteSpace(txtTenKhach.Text) ? Visibility.Visible : Visibility.Collapsed;
                lblSDT.Visibility = string.IsNullOrWhiteSpace(txtSDT.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                // Chỉ reset nếu không có thông tin trước đó
                if (string.IsNullOrWhiteSpace(txtTenKhach.Text) && string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    txtTenKhach.Text = "";
                    txtSDT.Text = "";
                    lblTenKhach.Visibility = Visibility.Visible;
                    lblSDT.Visibility = Visibility.Visible;
                }
            }

            gridBanAn.Visibility = Visibility.Collapsed;
            gridHoaDon.Visibility = Visibility.Visible;
            MoForm(new TraSua(this));
        }
        private void CapNhatTrangThaiBan(string soBan, string trangThai)
        {
            string query = "UPDATE Ban SET trangthai = @TrangThai WHERE banSo = @BanSo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@BanSo", soBan);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ThemMon(mon mon)
        {
            if (mon == null) return;

            string soBan = laDonMangVe ? "MangVề" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
            if (string.IsNullOrEmpty(soBan))
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!banHoaDon.ContainsKey(soBan)) banHoaDon[soBan] = new ObservableCollection<mon>();
            var danhSachCuaBan = banHoaDon[soBan];

            mon monDaTonTai = danhSachCuaBan.FirstOrDefault(x => x.MaSP == mon.MaSP);
            if (monDaTonTai != null)
            {
                monDaTonTai.SoLuong++;
            }
            else
            {
                mon.SoLuong = 1;
                danhSachCuaBan.Add(mon);

                // Cũng thêm vào DanhSachMon để nó hiển thị ngay lập tức
                DanhSachMon.Add(mon);
            }

            dataGridMon.Items.Refresh();
            CapNhatTongTien();
        }
        private void CapNhatTongTien()
        {
            decimal tongTien = DanhSachMon.Sum(mon => mon.ThanhTien);
            lblTongTien.Text = $"Tổng tiền: {tongTien:N0} VNĐ";
        }
        private void DatMangVe_Click(object sender, RoutedEventArgs e)
        {
            laDonMangVe = true;
            banDangChon = null;

            // Lưu thông tin khách hàng của bàn hiện tại trước khi chuyển
            if (!string.IsNullOrEmpty(soBanHienTai) && thongTinKhachHangBan.ContainsKey(soBanHienTai))
            {
                thongTinKhachHangBan[soBanHienTai] = (txtTenKhach.Text.Trim(), txtSDT.Text.Trim());
            }

            soBanHienTai = "MangVề";

            if (!banHoaDon.ContainsKey("MangVề"))
            {
                banHoaDon["MangVề"] = new ObservableCollection<mon>();
            }

            // Xóa danh sách món hiện tại
            DanhSachMon.Clear();

            // Thêm món từ mảng mang về
            foreach (var item in banHoaDon["MangVề"])
            {
                DanhSachMon.Add(item);
            }

            // Đặt lại ItemsSource
            dataGridMon.ItemsSource = DanhSachMon;
            dataGridMon.Items.Refresh();
            CapNhatTongTien();

            gridBanAn.Visibility = Visibility.Collapsed;
            gridHoaDon.Visibility = Visibility.Visible;
            gridMon.Visibility = Visibility.Visible;

            MoForm(new TraSua(this));
        }
        public void ResetBanSauKhiIn(string soBan)
        {            
            // Xóa thông tin khách hàng của bàn
            if (thongTinKhachHangBan.ContainsKey(soBan))
            {
                thongTinKhachHangBan.Remove(soBan);
            }

            // Reset các trường nhập liệu
            txtTenKhach.Text = "";
            txtSDT.Text = "";
            lblTenKhach.Visibility = Visibility.Visible;
            lblSDT.Visibility = Visibility.Visible;

            // Reset bàn hiện tại
            soBanHienTai = "";
            if (!string.IsNullOrEmpty(soBan) && banHoaDon.ContainsKey(soBan))
            {
                banHoaDon[soBan].Clear();
            }

            DanhSachMon.Clear();
            dataGridMon.ItemsSource = null;
            dataGridMon.Items.Refresh();

            txtTenKhach.Text = "";
            txtSDT.Text = "";

            if (!laDonMangVe && !string.IsNullOrEmpty(soBan))
            {
                trangThaiBan[soBan] = false;
                CapNhatTrangThaiBan(soBan, "Trống");

                foreach (Button btnBan in banAnContainer.Children)
                {
                    if (btnBan.Content.ToString().StartsWith(soBan))
                    {
                        btnBan.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D49A6A"));
                        break;
                    }
                }
            }
            gridHoaDon.Visibility = Visibility.Collapsed;
            gridBanAn.Visibility = Visibility.Visible;
        }
        public void GiamGiaHoaDon(decimal giamGia, int diemDung, string sdtKhach)
        {
            try
            {
                if (string.IsNullOrEmpty(sdtKhach))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại khách hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                decimal tongTien = DanhSachMon.Sum(mon => mon.ThanhTien);
                decimal tongTienSauGiam = tongTien - giamGia;

                // Hiển thị tổng tiền sau giảm giá
                lblTongTien.Text = $"Tổng tiền: {tongTienSauGiam:N0} VNĐ (Giảm: {giamGia:N0} VNĐ)";

                // Tính điểm tích lũy mới (50,000đ = 1 điểm)
                int diemTichLuy = (int)(tongTien / 50000);

                // Cập nhật điểm tích lũy
                bool ketQua = modify.CapNhatDiemTichLuy(sdtKhach, diemDung, diemTichLuy);
                if (!ketQua)
                {
                    MessageBox.Show("Không thể cập nhật điểm tích lũy. Vui lòng thử lại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Cập nhật điểm tích lũy thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                dataGridMon.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật điểm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Phương thức chung để reset trạng thái bàn sau khi in hóa đơn
        public void ResetBan()
        {
            string soBan = laDonMangVe ? "Mang Về" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
            ResetBanSauKhiIn(soBan);
        }
        private void InDon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DanhSachMon == null || DanhSachMon.Count == 0)
                {
                    MessageBox.Show("Không có món nào để in hóa đơn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string tenKhach = txtTenKhach.Text.Trim();
                string sdtKhach = txtSDT.Text.Trim();
                string ghiChu = txtGhichu.Text.Trim();  // Lấy nội dung ghi chú
                string soBan = laDonMangVe ? "Mang Về" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
                decimal tongTien = DanhSachMon.Sum(mon => mon.ThanhTien);

                // Kiểm tra điểm tích lũy của khách hàng nếu có số điện thoại
                decimal giamGia = 0;
                int diemDung = 0;

                if (!string.IsNullOrEmpty(sdtKhach))
                {
                    int diemHienCo = modify.LayDiemTichLuy(sdtKhach);
                    if (diemHienCo > 0)
                    {
                        TichDiem tichDiem = new TichDiem(this, tenKhach, sdtKhach, diemHienCo);
                        if (tichDiem.ShowDialog() == true)
                        {
                            // Lấy giá trị giảm giá từ label tổng tiền
                            string tongTienText = lblTongTien.Text;
                            if (tongTienText.Contains("Giảm:"))
                            {
                                string giamGiaText = tongTienText.Substring(tongTienText.IndexOf("Giảm: ") + 6,
                                    tongTienText.IndexOf(" VNĐ", tongTienText.IndexOf("Giảm: ")) - (tongTienText.IndexOf("Giảm: ") + 6));
                                giamGia = decimal.Parse(giamGiaText.Replace(",", ""));
                                diemDung = (int)(giamGia / 100); // 1 điểm = 100 VNĐ khi sử dụng
                            }
                        }
                    }
                }

                // Tạo mã hóa đơn và lấy thông tin ngày
                string maHoaDon = modify.TaoMaHoaDon();
                string ngayLap = DateTime.Now.ToString("dd/MM/yyyy");

                // Xử lý maKH - Tạo hoặc lấy mã khách hàng
                string maKH = modify.LayMaKH(tenKhach, sdtKhach);
                if (string.IsNullOrEmpty(maKH))
                {
                    MessageBox.Show("Không thể tạo mã khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lưu hóa đơn vào CSDL
                if (modify.ThemHoaDon(maHoaDon, maKH, soBan, tongTien - giamGia))
                {
                    modify.ThemChiTietHoaDon(maHoaDon, DanhSachMon.ToList());

                    // Cập nhật điểm tích lũy sau khi lưu hóa đơn thành công
                    if (!string.IsNullOrEmpty(sdtKhach))
                    {
                        // Tính điểm tích lũy mới (50,000đ = 1 điểm)
                        int diemTichLuy = (int)(tongTien / 50000);

                        // Cập nhật điểm vào database
                        bool ketQua = modify.CapNhatDiemTichLuy(sdtKhach, diemDung, diemTichLuy);
                        if (!ketQua)
                        {
                            MessageBox.Show("Không thể cập nhật điểm tích lũy. Vui lòng kiểm tra lại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                    // Mở bill để in
                    bill billWindow = new bill(
                        this,
                        maHoaDon,
                        ngayLap,
                        tenKhach,
                        sdtKhach,
                        soBan,
                        giamGia,
                        ghiChu,  // Truyền ghi chú vào bill
                        new ObservableCollection<mon>(DanhSachMon)
                    );
                    billWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Lưu hóa đơn thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txtTenKhach_TextChanged(object sender, TextChangedEventArgs e)
        {
            CapNhatThongTinKhachHang();
            lblTenKhach.Visibility = string.IsNullOrWhiteSpace(txtTenKhach.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void txtSDT_TextChanged(object sender, TextChangedEventArgs e)
        {
            CapNhatThongTinKhachHang();
            lblSDT.Visibility = string.IsNullOrWhiteSpace(txtSDT.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == txtTenKhach) lblTenKhach.Visibility = Visibility.Collapsed;
            if (sender == txtSDT) lblSDT.Visibility = Visibility.Collapsed;
            if (sender == txtGhichu) lblGhichu.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocusKhach(object sender, RoutedEventArgs e)
        {
            if (sender == txtTenKhach && string.IsNullOrWhiteSpace(txtTenKhach.Text))
                lblTenKhach.Visibility = Visibility.Visible;

            if (sender == txtSDT && string.IsNullOrWhiteSpace(txtSDT.Text))
                lblSDT.Visibility = Visibility.Visible;
            if (sender == txtGhichu && string.IsNullOrWhiteSpace(txtGhichu.Text))
                lblGhichu.Visibility = Visibility.Visible;
        }
        private void MoForm(UserControl newForm)
        {
            if (activeform != null && gridMon.Children.Contains(activeform))
            {
                gridMon.Children.Remove(activeform);
            }

            activeform = newForm;
            gridMon.Children.Add(activeform);

            // 🔹 Đảm bảo khu vực chọn món hiển thị
            gridMon.Visibility = Visibility.Visible;
            gridMon.UpdateLayout(); // Làm mới giao diện
        }
        private void Menu_Trasua_Click(object sender, RoutedEventArgs e)
        {
            MoForm(new TraSua(this)); // Thêm vào gridMon thay vì gridHoaDon
        }
        private void Menu_AnVat_Click(object sender, RoutedEventArgs e)
        {
            MoForm(new DoAnVat(this)); // Thêm vào gridMon thay vì gridHoaDon
        }
        private void QuayLai_Click(object sender, RoutedEventArgs e)
        {
            // Lưu thông tin khách hàng của bàn hiện tại trước khi quay lại
            if (!string.IsNullOrEmpty(soBanHienTai) && banDangChon != null)
            {
                thongTinKhachHangBan[soBanHienTai] = (txtTenKhach.Text.Trim(), txtSDT.Text.Trim());
            }
            // Nếu có form đang hiển thị, xóa nó khỏi gridMon
            if (activeform != null && gridMon.Children.Contains(activeform))
            {
                gridMon.Children.Remove(activeform);
                activeform = null;
            }
            // 🔹 Đảm bảo hóa đơn luôn hiển thị
            gridHoaDon.Visibility = Visibility.Visible;

            // 🔹 Hiển thị lại danh sách bàn
            gridBanAn.Visibility = Visibility.Visible;

            // 🔹 Kiểm tra xem có bàn nào đang chọn không
            if (banDangChon != null)
            {
                string soBan = banDangChon.Content.ToString().Split('\n')[0].Trim();

                // Lấy danh sách món theo bàn
                if (banHoaDon.ContainsKey(soBan))
                {
                    dataGridMon.ItemsSource = banHoaDon[soBan];
                }
                else
                {
                    dataGridMon.ItemsSource = new ObservableCollection<mon>();
                }
                dataGridMon.Items.Refresh();
            }
        }
        private void TangSoLuong_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var mon = button.DataContext as mon;
            if (mon != null)
            {
                mon.SoLuong++;
                dataGridMon.Items.Refresh();
                CapNhatTongTien();
            }
        }
        private void GiamSoLuong_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var mon = button.DataContext as mon;
            if (mon != null && mon.SoLuong > 1)
            {
                mon.SoLuong--;
                dataGridMon.Items.Refresh();
                CapNhatTongTien();
            }
            else if (mon != null && mon.SoLuong == 1)
            {
                string soBan = laDonMangVe ? "MangVề" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
                if (banHoaDon.ContainsKey(soBan))
                {
                    banHoaDon[soBan].Remove(mon);
                    dataGridMon.Items.Refresh();
                    CapNhatTongTien();
                }
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Chỉ cho phép nhập số
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var mon = textBox.DataContext as mon;
                if (mon != null)
                {
                    if (int.TryParse(textBox.Text, out int soLuong))
                    {
                        if (soLuong <= 0)
                        {
                            // Nếu số lượng <= 0, xóa món khỏi danh sách
                            string soBan = laDonMangVe ? "MangVề" : banDangChon?.Content.ToString().Split('\n')[0].Trim();
                            if (banHoaDon.ContainsKey(soBan))
                            {
                                banHoaDon[soBan].Remove(mon);
                                dataGridMon.Items.Refresh();
                                CapNhatTongTien();
                            }
                        }
                        else
                        {
                            // Cập nhật số lượng mới
                            mon.SoLuong = soLuong;
                            dataGridMon.Items.Refresh();
                            CapNhatTongTien();
                        }
                    }
                    else
                    {
                        // Nếu không phải số hợp lệ, khôi phục số lượng cũ
                        textBox.Text = mon.SoLuong.ToString();
                    }
                }
            }
        }

    }
}

