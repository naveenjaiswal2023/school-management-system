using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Queries
{
    public class GetAttendanceStatisticsQuery : IRequest<AttendanceStatisticsDto>
    {
        public Guid StudentId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
