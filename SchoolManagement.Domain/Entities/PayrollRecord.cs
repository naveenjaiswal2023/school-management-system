using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class PayrollRecord : BaseEntity
    {
        public Guid EmployeeId { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public decimal BasicSalary { get; private set; }
        public decimal GrossSalary { get; private set; }
        public decimal NetSalary { get; private set; }
        public decimal TotalDeductions { get; private set; }
        public decimal TotalAllowances { get; private set; }
        public int WorkingDays { get; private set; }
        public int PresentDays { get; private set; }
        public decimal LossOfPayAmount { get; private set; }
        public PayrollStatus Status { get; private set; }
        public DateTime? ProcessedDate { get; private set; }

        // Navigation Properties
        public virtual Employee Employee { get; private set; }

        private PayrollRecord() { }

        public PayrollRecord(Guid employeeId, int month, int year, decimal basicSalary,
                           int workingDays, int presentDays)
        {
            EmployeeId = employeeId;
            Month = month;
            Year = year;
            BasicSalary = basicSalary;
            WorkingDays = workingDays;
            PresentDays = presentDays;
            Status = PayrollStatus.Draft;
        }

        public void ProcessPayroll(decimal grossSalary, decimal netSalary, decimal totalDeductions,
                                 decimal totalAllowances, decimal lossOfPayAmount)
        {
            GrossSalary = grossSalary;
            NetSalary = netSalary;
            TotalDeductions = totalDeductions;
            TotalAllowances = totalAllowances;
            LossOfPayAmount = lossOfPayAmount;
            Status = PayrollStatus.Processed;
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
