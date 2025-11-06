using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class EmployeeAttendance : BaseEntity
    {
        public Guid EmployeeId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan CheckInTime { get; private set; }
        public TimeSpan? CheckOutTime { get; private set; }
        public AttendanceStatus Status { get; private set; }
        public AttendanceMode Mode { get; private set; }
        public string DeviceId { get; private set; }
        public decimal RegularHours { get; private set; }
        public decimal OvertimeHours { get; private set; }
        public string Remarks { get; private set; }

        // Navigation Properties
        public virtual Employee Employee { get; private set; }

        private EmployeeAttendance() { } // EF Constructor

        public EmployeeAttendance(Guid employeeId, DateTime date, TimeSpan checkInTime,
                                AttendanceMode mode, string deviceId = null)
        {
            EmployeeId = employeeId;
            Date = date.Date;
            CheckInTime = checkInTime;
            Mode = mode;
            DeviceId = deviceId;
            Status = AttendanceStatus.Present;
        }

        public void MarkCheckOut(TimeSpan checkOutTime)
        {
            CheckOutTime = checkOutTime;
            CalculateWorkingHours();
        }

        private void CalculateWorkingHours()
        {
            if (CheckOutTime.HasValue)
            {
                var totalMinutes = (CheckOutTime.Value - CheckInTime).TotalMinutes;
                var regularMinutes = Math.Min(totalMinutes, 480); // 8 hours
                var overtimeMinutes = Math.Max(0, totalMinutes - 480);

                RegularHours = (decimal)(regularMinutes / 60);
                OvertimeHours = (decimal)(overtimeMinutes / 60);
            }
        }
    }
}
