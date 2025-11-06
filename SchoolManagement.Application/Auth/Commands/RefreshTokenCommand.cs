using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public string RefreshToken { get; set; }
    }
}
