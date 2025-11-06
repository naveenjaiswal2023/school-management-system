using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Services
{
    public interface ISalaryCalculationService
    {
        decimal CalculateGrossSalary(Salary baseSalary, IEnumerable<Allowance> allowances);
        decimal CalculateNetSalary(decimal grossSalary, IEnumerable<Deduction> deductions);
        PayrollCalculation ProcessPayroll(Employee employee, IEnumerable<EmployeeAttendance> attendances, DateTime payrollMonth);
    }
}
