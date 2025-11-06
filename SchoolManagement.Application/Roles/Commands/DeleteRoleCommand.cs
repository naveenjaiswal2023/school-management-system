using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Roles.Commands
{
    public class DeleteRoleCommand : IRequest<DeleteRoleResponse>
    {
        public Guid Id { get; set; }
    }
}
