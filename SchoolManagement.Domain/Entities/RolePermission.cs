using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; private set; }
        public Guid PermissionId { get; private set; }
        public bool IsGranted { get; private set; }

        // Navigation Properties
        public virtual Role Role { get; private set; }
        public virtual Permission Permission { get; private set; }

        private RolePermission() { }

        public RolePermission(Guid roleId, Guid permissionId, bool isGranted = true)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            IsGranted = isGranted;
        }

        public void GrantPermission()
        {
            IsGranted = true;
        }

        public void RevokePermission()
        {
            IsGranted = false;
        }
    }
}
