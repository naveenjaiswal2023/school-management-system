using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Employees.Commands
{
    public class OnboardEmployeeCommand : IRequest<OnboardEmployeeResponse>
    {
        public Guid EmployeeId { get; set; }
        public SalaryDto SalaryInfo { get; set; }
        public string BiometricData { get; set; }
        public int BiometricType { get; set; }
    }
}
