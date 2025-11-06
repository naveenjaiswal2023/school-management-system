using MediatR;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Students.Commands;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Students.Handler.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public CreateStudentCommandHandler(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork,
            INotificationService notificationService)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<CreateStudentResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Create Address value object
                var address = new Address(
                    request.Address.Street,
                    request.Address.City,
                    request.Address.State,
                    request.Address.Country,
                    request.Address.ZipCode);

                // Generate AdmissionNumber if not provided
                var admissionNumber = string.IsNullOrEmpty(request.AdmissionNumber)
                    ? $"ADM{DateTime.UtcNow:yyyyMMddHHmmss}"
                    : request.AdmissionNumber;

                // Create Student entity using constructor that allows required fields
                var student = new Student(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.DateOfBirth,
                    (Gender)request.Gender,
                    request.ClassId,
                    request.SectionId,
                    admissionNumber,                       // AdmissionNumber via constructor
                    request.AdmissionDate == default ? DateTime.UtcNow : request.AdmissionDate,
                    (StudentStatus)request.Status,                        // Status via constructor
                    request.PhotoUrl                        // PhotoUrl via constructor
                    );

                // Update optional details via methods
                student.UpdatePersonalInfo(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    middleName: request.MiddleName,
                    phone: request.Phone,
                    address: address
                );

                // Save
                var createdStudent = await _studentRepository.CreateAsync(student);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                // Send notification
                //if (!string.IsNullOrEmpty(request.Phone))
                //{
                //    await _notificationService.SendSMSAsync(request.Phone,
                //        $"Welcome to our school! Student ID: {createdStudent.StudentId}");
                //}

                return new CreateStudentResponse
                {
                    Id = createdStudent.Id,
                    StudentId = createdStudent.StudentId,
                    Message = "Student created successfully",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new CreateStudentResponse
                {
                    Message = $"Error creating student: {ex.Message}",
                    Success = false
                };
            }
        }
    }
}
