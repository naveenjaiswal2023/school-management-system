using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Queries
{
    public class GetClassAttendanceQuery : IRequest<IEnumerable<AttendanceDto>>
    {
        public Guid ClassId { get; set; }
        public DateTime Date { get; set; }
    }
}
