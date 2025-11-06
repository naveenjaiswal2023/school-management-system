using Microsoft.Extensions.Options;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.Services;
using SchoolManagement.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class AttendanceCalculationService : IAttendanceCalculationService
    {
        private readonly AttendanceSettings _settings;

        public AttendanceCalculationService(IOptions<AttendanceSettings> settings)
        {
            _settings = settings.Value;
        }

        public AttendanceStatus CalculateStatus(TimeSpan checkInTime, TimeSpan? checkOutTime, TimeSpan standardTime)
        {
            if (checkInTime <= standardTime)
                return AttendanceStatus.Present;

            if (checkInTime <= standardTime.Add(_settings.LateArrivalGracePeriod))
                return AttendanceStatus.Late;

            if (checkOutTime.HasValue &&
                checkOutTime.Value.Subtract(checkInTime) >= _settings.MinimumHalfDayDuration)
                return AttendanceStatus.HalfDay;

            return AttendanceStatus.Absent;
        }

        public decimal CalculateAttendancePercentage(IEnumerable<Attendance> attendances, int workingDays)
        {
            if (workingDays == 0) return 0;

            var presentDays = attendances.Count(a => a.Status == AttendanceStatus.Present || a.Status == AttendanceStatus.Late);
            var halfDays = attendances.Count(a => a.Status == AttendanceStatus.HalfDay);

            return ((presentDays + (halfDays * 0.5m)) / workingDays) * 100;
        }

        public OvertimeCalculation CalculateOvertime(TimeSpan checkInTime, TimeSpan checkOutTime, TimeSpan standardHours)
        {
            var totalHours = (checkOutTime - checkInTime).TotalHours;
            var regularHours = Math.Min(totalHours, standardHours.TotalHours);
            var overtimeHours = Math.Max(0, totalHours - standardHours.TotalHours);

            return new OvertimeCalculation
            {
                RegularHours = (decimal)regularHours,
                OvertimeHours = (decimal)overtimeHours,
                OvertimeRate = _settings.OvertimeRate,
                OvertimeAmount = (decimal)overtimeHours * _settings.OvertimeRate
            };
        }
    }
}
