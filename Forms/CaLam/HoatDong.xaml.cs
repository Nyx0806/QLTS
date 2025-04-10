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
using QLTS.Sql;

namespace QLTS.Forms.CaLam
{
    /// <summary>
    /// Interaction logic for HoatDong.xaml
    /// </summary>
    public partial class HoatDong : UserControl
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;

        public HoatDong()
        {
            InitializeComponent();
            LoadNutVoCa();
        }
        private void LoadNutVoCa()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"SELECT nv.maNV, nv.hoTen, cc.thoiGianVao 
                                   FROM ChamCong cc
                                   JOIN NhanVien nv ON cc.maNV = nv.maNV
                                   WHERE cc.thoiGianRa IS NULL";  // Chỉ lấy ca chưa kết thúc

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maNV = reader["maNV"].ToString();
                            string hoTen = reader["hoTen"].ToString();
                            DateTime gioVao = Convert.ToDateTime(reader["thoiGianVao"]);

                            // Tạo nutvoca
                            var nut = new QLTS.Forms.Nut.nutvoca(hoTen, gioVao);
                            nut.SetMaNV(maNV);

                            // Thêm vào WrapPanel
                            unif.Children.Add(nut);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Lỗi khi tải dữ liệu ca làm: {ex.Message}", "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
