using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Commands
{
    public class MarkAttendanceCommand : IRequest<MarkAttendanceResponse>
    {
        public Guid StudentId { get; set; }
        public string BiometricData { get; set; }
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public int BiometricType { get; set; }
    }
}
