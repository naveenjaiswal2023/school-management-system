using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using static System.Collections.Specialized.BitVector32;

namespace SchoolManagement.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string StudentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Address Address { get; private set; }
        public Guid ClassId { get; private set; }
        public Guid SectionId { get; private set; }
        public string AdmissionNumber { get; private set; }
        public DateTime AdmissionDate { get; private set; }
        public StudentStatus Status { get; private set; }
        public string PhotoUrl { get; private set; }
        public BiometricInfo BiometricInfo { get; private set; }

        // Navigation Properties
        public virtual Class Class { get; private set; }
        public virtual Section Section { get; private set; }
        public virtual ICollection<StudentParent> StudentParents { get; private set; }
        public virtual ICollection<Attendance> Attendances { get; private set; }
        public virtual ICollection<FeePayment> FeePayments { get; private set; }
        public virtual ICollection<ExamResult> ExamResults { get; private set; }

        private Student() { } // EF Constructor

        public Student(
        string firstName,
        string lastName,
        string email,
        DateTime dateOfBirth,
        Gender gender,
        Guid classId,
        Guid sectionId,
        string admissionNumber = null,
        DateTime? admissionDate = null,
        StudentStatus status = StudentStatus.Active,
        string photoUrl = null)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            DateOfBirth = dateOfBirth;
            Gender = gender;
            ClassId = classId;
            SectionId = sectionId;

            // Required DB fields
            AdmissionNumber = admissionNumber ?? GenerateAdmissionNumber();
            AdmissionDate = admissionDate ?? DateTime.UtcNow;
            Status = status;
            PhotoUrl = photoUrl;

            StudentId = GenerateStudentId();

            // Initialize navigation collections
            StudentParents = new List<StudentParent>();
            Attendances = new List<Attendance>();
            FeePayments = new List<FeePayment>();
            ExamResults = new List<ExamResult>();
        }

        public void UpdatePersonalInfo(string firstName, string lastName, string middleName,
                                     string phone, Address address)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            MiddleName = middleName;
            Phone = phone;
            Address = address;
        }

        public void TransferToClass(Guid classId, Guid sectionId)
        {
            ClassId = classId;
            SectionId = sectionId;
        }

        public void EnrollBiometric(BiometricInfo biometricInfo)
        {
            BiometricInfo = biometricInfo ?? throw new ArgumentNullException(nameof(biometricInfo));
        }

        public void UpdateStatus(StudentStatus status)
        {
            Status = status;
        }

        private string GenerateStudentId()
        {
            return $"STD{DateTime.UtcNow.Year}{DateTime.UtcNow.Month:D2}{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }

        private string GenerateAdmissionNumber()
        {
            return $"ADM{DateTime.UtcNow:yyyyMMddHHmmss}";
        }
    }
}
