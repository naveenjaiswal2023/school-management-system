using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IRoleMenuPermissionRepository
    {
        Task<RoleMenuPermission> GetByRoleAndMenuAsync(Guid roleId, Guid menuId);
        Task<IEnumerable<RoleMenuPermission>> GetByRoleAsync(Guid roleId);
        Task<IEnumerable<RoleMenuPermission>> GetByMenuAsync(Guid menuId);
        Task<RoleMenuPermission> CreateAsync(RoleMenuPermission roleMenuPermission);
        Task<RoleMenuPermission> UpdateAsync(RoleMenuPermission roleMenuPermission);
        Task DeleteAsync(Guid roleId, Guid menuId);
        Task DeleteByRoleAsync(Guid roleId);
        Task DeleteByMenuAsync(Guid menuId);
    }
}
