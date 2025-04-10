using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Models
{
    public class Employee
    {
        private string _employeeId;
        public string EmployeeId
        {

            get { return _employeeId; }
            set
            {
                _employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }


        public string Name { get; set; }  // hoten
        public string Position { get; set; }  // chucvu
        public decimal Salary { get; set; }  // luong
        public string Email { get; set; }  // Email
        public string PhoneNumber { get; set; }  // sdt
        public string Status { get; set; }
        public string Gender { get; set; }

        // Danh sách trạng thái có thể chọn
        public string StatusList { get; set; }
        public DateTime DateOfBirth { get; set; }  // ngay sinh

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class l
    {
        public static ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
    }
}

