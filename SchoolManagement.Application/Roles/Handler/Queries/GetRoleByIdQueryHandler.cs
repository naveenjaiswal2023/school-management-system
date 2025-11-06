using MediatR;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Roles.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Roles.Handler.Queries
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(request.Id);

            if (role == null) return null;

            // Map entity to DTO
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                DisplayName = role.DisplayName,
                Description = role.Description,
                IsSystemRole = role.IsSystemRole,
                IsActive = role.IsActive,
                Level = role.Level,
                //UserCount = await _unitOfWork.UserRoleRepository.CountUsersByRoleIdAsync(role.Id) // example if you track user count
            };
        }
    }
}
