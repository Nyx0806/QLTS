using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Models.TongDoanhThu
{
    class doanhthumax
    {
        private int thang;
        private decimal doanhthu;

        public doanhthumax()
        {
        }

        public doanhthumax(int thang, decimal doanhthu)
        {
            this.thang = thang;
            this.doanhthu = doanhthu;
        }

        public int Thang { get => thang; set => thang = value; }
        public decimal Doanhthu { get => doanhthu; set => doanhthu = value; }
    }
}
