using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation Properties
        public virtual User User { get; private set; }
        public virtual Role Role { get; private set; }

        private UserRole() { }

        public UserRole(Guid userId, Guid roleId, DateTime assignAt,bool isActive, DateTime? expiresAt = null)
        {
            UserId = userId;
            RoleId = roleId;
            AssignedAt = (DateTime)assignAt;
            ExpiresAt = expiresAt;
            IsActive = isActive;
        }

        public void DeactivateRole()
        {
            IsActive = false;
        }

        public void ExtendRole(DateTime newExpiryDate)
        {
            ExpiresAt = newExpiryDate;
        }

        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }
    }
}
