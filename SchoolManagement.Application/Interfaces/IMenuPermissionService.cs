using SchoolManagement.Application.DTOs;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IMenuPermissionService
    {
        Task<IEnumerable<MenuItemDto>> GetUserMenusAsync(Guid userId);
        Task<MenuPermissions> GetUserMenuPermissionsAsync(Guid userId, Guid menuId);
        Task<bool> HasPermissionAsync(Guid userId, string permissionName);
        Task<bool> HasMenuAccessAsync(Guid userId, Guid menuId);
        Task AssignMenuPermissionsToRoleAsync(Guid roleId, Dictionary<Guid, MenuPermissions> menuPermissions);
        Task RevokeMenuPermissionsFromRoleAsync(Guid roleId, IEnumerable<Guid> menuIds);
    }
}
