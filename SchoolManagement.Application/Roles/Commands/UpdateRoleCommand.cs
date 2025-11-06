using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Roles.Commands
{
    public class UpdateRoleCommand : IRequest<UpdateRoleResponse>
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public bool IsActive { get; set; }
    }
}
