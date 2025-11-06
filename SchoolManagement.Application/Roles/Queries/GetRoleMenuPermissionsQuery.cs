using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Roles.Queries
{
    public class GetRoleMenuPermissionsQuery : IRequest<IEnumerable<RoleMenuPermissionDto>>
    {
        public Guid RoleId { get; set; }

        public GetRoleMenuPermissionsQuery(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}
