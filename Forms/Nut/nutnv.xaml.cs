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

namespace QLTS.Forms.Nut
{
    /// <summary>
    /// Interaction logic for nutnv.xaml
    /// </summary>
    public partial class nutnv : UserControl
    {
        public event EventHandler<string> Click;

        private string maNV;
        private string EmployeeName;
        private string Salary;
        private string Email;
        private string PhoneNumber;
        private string Status;
        private string Gender;
        private string DateOfBirth;

        public nutnv()
        {
            InitializeComponent();
        }

        public string manv
        {
            get { return maNV; }
            set { this.maNV = value; tbl_manhanvien.Text = this.maNV; }
        }
        public string ten
        {
            get { return EmployeeName; }
            set { this.EmployeeName = value; tbl_TenNhanVien.Text = this.EmployeeName; }
        }

        public string Luong
        {
            get { return Salary; }
            set { this.Salary = value; tbl_luong.Text = this.Salary; }
        }
        public string email
        {
            get { return Email; }
            set { this.Email = value; tbl_email.Text = this.Email; }
        }
        public string sdt
        {
            get { return PhoneNumber; }
            set { this.PhoneNumber = value; tbl_SDT.Text = this.PhoneNumber; }
        }
        public string trangthai
        {
            get { return Status; }
            set { this.Status = value; tbl_trangthai.Text = this.Status; }
        }
        public string gioitinh
        {
            get { return Gender; }
            set { this.Gender = value; tbl_GioiTinh.Text = this.Gender; }
        }
        public string ngaysinh
        {
            get { return DateOfBirth; }
            set { this.DateOfBirth = value; tbl_NgaySinh.Text = this.DateOfBirth; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string idnv = maNV;
            Click?.Invoke(this, idnv);
        }
    }
}
