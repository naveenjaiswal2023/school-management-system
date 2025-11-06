using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class RoleMenuPermissionDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }
        public MenuPermissions Permissions { get; set; }
    }
}
