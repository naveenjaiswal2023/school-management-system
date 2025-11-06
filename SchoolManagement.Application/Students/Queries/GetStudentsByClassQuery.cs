using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Students.Queries
{
    public class GetStudentsByClassQuery : IRequest<IEnumerable<StudentDto>>
    {
        public Guid ClassId { get; set; }
    }
}
