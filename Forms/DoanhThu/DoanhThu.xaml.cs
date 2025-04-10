using QLTS.Models.TongDoanhThu;
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
using System.Windows.Shapes;

namespace QLTS.Forms.DoanhThu
{
    /// <summary>
    /// Interaction logic for DoanhThu.xaml
    /// </summary>
    public partial class DoanhThu : UserControl
    {
        DLDoanhThu DL = new DLDoanhThu();
        Modify modify = new Modify();
      
        private List <double>dlbang = new List <double> ();

        public DoanhThu()
        {
            InitializeComponent();
            DL.dldoanhthu(dlbang);
            tgcot();
            dtmax();
            dtmin();
        }
        private void tgcot()
        {
            List<Border> cot = new List<Border>
            {
                ct1, ct2, ct3, ct4, ct5, ct6, ct7, ct8, ct9, ct10, ct11, ct12
            };
            for (int j = 0; j < cot.Count; j++)
            {
                double chieucao = dlbang[j] / 100 * 309; 
                cot[j].Height = chieucao + 5;
            }
        }

        private void dtmax()
        {
            string tmax = "SELECT TOP 1 MONTH(ngayLap) AS Thang, SUM(tongTien) AS TongTien FROM HoaDon WHERE YEAR(ngayLap) = YEAR(GETDATE()) GROUP BY MONTH(ngayLap) ORDER BY TongTien DESC";
            doanhthumax dtmax = modify.DoanhThuThangMax(tmax);

            if (dtmax != null)
            {
                tbtnhiunhat.Text = $"Tháng {dtmax.Thang}:  {(int)dtmax.Doanhthu} VNĐ";

            }
        }

        private void dtmin()
        {
            string tmin = "SELECT TOP 1 MONTH(ngayLap) AS Thang, SUM(tongTien) AS TongTien FROM HoaDon WHERE YEAR(ngayLap) = YEAR(GETDATE()) GROUP BY MONTH(ngayLap) ORDER BY TongTien ASC";
            doanhthulowest dtmin = modify.DoanhThuThangLowest(tmin);

            if (dtmin != null)
            {
                tbtitnha.Text = $"Tháng {dtmin.Thang}:  {(int)dtmin.Doanhthu} VNĐ";
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
