using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.UserRoles.Commands
{
    public class RevokeRoleFromUserCommand : IRequest<RevokeRoleFromUserResponse>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
