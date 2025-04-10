using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Models.TongDoanhThu
{
    class doanhthulowest
    {
        private int thang;
        private decimal doanhthu;

        public doanhthulowest()
        {
        }

        public doanhthulowest(int thang, decimal doanhthu)
        {
            this.thang = thang;
            this.doanhthu = doanhthu;
        }

        public int Thang { get => thang; set => thang = value; }
        public decimal Doanhthu { get => doanhthu; set => doanhthu = value; }
    }
}
