using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public class OfflineAttendanceRecord
    {
        public Guid Id { get; set; }
        public string DeviceId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Timestamp { get; set; }
        public string BiometricData { get; set; }
        public AttendanceMode Mode { get; set; }
    }
}
