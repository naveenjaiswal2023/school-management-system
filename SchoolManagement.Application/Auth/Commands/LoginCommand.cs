using MediatR;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Auth.Commands
{
    public class LoginCommand : IRequest<Result<AuthResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
