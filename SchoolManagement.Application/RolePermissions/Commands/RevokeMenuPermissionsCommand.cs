using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.RolePermissions.Commands
{
    public class RevokeMenuPermissionsCommand : IRequest<RevokeMenuPermissionsResponse>
    {
        public Guid RoleId { get; set; }
        public List<Guid> MenuIds { get; set; }
    }
}
