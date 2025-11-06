using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string PasswordHash { get; private set; }
        public string? PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
        public bool EmailVerified { get; private set; }
        public bool PhoneVerified { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public int LoginAttempts { get; private set; }
        public DateTime? LockedUntil { get; private set; }
        public UserType UserType { get; private set; }
        public Guid? StudentId { get; private set; }
        public Guid? EmployeeId { get; private set; }
        public List<RefreshToken> RefreshTokens { get; private set; }

        // Navigation Properties
        public virtual Student Student { get; private set; }
        public virtual Employee Employee { get; private set; }
        public virtual ICollection<UserRole> UserRoles { get; private set; }

        // Parameterless constructor for EF Core
        private User()
        {
            UserRoles = new List<UserRole>();
        }

        // Main constructor
        public User(string username, string email, string firstName, string lastName,
                   string passwordHash, UserType userType)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            UserType = userType;
            IsActive = true;
            EmailVerified = false;
            PhoneVerified = false;
            LoginAttempts = 0;
            UserRoles = new List<UserRole>();
            RefreshTokens = new List<RefreshToken>();
        }

        // Methods to modify properties
        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be null or empty", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
        }

        public void UpdateProfile(string firstName, string lastName, string email)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void VerifyEmail()
        {
            EmailVerified = true;
        }

        public void VerifyPhone()
        {
            PhoneVerified = true;
        }

        public void LinkToStudent(Guid studentId)
        {
            if (UserType != UserType.Student && UserType != UserType.Parent)
                throw new InvalidOperationException("Only student or parent users can be linked to students");

            StudentId = studentId;
        }

        public void LinkToEmployee(Guid employeeId)
        {
            if (UserType != UserType.Staff)
                throw new InvalidOperationException("Only staff users can be linked to employees");

            EmployeeId = employeeId;
        }

        public void RecordLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            LoginAttempts = 0;
            LockedUntil = null;
        }

        public void RecordFailedLogin(int maxAttempts, TimeSpan lockoutDuration)
        {
            LoginAttempts++;
            if (LoginAttempts >= maxAttempts)
            {
                LockedUntil = DateTime.UtcNow.Add(lockoutDuration);
            }
        }

        public bool IsLockedOut()
        {
            return LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;
        }

        public void UnlockAccount()
        {
            LockedUntil = null;
            LoginAttempts = 0;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void AddRefreshToken(RefreshToken token)
        {
            RefreshTokens.Add(token);
        }

        public void RemoveRefreshToken(RefreshToken token)
        {
            RefreshTokens.Remove(token);
        }
    }
}