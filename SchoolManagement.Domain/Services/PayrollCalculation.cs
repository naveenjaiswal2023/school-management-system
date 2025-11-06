using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Services
{
    public class PayrollCalculation
    {
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalAllowances { get; set; }
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public decimal LossOfPayAmount { get; set; }
    }
}
