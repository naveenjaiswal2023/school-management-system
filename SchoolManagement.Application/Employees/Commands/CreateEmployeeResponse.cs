using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Employees.Commands
{
    public class CreateEmployeeResponse
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

}
