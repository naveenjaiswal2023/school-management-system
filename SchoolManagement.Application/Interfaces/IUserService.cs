using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    //public interface IUserService
    //{
    //    Task<User> GetByIdAsync(Guid id);
    //    Task<User> GetByUsernameAsync(string username);
    //    Task<User> GetByEmailAsync(string email);
    //    Task<User> CreateAsync(User user);
    //    Task<User> UpdateAsync(User user);
    //    Task<bool> ValidatePasswordAsync(Guid userId, string password);
    //    Task ChangePasswordAsync(Guid userId, string newPassword);
    //    Task AssignRoleAsync(Guid userId, Guid roleId, DateTime? expiresAt = null);
    //    Task RevokeRoleAsync(Guid userId, Guid roleId);
    //    Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
    //}
    public interface IUserService
    {
        Task<User> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user, string password);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> ValidatePasswordAsync(Guid userId, string password);
        Task ChangePasswordAsync(Guid userId, string newPassword);
        Task AssignRoleAsync(Guid userId, Guid roleId, DateTime? expiresAt = null);
        Task RevokeRoleAsync(Guid userId, Guid roleId);
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
        Task<bool> DeleteAsync(Guid id);
        Task<User> ActivateUserAsync(Guid userId);
        Task<User> DeactivateUserAsync(Guid userId);
        Task<User> VerifyEmailAsync(Guid userId);
        Task<User> VerifyPhoneAsync(Guid userId);
        Task<User> UnlockUserAsync(Guid userId);
        Task<IEnumerable<User>> GetByUserTypeAsync(UserType userType);
    }
}
