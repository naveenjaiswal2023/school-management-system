using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace SchoolManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string EmployeeId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public Address Address { get; private set; }   // Value Object
        public DateTime JoiningDate { get; private set; }
        public Guid DepartmentId { get; private set; }
        public Guid DesignationId { get; private set; }
        public EmployeeStatus Status { get; private set; }
        public EmploymentType EmploymentType { get; private set; }
        public Salary SalaryInfo { get; private set; }  // Value Object
        public string PhotoUrl { get; private set; }
        public BiometricInfo BiometricInfo { get; private set; }  // Value Object

        // Navigation Properties
        public virtual Department Department { get; private set; }
        public virtual Designation Designation { get; private set; }
        public virtual ICollection<EmployeeAttendance> Attendances { get; private set; }
        public virtual ICollection<LeaveApplication> LeaveApplications { get; private set; }
        public virtual ICollection<PerformanceReview> PerformanceReviews { get; private set; }
        public virtual ICollection<PayrollRecord> PayrollRecords { get; private set; }

        private Employee()
        {
            // EF Core constructor – initialize collections
            Attendances = new List<EmployeeAttendance>();
            LeaveApplications = new List<LeaveApplication>();
            PerformanceReviews = new List<PerformanceReview>();
            PayrollRecords = new List<PayrollRecord>();
        }

        public Employee(string firstName, string lastName, string email,
                        DateTime dateOfBirth, Gender gender, Guid departmentId,
                        Guid designationId, EmploymentType employmentType)
            : this() // call private ctor to init collections
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            DateOfBirth = dateOfBirth;
            Gender = gender;
            DepartmentId = departmentId;
            DesignationId = designationId;
            EmploymentType = employmentType;
            JoiningDate = DateTime.UtcNow;
            Status = EmployeeStatus.Active;
            EmployeeId = GenerateEmployeeId();
        }

        public void UpdatePersonalInfo(string firstName, string lastName, string middleName,
                                       string phone, Address address)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            MiddleName = middleName;
            Phone = phone;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void UpdateJobInfo(Guid departmentId, Guid designationId, EmploymentType employmentType)
        {
            DepartmentId = departmentId;
            DesignationId = designationId;
            EmploymentType = employmentType;
        }

        public void UpdateSalary(Salary salaryInfo)
        {
            SalaryInfo = salaryInfo ?? throw new ArgumentNullException(nameof(salaryInfo));
        }

        public void EnrollBiometric(BiometricInfo biometricInfo)
        {
            BiometricInfo = biometricInfo ?? throw new ArgumentNullException(nameof(biometricInfo));
        }

        private string GenerateEmployeeId()
        {
            return $"EMP{DateTime.UtcNow.Year}{DateTime.UtcNow.Month:D2}{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }
    }
}
