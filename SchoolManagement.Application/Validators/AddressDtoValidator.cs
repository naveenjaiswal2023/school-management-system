using FluentValidation;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street address is required")
                .MaximumLength(200).WithMessage("Street address must not exceed 200 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(50).WithMessage("City must not exceed 50 characters");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required")
                .Matches(@"^\d{5,10}$").WithMessage("Invalid zip code format");
        }
    }
}
