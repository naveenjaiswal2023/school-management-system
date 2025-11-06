using FluentValidation;
using SchoolManagement.Application.Employees.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Validators
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Today.AddYears(-18)).WithMessage("Employee must be at least 18 years old")
                .GreaterThan(DateTime.Today.AddYears(-70)).WithMessage("Employee cannot be older than 70 years");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department selection is required");

            RuleFor(x => x.DesignationId)
                .NotEmpty().WithMessage("Designation selection is required");

            RuleFor(x => x.EmploymentType)
                .IsInEnum().WithMessage("Invalid employment type");
        }
    }
}
