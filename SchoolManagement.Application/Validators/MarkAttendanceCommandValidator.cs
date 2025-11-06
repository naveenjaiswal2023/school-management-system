using FluentValidation;
using SchoolManagement.Application.Attendance.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Validators
{
    public class MarkAttendanceCommandValidator : AbstractValidator<MarkAttendanceCommand>
    {
        public MarkAttendanceCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required");

            RuleFor(x => x.BiometricData)
                .NotEmpty().WithMessage("Biometric data is required")
                .MinimumLength(10).WithMessage("Invalid biometric data");

            RuleFor(x => x.DeviceId)
                .NotEmpty().WithMessage("Device ID is required")
                .MaximumLength(50).WithMessage("Device ID must not exceed 50 characters");

            RuleFor(x => x.Timestamp)
                .NotEmpty().WithMessage("Timestamp is required")
                .GreaterThan(DateTime.Today.AddDays(-1)).WithMessage("Attendance can only be marked for today or yesterday")
                .LessThan(DateTime.UtcNow.AddMinutes(5)).WithMessage("Future timestamps not allowed");

            RuleFor(x => x.BiometricType)
                .IsInEnum().WithMessage("Invalid biometric type");
        }
    }
}
