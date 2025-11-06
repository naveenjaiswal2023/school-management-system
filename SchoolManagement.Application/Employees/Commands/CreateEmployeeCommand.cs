using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<CreateEmployeeResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid DesignationId { get; set; }
        public int EmploymentType { get; set; }
        public AddressDto Address { get; set; }
    }
}
