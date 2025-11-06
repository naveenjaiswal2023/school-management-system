using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(Guid id);
        Task<Role> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task DeleteAsync(Guid id);
    }
}
