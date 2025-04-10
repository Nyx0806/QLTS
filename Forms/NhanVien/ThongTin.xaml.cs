using QLTS.Models;
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
using System.Windows.Shapes;

namespace QLTS.Forms.NhanVien
{
    /// <summary>
    /// Interaction logic for ThongTin.xaml
    /// </summary>
    public partial class ThongTin : Window
    {
        private string idnv;
        Modify modify = new Modify();
        private void chon()
        {
            ComboBox comboBox = new ComboBox();
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();


                switch (selectedText)
                {
                    case "Thu Ngân":
                        ThuNgan TN = new ThuNgan();
                        break;

                    case "Pha Chế":
                        PhaChe PC = new PhaChe();
                        break;

                    case "Phục Vụ":
                        PhucVu PV = new PhucVu();
                        break;

                    default:

                        break;
                }
            }
        }

        private List<string> chucvu = new List<string>
        {
            "Thu Ngân",
            "Pha Chế",
            "Phục Vụ"
        };
        private List<string> gioitinh = new List<string>
        {
            "Nam",
            "Nữ"
        };
        private List<string> trangthai = new List<string> { "Ðang Làm", "Đã Nghỉ" };
        public ThongTin(string id)
        {
            InitializeComponent();
            idnv = id;
            cmbTrangThai.ItemsSource = trangthai;
            cmbChucVu.ItemsSource = chucvu;
            cbGioitinh.ItemsSource = gioitinh;
            laythongtin();
        }
        private void laythongtin()
        {
            foreach (var emp in l.Employees)
            {
                if (emp != null)
                {
                    if (emp.EmployeeId == idnv)
                    {
                        txtma.Text = emp.EmployeeId;
                        txtHoTen.Text = emp.Name;
                        dpNgaySinh.SelectedDate = emp.DateOfBirth;
                        txtSDT.Text = emp.PhoneNumber;
                        txtEmail.Text = emp.Email;
                        cbGioitinh.SelectedItem = emp.Gender;
                        txtluong.Text = emp.Salary.ToString();
                        cmbChucVu.SelectedItem = emp.Position;
                        cmbTrangThai.SelectedItem = emp.Status;

                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (cbGioitinh.SelectedItem?.ToString() == "Nam")
            {
                string ngaySinh = dpNgaySinh.SelectedDate?.ToString("yyyy-MM-dd") ?? "NULL";
                string update = "update NhanVien set hoten = N'" + txtHoTen.Text + "', chucvu = N'" + cmbChucVu.SelectedItem.ToString() + "' , luong = '" + txtluong.Text + "', Email ='" + txtEmail.Text + "', sdt = '" + txtSDT.Text + "',TrangThai =N'" + cmbTrangThai.SelectedItem.ToString() + "' ,ngaysinh ='" + ngaySinh + "', gioitinh = 1 where maNV = '" + txtma.Text + "' ";
                modify.ThucThi(update);
                MessageBox.Show(" Đã lưu thông tin thành công ");

            }
            else
            {
                string ngaySinh = dpNgaySinh.SelectedDate?.ToString("yyyy-MM-dd") ?? "NULL";
                string update = "update NhanVien set hoten = N'" + txtHoTen.Text + "', chucvu = N'" + cmbChucVu.SelectedItem.ToString() + "' , luong = '" + txtluong.Text + "', Email ='" + txtEmail.Text + "', sdt = '" + txtSDT.Text + "',TrangThai =N'" + cmbTrangThai.SelectedItem.ToString() + "' ,ngaysinh ='" + ngaySinh + "', gioitinh = 0  where maNV = '" + txtma.Text + "' ";
                modify.ThucThi(update);
                MessageBox.Show("Đã lưu thông tin thành công ");
            }
        }
    }
}
