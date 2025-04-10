using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Models
{
    class luunhanvien
    {
        private string maNV;
        private string hoten;
        private string chucvu;
        private decimal luong;
        private string email;
        private string sdt;
        private string trangthai;
        private DateTime ngaysinh;
        private string gioitinh;

        public luunhanvien()
        {
        }

        public luunhanvien(string maNV, string hoten, string chucvu, decimal luong, string email, string sdt, string trangthai, DateTime ngaysinh, string gioitinh)
        {
            this.maNV = maNV;
            this.hoten = hoten;
            this.chucvu = chucvu;
            this.luong = luong;
            this.email = email;
            this.sdt = sdt;
            this.trangthai = trangthai;
            this.ngaysinh = ngaysinh;
            this.gioitinh = gioitinh;
        }

        public string MaNV { get => maNV; set => maNV = value; }
        public string Hoten { get => hoten; set => hoten = value; }
        public string Chucvu { get => chucvu; set => chucvu = value; }
        public decimal Luong { get => luong; set => luong = value; }
        public string Email { get => email; set => email = value; }
        public string Sdt { get => sdt; set => sdt = value; }
        public string Trangthai { get => trangthai; set => trangthai = value; }
        public DateTime Ngaysinh { get => ngaysinh; set => ngaysinh = value; }
        public string Gioitinh { get => gioitinh; set => gioitinh = value; }
    }
}
