using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Auth.Queries
{
    public class GetUserProfileQuery : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }
}
