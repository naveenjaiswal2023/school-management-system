using MediatR;
using SchoolManagement.Application.DTOs;
using System;

namespace SchoolManagement.Application.Students.Commands
{
    public class CreateStudentCommand : IRequest<CreateStudentResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; } // 0 = Male, 1 = Female, etc.
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }

        // Additional fields based on your table
        public string AdmissionNumber { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } = 1; // e.g., Active = 1, Inactive = 0
        public string PhotoUrl { get; set; }
        
        // Optional: keep Address if you store as JSON or separate table
        public AddressDto Address { get; set; }
    }
}
