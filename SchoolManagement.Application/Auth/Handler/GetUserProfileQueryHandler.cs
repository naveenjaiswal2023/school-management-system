using MediatR;
using SchoolManagement.Application.Auth.Queries;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Auth.Handler
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.AuthRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException("User not found");

            string roleName = Enum.IsDefined(typeof(UserType), user.UserType)
            ? ((UserType)user.UserType).ToString()
            : "Unknown";

            return new UserDto
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = new List<string> { roleName }
            };
        }
    }
}
