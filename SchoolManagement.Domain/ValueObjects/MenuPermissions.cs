using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.ValueObjects
{
    public class MenuPermissions
    {
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanExport { get; set; }
        public bool CanPrint { get; set; }
        public bool CanApprove { get; set; }
        public bool CanReject { get; set; }

        public static MenuPermissions ViewOnly()
        {
            return new MenuPermissions { CanView = true };
        }

        public static MenuPermissions FullAccess()
        {
            return new MenuPermissions
            {
                CanView = true,
                CanAdd = true,
                CanEdit = true,
                CanDelete = true,
                CanExport = true,
                CanPrint = true,
                CanApprove = true,
                CanReject = true
            };
        }

        public static MenuPermissions ReadWrite()
        {
            return new MenuPermissions
            {
                CanView = true,
                CanAdd = true,
                CanEdit = true,
                CanExport = true,
                CanPrint = true
            };
        }
    }
}
