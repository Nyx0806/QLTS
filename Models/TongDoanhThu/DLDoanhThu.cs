using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTS.Sql;

namespace QLTS.Models.TongDoanhThu
{
    class DLDoanhThu
    {
        public class DoanhThuThang
        {
            public int Thang { get; set; }
            public decimal TongTien { get; set; }
        }

        Modify Modify = new Modify();
        public void dldoanhthu(List<double> listthang)
        {
            List<string> lenh = new List<string>
            {
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '1' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '2' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '3' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '4' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '5' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '6' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '7' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '8' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '9' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '10' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '11' AND YEAR(ngayLap) = YEAR(GETDATE())",
                "SELECT * FROM HoaDon WHERE MONTH(ngayLap) = '12' AND YEAR(ngayLap) = YEAR(GETDATE())",
            };

            for (int i = 0; i < lenh.Count; i++)
            {
                decimal ttien = 0;
                List<HoaDon> hoedon = Modify.HoaDons(lenh[i]);
                foreach (var ho in hoedon)
                {
                    ttien += ho.TongTien;
                }
                double phantram = (120000000 == 0) ? 0 : (double)ttien / 120000000 * 100;
                listthang.Add(phantram);
            }

        }

    }
}
