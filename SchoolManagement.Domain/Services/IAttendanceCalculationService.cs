using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Services
{
    public interface IAttendanceCalculationService
    {
        AttendanceStatus CalculateStatus(TimeSpan checkInTime, TimeSpan? checkOutTime, TimeSpan standardTime);
        decimal CalculateAttendancePercentage(IEnumerable<Attendance> attendances, int workingDays);
        OvertimeCalculation CalculateOvertime(TimeSpan checkInTime, TimeSpan checkOutTime, TimeSpan standardHours);
    }
}
