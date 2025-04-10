using QLTS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for NhanVien.xaml
    /// </summary>
    public partial class NhanVien : UserControl
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public NhanVien()
        {
            InitializeComponent();
            Employees = new ObservableCollection<Employee>();

        }
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
        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();
                xulychon(selectedText);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemNV themNV = new ThemNV(this.Employees);
            themNV.ShowDialog();
            xulychon(cbchucvu.Text);
        }
        private void xulychon(string ChucVu)
        {

            // Xử lý khi chọn mục
            switch (ChucVu)
            {
                case "Thu Ngân":
                    Mo(chucvu, activeform, new ThuNgan());
                    break;

                case "Pha Chế":
                    Mo(chucvu, activeform, new PhaChe());
                    break;

                case "Phục Vụ":
                    Mo(chucvu, activeform, new PhucVu());
                    break;

                default:

                    break;
            }
        }

    }
    public static class cvNhanVien
    {
        public static string ChucVu;

    }
}

