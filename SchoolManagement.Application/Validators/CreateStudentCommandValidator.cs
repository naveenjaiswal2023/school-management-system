using FluentValidation;
using SchoolManagement.Application.Students.Commands;
using System;

namespace SchoolManagement.Application.Validators
{
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            // Name validations
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).WithMessage("Middle name must not exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.MiddleName));

            // Email validation
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

            // Phone validation
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            // Date of Birth validation
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Today.AddYears(-3)).WithMessage("Student must be at least 3 years old")
                .GreaterThan(DateTime.Today.AddYears(-25)).WithMessage("Student cannot be older than 25 years");

            // Gender validation
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value");

            // Class and Section validation
            RuleFor(x => x.ClassId)
                .NotEmpty().WithMessage("Class selection is required");

            RuleFor(x => x.SectionId)
                .NotEmpty().WithMessage("Section selection is required");

            // Address validation
            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required")
                .SetValidator(new AddressDtoValidator());

            // Admission Number validation
            RuleFor(x => x.AdmissionNumber)
                .NotEmpty().WithMessage("Admission number is required")
                .MaximumLength(20).WithMessage("Admission number must not exceed 20 characters");

            // Admission Date validation
            RuleFor(x => x.AdmissionDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Admission date cannot be in the future");

            // Status validation
            RuleFor(x => x.Status)
                .InclusiveBetween(0, 1).WithMessage("Invalid status value"); // 0 = Inactive, 1 = Active

            // Optional: PhotoUrl validation
            RuleFor(x => x.PhotoUrl)
                .MaximumLength(250).WithMessage("Photo URL must not exceed 250 characters")
                .When(x => !string.IsNullOrEmpty(x.PhotoUrl));

        }
    }
}
