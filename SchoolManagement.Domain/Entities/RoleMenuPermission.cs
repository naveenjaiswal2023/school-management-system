using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class RoleMenuPermission : BaseEntity
    {
        public Guid RoleId { get; private set; }
        public Guid MenuId { get; private set; }
        public bool CanView { get; private set; }
        public bool CanAdd { get; private set; }
        public bool CanEdit { get; private set; }
        public bool CanDelete { get; private set; }
        public bool CanExport { get; private set; }
        public bool CanPrint { get; private set; }
        public bool CanApprove { get; private set; }
        public bool CanReject { get; private set; }

        // Navigation Properties
        public virtual Role Role { get; private set; }
        public virtual Menu Menu { get; private set; }

        private RoleMenuPermission() { }

        public RoleMenuPermission(Guid roleId, Guid menuId, MenuPermissions permissions)
        {
            RoleId = roleId;
            MenuId = menuId;
            SetPermissions(permissions);
        }

        public void SetPermissions(MenuPermissions permissions)
        {
            CanView = permissions.CanView;
            CanAdd = permissions.CanAdd;
            CanEdit = permissions.CanEdit;
            CanDelete = permissions.CanDelete;
            CanExport = permissions.CanExport;
            CanPrint = permissions.CanPrint;
            CanApprove = permissions.CanApprove;
            CanReject = permissions.CanReject;
        }

        public MenuPermissions GetPermissions()
        {
            return new MenuPermissions
            {
                CanView = CanView,
                CanAdd = CanAdd,
                CanEdit = CanEdit,
                CanDelete = CanDelete,
                CanExport = CanExport,
                CanPrint = CanPrint,
                CanApprove = CanApprove,
                CanReject = CanReject
            };
        }
    }
}
