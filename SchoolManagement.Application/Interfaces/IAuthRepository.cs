using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken);
    }
}
