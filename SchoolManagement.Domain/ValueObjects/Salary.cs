using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.ValueObjects
{
    public class Salary
    {
        public decimal BasicSalary { get; private set; }
        public decimal HRA { get; private set; }
        public decimal DA { get; private set; }
        public decimal SpecialAllowance { get; private set; }
        public decimal GrossSalary { get; private set; }

        private Salary() { } // EF Constructor

        public Salary(decimal basicSalary, decimal hra, decimal da, decimal specialAllowance)
        {
            BasicSalary = basicSalary;
            HRA = hra;
            DA = da;
            SpecialAllowance = specialAllowance;
            GrossSalary = BasicSalary + HRA + DA + SpecialAllowance;
        }
    }
}
