using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission> GetByIdAsync(Guid id);
        Task<Permission> GetByNameAsync(string name);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<IEnumerable<Permission>> GetByModuleAsync(string module);
        Task<Permission> CreateAsync(Permission permission);
        Task<Permission> UpdateAsync(Permission permission);
        Task DeleteAsync(Guid id);
    }
}
