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
    /// Interaction logic for nutmon.xaml
    /// </summary>
    public partial class nutmon : UserControl
    {
        public event EventHandler<string> Click;

        private string maSP;
        private string tenSP;
        private string gia;
        private string trangThai;
        private BitmapImage anh;



        public nutmon()
        {
            InitializeComponent();
        }
        public string TT
        {
            get { return trangThai; }
            set { this.trangThai = value; tbl_trangthai.Text = this.trangThai; }
        }

        public string ma
        {
            get { return maSP; }
            set { this.maSP = value; }
        }
        public string TenMon
        {
            get { return tenSP; }
            set { this.tenSP = value; tbl_tenmon.Text = this.tenSP; }
        }
        public string Gia
        {
            get { return gia; }
            set { this.gia = value; tbl_gia.Text = this.gia; }
        }
        public BitmapImage Anh
        {
            get { return anh; }
            set { anh = value; ImagePreview.Source = anh; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string masp = maSP;
            Click?.Invoke(this, masp);
        }
    }
}
