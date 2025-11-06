using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Commands
{
    public class MarkAttendanceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid AttendanceId { get; set; }
        public TimeSpan CheckInTime { get; set; }
    }
}
