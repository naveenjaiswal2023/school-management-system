using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Employees.Queries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
    }
}
