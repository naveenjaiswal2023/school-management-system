using Entities = SchoolManagement.Domain.Entities;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Entities.Attendance>> GetAllAsync();
        Task<Entities.Attendance> CreateAsync(Entities.Attendance attendance);
        Task<IEnumerable<Entities.Attendance>> GetStudentAttendanceAsync(Guid studentId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Entities.Attendance>> GetClassAttendanceAsync(Guid classId, DateTime date);
        Task<Entities.Attendance> GetTodayAttendanceAsync(Guid studentId, DateTime date);
        Task<AttendanceStatistics> GetAttendanceStatisticsAsync(Guid studentId, DateTime fromDate, DateTime toDate);
    }
}