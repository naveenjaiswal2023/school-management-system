using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Commands
{
    public class AssignMenuPermissionsCommand : IRequest<AssignMenuPermissionsResponse>
    {
        public Guid RoleId { get; set; }
        public Dictionary<Guid, MenuPermissionsDto> MenuPermissions { get; set; }
    }
}
