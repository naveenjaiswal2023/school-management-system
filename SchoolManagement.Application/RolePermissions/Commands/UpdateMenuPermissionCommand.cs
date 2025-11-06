using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.RolePermissions.Commands
{
    public class UpdateMenuPermissionCommand : IRequest<UpdateMenuPermissionResponse>
    {
        public Guid RoleId { get; set; }
        public Guid MenuId { get; set; }
        public MenuPermissionsDto Permissions { get; set; }
    }
}
