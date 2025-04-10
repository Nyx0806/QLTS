using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QLTS.Models
{
    public class mon
    {
        private string maSP;
        private string tenSP;
        private decimal gia;
        private string loai;
        private int _soLuong = 1;
        private BitmapImage anh;
        private string tinhTrang;
        private decimal _thanhTien;
        public decimal ThanhTien
        {
            get { return SoLuong * Gia; }
            set { _thanhTien = value; }  // Cho phép gán nhưng không làm mất logic
        }

        public mon() { }

        public mon(string maSP, string tenSP, decimal gia, string loai,int _soLuong, BitmapImage anh, string tinhTrang)
        {
            this.maSP = maSP;
            this.tenSP = tenSP;
            this.gia = gia;
            this.loai = loai;
            this._soLuong = _soLuong;
            this.anh = anh;
            this.tinhTrang = tinhTrang;

        }
        public string MaSP { get => maSP; set => maSP = value; }
        public string TenSP { get => tenSP; set => tenSP = value; }
        public decimal Gia { get => gia; set => gia = value; }
        public string Loai { get => loai; set => loai = value; }
        public int SoLuong { get => _soLuong; set => _soLuong = value; }
        public BitmapImage Anh { get => anh; set => anh = value; }
        public string TinhTrang { get => tinhTrang; set => tinhTrang = value; }

    }
}
