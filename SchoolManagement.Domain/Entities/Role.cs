using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string? Name { get; private set; }
        public string? DisplayName { get; private set; }
        public string? Description { get; private set; }
        public bool IsSystemRole { get; private set; }
        public bool IsActive { get; private set; }
        public int Level { get; private set; } // Hierarchy level

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; private set; }
        public virtual ICollection<RoleMenuPermission> RoleMenuPermissions { get; private set; }
        public virtual ICollection<RolePermission> RolePermissions { get; private set; }

        private Role()
        {
            UserRoles = new List<UserRole>();
            RoleMenuPermissions = new List<RoleMenuPermission>();
            RolePermissions = new List<RolePermission>();
        }

        public Role(string name, string displayName, string description,
                   bool isSystemRole = false, int level = 0)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Description = description;
            IsSystemRole = isSystemRole;
            Level = level;
            IsActive = true;

            UserRoles = new List<UserRole>();
            RoleMenuPermissions = new List<RoleMenuPermission>();
            RolePermissions = new List<RolePermission>();
        }

        public void UpdateDetails(string displayName, string description, int level)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Description = description;
            Level = level;
        }

        public void SetActiveStatus(bool isActive)
        {
            if (IsSystemRole && !isActive)
            {
                throw new InvalidOperationException("System roles cannot be deactivated");
            }
            IsActive = isActive;
        }
    }
}
