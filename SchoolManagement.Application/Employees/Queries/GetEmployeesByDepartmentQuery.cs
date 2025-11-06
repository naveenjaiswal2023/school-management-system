using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Employees.Queries
{
    public class GetEmployeesByDepartmentQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public Guid DepartmentId { get; set; }
    }
}
