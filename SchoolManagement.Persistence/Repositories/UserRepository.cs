using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserRoleEntity = SchoolManagement.Domain.Entities.UserRole;
using UserRoleEnum = SchoolManagement.Domain.Enums.UserRole;


namespace SchoolManagement.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolManagementDbContext _context;

        public UserRepository(SchoolManagementDbContext context)
        {
            _context = context;
        }

        // ✅ Always load user with tracking and roles
        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return user;
        }

        /// <summary>
        /// Assigns a role to a user (tracked safely).
        /// </summary>
        public async Task AssignRoleAsync(Guid userId, Guid roleId, DateTime assignAt, bool isActive, DateTime? expiresAt = null)
        {
            // ✅ Load user with roles, tracked
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} not found.");

            var existingUserRole = user.UserRoles
                .FirstOrDefault(ur => ur.RoleId == roleId && !ur.IsDeleted);

            if (existingUserRole != null)
            {
                if (expiresAt.HasValue)
                    existingUserRole.ExtendRole(expiresAt.Value);
            }
            else
            {
                // ✅ Add role via navigation property (ensures tracking)
                var userRole = new UserRoleEntity(userId, roleId, DateTime.UtcNow, isActive, expiresAt);
                

                user.UserRoles.Add(userRole);
            }
        }

        public async Task RevokeRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} not found.");

            var userRole = user.UserRoles
                .FirstOrDefault(ur => ur.RoleId == roleId && !ur.IsDeleted);

            if (userRole != null)
            {
                userRole.DeactivateRole();
            }
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive && !ur.IsExpired() && !ur.IsDeleted)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            user.MarkAsDeleted(); // Ensure your entity has this method
            _context.Users.Update(user);

            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByUserTypeAsync(UserType userType)
        {
            return await _context.Users
                .Where(u => u.UserType == userType && !u.IsDeleted)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }
    }
}
