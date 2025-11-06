using MediatR;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Roles.Queries
{
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        public Guid Id { get; set; }

        public GetRoleByIdQuery(Guid roleId)
        {
            Id = roleId;
        }
    }
}
