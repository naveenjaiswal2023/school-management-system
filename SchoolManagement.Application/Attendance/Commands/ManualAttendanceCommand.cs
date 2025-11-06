using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Attendance.Commands
{
    public class ManualAttendanceCommand : IRequest<ManualAttendanceResponse>
    {
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public Guid MarkedBy { get; set; }
    }
}
