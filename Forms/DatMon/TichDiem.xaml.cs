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
using System.Windows.Shapes;

namespace QLTS.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for TichDiem.xaml
    /// </summary>
    public partial class TichDiem : Window
    {
        private Modify modify = new Modify();
        private DatMon datMon;
        private string sdtKhach;
        private int diemTichLuy;

        public TichDiem(DatMon datMonInstance, string tenKhach, string sdt, int diem)
        {
            InitializeComponent();
            this.datMon = datMonInstance;
            this.sdtKhach = sdt;
            
            // Lấy số điểm mới nhất từ database
            this.diemTichLuy = modify.LayDiemTichLuy(sdt);

            lblTenKhach.Text = tenKhach;
            lblSDT.Text = sdt;
            lblDiemTichLuy.Text = diemTichLuy.ToString();
        }

        private void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra và xác nhận điểm sử dụng
            if (string.IsNullOrWhiteSpace(txtDiemSuDung.Text) || txtDiemSuDung.Text.Trim() == "0")
            {
                // Nếu không sử dụng điểm, đóng form tích điểm
                this.DialogResult = false;
                this.Close();
                return;
            }

            if (!int.TryParse(txtDiemSuDung.Text, out int diemMuonDung) || diemMuonDung < 0)
            {
                MessageBox.Show("Số điểm không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra lại số điểm hiện có từ database
            int diemHienTai = modify.LayDiemTichLuy(sdtKhach);
            if (diemMuonDung > diemHienTai)
            {
                MessageBox.Show("Không đủ điểm tích lũy!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Quy đổi điểm: 1 điểm = 100 VNĐ
            decimal giamGia = diemMuonDung * 100;

            // Gọi hàm cập nhật giảm giá trong DatMon
            datMon.GiamGiaHoaDon(giamGia, diemMuonDung, sdtKhach);

            this.DialogResult = true;
            this.Close();
        }
    }
}
