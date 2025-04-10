using QLTS.Models;
using QLTS.Sql ;
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
using QLTS.Forms.Nut;

namespace QLTS.Forms.DatMon
{
    /// <summary>
    /// Interaction logic for DoAnVat.xaml
    /// </summary>
    public partial class DoAnVat : UserControl
    { 
        private DatMon datMon;
        Modify modify = new Modify();

        public DoAnVat(DatMon datMonInstance)
        {
            InitializeComponent();
            this.datMon = datMonInstance ?? throw new ArgumentNullException(nameof(datMonInstance));
            TaiDanhSachMon();
        }
        public void TaiDanhSachMon()
        {
            string query = "select * from SanPham where loai = N'Ăn Vặt' and tinhTrang = N'Còn Bán'";
            List<mon> list = modify.mons(query);
            foreach (mon mon in list)
            {
                nutmon bt_mon = new nutmon();
                bt_mon.Height = 241;
                bt_mon.Width = 191;
                bt_mon.ma = mon.MaSP;
                bt_mon.TenMon = mon.TenSP;
                bt_mon.Gia = mon.Gia.ToString();
                bt_mon.Anh = mon.Anh;
                bt_mon.TT = mon.TinhTrang;
                unif.Children.Add(bt_mon);
                bt_mon.Click += Nutmon_Click;
            }
        }
        private void Nutmon_Click(object sender, string e)
        {
            string maSanPham = e;
            mon mons = modify.LayThongTinMon(maSanPham);
            if (mons != null)
            {
                ThemMonVaoDatMon(mons);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin món!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ThemMonVaoDatMon(mon mons)
        {
            datMon?.ThemMon(mons); // 🔹 Gọi ThemMon từ DatMon
        }
    }
}
