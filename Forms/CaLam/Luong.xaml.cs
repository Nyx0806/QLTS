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
using QLTS.Forms.Nut;

namespace QLTS.Forms.CaLam
{
    /// <summary>
    /// Interaction logic for Luong.xaml
    /// </summary>
    public partial class Luong : UserControl
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;
        public Luong()
        {
            InitializeComponent();
            LoadLuongData();
        }

        private void LoadLuongData()
        {
            string query = @"
            SELECT nv.maNV, nv.hoten, nv.chucvu, nv.luong, ISNULL(SUM(cc.TongGioLam), 0) AS TongGioLam
                    FROM NhanVien nv
                    LEFT JOIN ChamCong cc ON nv.maNV = cc.maNV
                    WHERE nv.chucvu != N'Quản lí'
                    GROUP BY nv.maNV, nv.hoten, nv.chucvu, nv.luong";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string maNV = reader["maNV"].ToString();
                    string hoTen = reader["hoten"].ToString();
                    decimal luong = reader.GetDecimal(reader.GetOrdinal("luong"));
                    decimal tongGioLam = reader.GetDecimal(reader.GetOrdinal("TongGioLam"));
                    string chucVu = reader["chucvu"].ToString();
                    decimal tongLuong = luong * tongGioLam;

                    var hienLuongUC = new HienLuong();
                    hienLuongUC.DataContext = new
                    {
                        MaNV = maNV,
                        HoTen = hoTen,
                        TongLuong = tongLuong,
                        ChucVu = chucVu,
                        TongGioLam = tongGioLam
                    };

                    unif.Children.Add(hienLuongUC);
                }
            }
        }
    }

}
