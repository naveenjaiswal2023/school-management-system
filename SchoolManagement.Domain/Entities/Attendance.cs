using SchoolManagement.Domain.Enums;
using System;

namespace SchoolManagement.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan CheckInTime { get; private set; } = TimeSpan.Zero;
        public TimeSpan? CheckOutTime { get; private set; }
        public AttendanceStatus Status { get; private set; }
        public AttendanceMode Mode { get; private set; }
        public string DeviceId { get; private set; }
        public string Remarks { get; private set; }

        // Navigation Properties
        public virtual Student Student { get; private set; }

        private Attendance() { } // EF Core needs this

        public Attendance(Guid studentId, DateTime date, TimeSpan checkInTime,
                          AttendanceMode mode, string deviceId = null)
        {
            StudentId = studentId;
            Date = date.Date;
            CheckInTime = checkInTime;
            Mode = mode;
            DeviceId = deviceId;
            Status = AttendanceStatus.Present;
        }

        public void MarkCheckOut(TimeSpan checkOutTime)
        {
            CheckOutTime = checkOutTime;
        }

        public void UpdateStatus(AttendanceStatus status, string remarks = null)
        {
            Status = status;
            Remarks = remarks;
        }
    }
}
