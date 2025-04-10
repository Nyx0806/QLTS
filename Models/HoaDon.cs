using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Models
{
    class HoaDon
    {
        private string maHoaDon;
        private string maKhachHang;
        private string banSo;
        private DateTime ngayLapHoaDon;
        private decimal tongTien;

        public HoaDon()
        {
        }

        public HoaDon(string maHoaDon, string maKhachHang, string banSo, DateTime ngayLapHoaDon, decimal tongTien)
        {
            this.maHoaDon = maHoaDon;
            this.maKhachHang = maKhachHang;
            this.banSo = banSo;
            this.ngayLapHoaDon = ngayLapHoaDon;
            this.tongTien = tongTien;
        }

        public string MaHoaDon { get => maHoaDon; set => maHoaDon = value; }
        public string MaKhachHang { get => maKhachHang; set => maKhachHang = value; }
        public string BanSo { get => banSo; set => banSo = value; }
        public DateTime NgayLapHoaDon { get => ngayLapHoaDon; set => ngayLapHoaDon = value; }
        public decimal TongTien { get => tongTien; set => tongTien = value; }
    }
}
