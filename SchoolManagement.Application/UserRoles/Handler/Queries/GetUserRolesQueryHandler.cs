using MediatR;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.UserRoles.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Application.UserRoles.Handlers
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<UserRoleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserRolesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var userRoles = await _unitOfWork.UserRoleRepository.GetAllAsync();

            // Map entities to DTO
            var result = userRoles.Select(ur => new UserRoleDto
            {
                UserId = ur.UserId,
                Username = ur.User?.Username ?? string.Empty,
                FullName = ur.User != null ? $"{ur.User.FirstName} {ur.User.LastName}" : string.Empty,
                RoleId = ur.RoleId,
                RoleName = ur.Role?.Name ?? string.Empty,
                AssignedAt = ur.AssignedAt,
                ExpiresAt = ur.ExpiresAt,
                IsActive = ur.IsActive,
                IsExpired = ur.ExpiresAt.HasValue && ur.ExpiresAt.Value < DateTime.UtcNow
            });

            return result;
        }
    }
}
