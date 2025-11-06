using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.RolePermissions.Commands
{
    public class RevokeMenuPermissionsResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int PermissionsRevoked { get; set; }
    }
}
