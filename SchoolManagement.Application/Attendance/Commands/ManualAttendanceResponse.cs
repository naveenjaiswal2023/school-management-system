using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Commands
{
    public class ManualAttendanceResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public Guid AttendanceId { get; set; }
    }
}
