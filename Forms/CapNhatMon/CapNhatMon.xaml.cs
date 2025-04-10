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

namespace QLTS.Forms.CapNhatMon
{
    /// <summary>
    /// Interaction logic for CapNhatMon.xaml
    /// </summary>
    public partial class CapNhatMon : UserControl
    {
        private void Mo(Grid panel1, UserControl activeform, UserControl childform)
        {
            if (activeform != null)
            {
                panel1.Children.Remove(activeform); // Xóa giao diện cũ
            }
            activeform = childform; // Gán giao diện mới
            panel1.Children.Add(childform); // Thêm vào Grid
        }
        UserControl activeform = null;
        public CapNhatMon()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemMon themMon = new ThemMon();
            themMon.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Mo(CapNhap, activeform, new CapNhat_Trasua());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Mo(CapNhap, activeform, new CapNhat_Anvat());
        }
    }
}
