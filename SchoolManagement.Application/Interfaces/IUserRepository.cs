using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByUserTypeAsync(UserType userType);
        Task AssignRoleAsync(Guid userId, Guid roleId, DateTime assignAt, bool isActive, DateTime? expiresAt = null);
        //Task AssignRoleAsync(User user);
        Task RevokeRoleAsync(Guid userId, Guid roleId);
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
    }
}
