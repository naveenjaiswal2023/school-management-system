using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class SalaryDto
    {
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal DA { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal GrossSalary { get; set; }
    }
}
