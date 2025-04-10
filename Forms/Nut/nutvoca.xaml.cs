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

namespace QLTS.Forms.Nut
{
    /// <summary>
    /// Interaction logic for nutvoca.xaml
    /// </summary>
    public partial class nutvoca : UserControl
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;
        private string maNV;
        private DateTime gioVao;

        public nutvoca(string hoten, DateTime gioVao)
        {
            InitializeComponent();
            txtTenNV.Text = $"Tên: {hoten}";
            txtGioVao.Text = $"Giờ vào: {gioVao.ToString("HH:mm")}";
            txtNgaylam.Text = $"Ngày làm: {gioVao:dd/MM/yyyy}";
            this.gioVao = gioVao;
        }

        private void BtnKetThucCa_Click(object sender, RoutedEventArgs e)
        {
            DateTime gioRa = DateTime.Now;
            TimeSpan tongGio = gioRa - gioVao;

            txtGioRa.Text = $"Giờ ra: {gioRa:HH:mm}";
            txtTongThoiGian.Text = $"Tổng thời gian: {tongGio.Hours} giờ {tongGio.Minutes} phút";

            // Chuyển TimeSpan thành float (giờ)
            float tongGioFloat = (float)tongGio.TotalHours;

            // Cập nhật vào database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(
                    "UPDATE ChamCong SET thoiGianRa = @gioRa, TongGioLam = @TongGioLam WHERE maNV = @maNV AND thoiGianVao = @gioVao", conn);

                cmdUpdate.Parameters.AddWithValue("@gioRa", gioRa);
                cmdUpdate.Parameters.AddWithValue("@gioVao", gioVao);
                cmdUpdate.Parameters.AddWithValue("@maNV", maNV);
                cmdUpdate.Parameters.AddWithValue("@TongGioLam", tongGioFloat);

                cmdUpdate.ExecuteNonQuery();
            }
        }


        public void SetMaNV(string maNV)
        {
            this.maNV = maNV;
        }
    }
}
