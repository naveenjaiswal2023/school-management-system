using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Status { get; set; }
        public DepartmentDto Department { get; set; }
        public DesignationDto Designation { get; set; }
        public string EmploymentType { get; set; }
        public SalaryDto SalaryInfo { get; set; }
        public AddressDto Address { get; set; }
        public string PhotoUrl { get; set; }
        public bool BiometricEnrolled { get; set; }
    }
}
