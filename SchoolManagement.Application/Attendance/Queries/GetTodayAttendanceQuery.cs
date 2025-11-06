using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Queries
{
    public class GetTodayAttendanceQuery : IRequest<AttendanceDto>
    {
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
    }
}
