using QLTS.Forms.Nut;
using QLTS.Models;
using QLTS.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace QLTS.Forms.NhanVien
{
    /// <summary>
    /// Interaction logic for ThuNgan.xaml
    /// </summary>
    public partial class ThuNgan : UserControl
    {
        Modify modify = new Modify();
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public ThuNgan()
        {
            InitializeComponent();
            this.DataContext = this;
            addnhanvien();
        }

        private void addnhanvien()
        {
            l.Employees.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT maNV, hoten, chucvu, luong, Email, sdt, TrangThai, gioitinh, ngaysinh FROM NhanVien WHERE chucvu = N'Thu Ngân'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        l.Employees.Add(new Employee
                        {
                            EmployeeId = reader["maNV"].ToString(),
                            Name = reader["hoten"].ToString(),
                            Position = reader["chucvu"].ToString(),
                            Salary = reader["luong"] != DBNull.Value ? reader.GetDecimal(reader.GetOrdinal("luong")) : 0,
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                            PhoneNumber = reader["sdt"] != DBNull.Value ? reader["sdt"].ToString() : "",
                            Status = reader["TrangThai"].ToString(),
                            DateOfBirth = reader["ngaysinh"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("ngaysinh")) : DateTime.MinValue,
                            Gender = reader["gioiTinh"] != DBNull.Value && reader.GetBoolean(reader.GetOrdinal("gioiTinh")) ? "Nam" : "Nữ"
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                }
            }

            foreach (Employee emp in l.Employees)
            {
                nutnv tn = new nutnv();
                tn.manv = emp.EmployeeId;
                tn.ten = emp.Name;
                tn.ngaysinh = emp.DateOfBirth.ToString("dd/MM/yyyy");
                tn.sdt = emp.PhoneNumber;
                tn.trangthai = emp.Status;
                tn.Luong = emp.Salary.ToString();
                tn.email = emp.Email;
                tn.gioitinh = emp.Gender;

                unif.Children.Add(tn);
                tn.Click += Tn_Click;
            }
        }

        private void Tn_Click(object sender, string e)
        {
            ThongTin thongTin = new ThongTin(e);
            thongTin.ShowDialog();
        }
    }
}
