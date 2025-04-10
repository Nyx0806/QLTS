using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Models
{
    class ChiTietHoaDon
    {
        private string cTHoaDon;
        private string maCT;
        private string maSanPham;
        private int soLuong;
        private DateTime ngayGhiDon;
        private decimal donGia;

        public ChiTietHoaDon()
        {
        }

        public ChiTietHoaDon(string cTHoaDon, string maCT, string maSanPham, int soLuong, DateTime ngayGhiDon, decimal donGia)
        {
            this.cTHoaDon = cTHoaDon;
            this.maCT = maCT;
            this.maSanPham = maSanPham;
            this.soLuong = soLuong;
            this.ngayGhiDon = ngayGhiDon;
            this.donGia = donGia;
        }

        public string CTHoaDon { get => cTHoaDon; set => cTHoaDon = value; }
        public string MaCT { get => maCT; set => maCT = value; }
        public string MaSanPham { get => maSanPham; set => maSanPham = value; }
        public int SoLuong { get => soLuong; set => soLuong = value; }
        public DateTime NgayGhiDon { get => ngayGhiDon; set => ngayGhiDon = value; }
        public decimal DonGia { get => donGia; set => donGia = value; }
    }
}
