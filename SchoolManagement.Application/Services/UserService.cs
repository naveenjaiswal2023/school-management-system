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

namespace SchoolManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IAuthRepository authRepository, IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<User> GetByIdAsync(Guid id) => await _authRepository.GetByIdAsync(id);

        public async Task<User> GetByEmailAsync(string email) => await _authRepository.GetByEmailAsync(email);

        public async Task<User> GetByUsernameAsync(string username)
        {
            var allUsers = await _authRepository.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _authRepository.GetAllAsync();

        public async Task<IEnumerable<User>> GetByUserTypeAsync(UserType userType)
        {
            var allUsers = await _authRepository.GetAllAsync();
            return allUsers.Where(u => u.UserType == userType);
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            var hashedPassword = _passwordService.HashPassword(password);
            user.UpdatePassword(hashedPassword);

            await _authRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _authRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            await _authRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ValidatePasswordAsync(Guid userId, string password)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            return _passwordService.VerifyPassword(password, user.PasswordHash);
        }

        public async Task ChangePasswordAsync(Guid userId, string newPassword)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var hashedPassword = _passwordService.HashPassword(newPassword);
            user.UpdatePassword(hashedPassword);
            await UpdateAsync(user);
        }

        public async Task AssignRoleAsync(Guid userId, Guid roleId, DateTime? expiresAt = null)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var existingRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId && ur.IsActive);
            if (existingRole != null)
            {
                if (expiresAt.HasValue)
                    existingRole.ExtendRole(expiresAt.Value);
            }
            else
            {
                var newUserRole = new UserRoleEntity(userId, roleId, DateTime.UtcNow, true, expiresAt);
                user.UserRoles.Add(newUserRole);
            }

            await UpdateAsync(user);
        }

        public async Task RevokeRoleAsync(Guid userId, Guid roleId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var role = user.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId && ur.IsActive);
            if (role != null)
            {
                role.DeactivateRole();
                await UpdateAsync(user);
            }
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            return user.UserRoles
                       .Where(ur => ur.IsActive && !ur.IsExpired())
                       .Select(ur => ur.Role)
                       .Where(r => r.IsActive)
                       .ToList();
        }

        public async Task<User> ActivateUserAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.Activate();
            await UpdateAsync(user);
            return user;
        }

        public async Task<User> DeactivateUserAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.Deactivate();
            await UpdateAsync(user);
            return user;
        }

        public async Task<User> VerifyEmailAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.VerifyEmail();
            await UpdateAsync(user);
            return user;
        }

        public async Task<User> VerifyPhoneAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.VerifyPhone();
            await UpdateAsync(user);
            return user;
        }

        public async Task<User> UnlockUserAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            user.UnlockAccount();
            await UpdateAsync(user);
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            user.Deactivate();
            await UpdateAsync(user);
            return true;
        }
    }
}