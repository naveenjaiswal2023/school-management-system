using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.Services;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        public decimal CalculateGrossSalary(Salary baseSalary, IEnumerable<Allowance> allowances)
        {
            var totalAllowances = allowances.Where(a => a.IsActive)
                .Sum(a => a.IsPercentage ? (baseSalary.BasicSalary * a.Amount / 100) : a.Amount);

            return baseSalary.BasicSalary + baseSalary.HRA + baseSalary.DA + baseSalary.SpecialAllowance + totalAllowances;
        }

        public decimal CalculateNetSalary(decimal grossSalary, IEnumerable<Deduction> deductions)
        {
            var totalDeductions = deductions.Where(d => d.IsActive)
                .Sum(d => d.IsPercentage ? (grossSalary * d.Amount / 100) : d.Amount);

            return grossSalary - totalDeductions;
        }

        public PayrollCalculation ProcessPayroll(Employee employee, IEnumerable<EmployeeAttendance> attendances, DateTime payrollMonth)
        {
            var workingDays = GetWorkingDaysInMonth(payrollMonth);
            var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present || a.Status == AttendanceStatus.Late);

            var grossSalary = employee.SalaryInfo.GrossSalary;
            var dailyRate = grossSalary / workingDays;
            var lossOfPayAmount = (workingDays - presentDays) * dailyRate;

            var adjustedGrossSalary = grossSalary - lossOfPayAmount;
            var totalDeductions = CalculateStatutoryDeductions(adjustedGrossSalary);
            var netSalary = adjustedGrossSalary - totalDeductions;

            return new PayrollCalculation
            {
                GrossSalary = adjustedGrossSalary,
                NetSalary = netSalary,
                TotalDeductions = totalDeductions,
                TotalAllowances = 0, // Would be calculated from allowances
                WorkingDays = workingDays,
                PresentDays = presentDays,
                LossOfPayAmount = lossOfPayAmount
            };
        }

        private int GetWorkingDaysInMonth(DateTime month)
        {
            var firstDay = new DateTime(month.Year, month.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var workingDays = 0;

            for (var day = firstDay; day <= lastDay; day = day.AddDays(1))
            {
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            return workingDays;
        }

        private decimal CalculateStatutoryDeductions(decimal grossSalary)
        {
            var pfDeduction = grossSalary * 0.12m; // 12% PF
            var esiDeduction = grossSalary <= 21000 ? grossSalary * 0.0175m : 0; // 1.75% ESI if salary <= 21k
            var professionalTax = 200m; // Fixed PT amount

            return pfDeduction + esiDeduction + professionalTax;
        }
    }
}
