using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.UserRoles.Commands;
using SchoolManagement.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Application.UserRoles.Handler.Commands
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, AssignRoleToUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignRoleToUserCommandHandler> _logger;
        private const int MaxRetryCount = 3;

        public AssignRoleToUserCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignRoleToUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AssignRoleToUserResponse> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var retryCount = 0;

            while (retryCount < MaxRetryCount)
            {
                try
                {
                    // Load the user
                    var user = await _userRepository.GetByIdAsync(request.UserId);
                    if (user == null)
                    {
                        return new AssignRoleToUserResponse { Success = false, Message = "User not found" };
                    }

                    // Load the role
                    var role = await _roleRepository.GetByIdAsync(request.RoleId);
                    if (role == null)
                    {
                        return new AssignRoleToUserResponse { Success = false, Message = "Role not found" };
                    }

                    // Check if user already has active role
                    var existingUserRole = await _unitOfWork.UserRoleRepository
                        .FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId && ur.IsActive, cancellationToken);

                    if (existingUserRole != null)
                    {
                        return new AssignRoleToUserResponse { Success = false, Message = "User already has the active role" };
                    }

                    // Add new UserRole
                    var userRole = new UserRole(user.Id, role.Id, DateTime.UtcNow, true, request.ExpiresAt);
                    _unitOfWork.UserRoleRepository.Add(userRole);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new AssignRoleToUserResponse { Success = true, Message = "Role assigned successfully" };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryCount++;
                    _logger.LogWarning(ex, "Concurrency conflict assigning role to user {UserId}. Retry {RetryCount}/{MaxRetry}", request.UserId, retryCount, MaxRetryCount);

                    if (retryCount >= MaxRetryCount)
                    {
                        return new AssignRoleToUserResponse { Success = false, Message = "Concurrency conflict, please retry." };
                    }

                    // Reload the tracked entities to get latest values
                    foreach (var entry in ex.Entries)
                    {
                        await entry.ReloadAsync(cancellationToken);
                    }
                }
            }

            return new AssignRoleToUserResponse { Success = false, Message = "Failed to assign role due to concurrency issues." };
        }
    }
}
