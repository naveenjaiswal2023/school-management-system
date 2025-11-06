using MediatR;
using SchoolManagement.Application.Attendance.Commands;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Students.Commands;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Features.Attendance.Handlers.Commands
{
    public class MarkAttendanceCommandHandler : IRequestHandler<MarkAttendanceCommand, MarkAttendanceResponse>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IBiometricVerificationService _biometricService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public MarkAttendanceCommandHandler(
            IAttendanceRepository attendanceRepository,
            IStudentRepository studentRepository,
            IBiometricVerificationService biometricService,
            IUnitOfWork unitOfWork,
            INotificationService notificationService)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _biometricService = biometricService;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<MarkAttendanceResponse> Handle(MarkAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verify student exists
                var student = await _studentRepository.GetByIdAsync(request.StudentId);
                if (student == null)
                {
                    return new MarkAttendanceResponse
                    {
                        Success = false,
                        Message = "Student not found"
                    };
                }

                // Verify biometric data
                var verificationResult = await _biometricService.VerifyAsync(
                    request.BiometricData, (BiometricType)request.BiometricType);

                if (!verificationResult.IsVerified)
                {
                    return new MarkAttendanceResponse
                    {
                        Success = false,
                        Message = "Biometric verification failed"
                    };
                }

                // Check if already marked for today
                var existingAttendance = await _attendanceRepository.GetTodayAttendanceAsync(
                    request.StudentId, request.Timestamp.Date);

                if (existingAttendance != null)
                {
                    return new MarkAttendanceResponse
                    {
                        Success = false,
                        Message = "Attendance already marked for today"
                    };
                }

                // Create attendance record using the namespace alias
                var attendance = new Entities.Attendance(
                    request.StudentId,
                    request.Timestamp.Date,
                    request.Timestamp.TimeOfDay,
                    AttendanceMode.Biometric,
                    request.DeviceId);

                var createdAttendance = await _attendanceRepository.CreateAsync(attendance);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Send notification
                await _notificationService.SendSMSAsync(student.Phone,
                    $"{student.FirstName} has arrived at school at {request.Timestamp:HH:mm}");

                return new MarkAttendanceResponse
                {
                    Success = true,
                    Message = "Attendance marked successfully",
                    AttendanceId = createdAttendance.Id,
                    CheckInTime = request.Timestamp.TimeOfDay
                };
            }
            catch (Exception ex)
            {
                return new MarkAttendanceResponse
                {
                    Success = false,
                    Message = $"Error marking attendance: {ex.Message}"
                };
            }
        }
    }
}