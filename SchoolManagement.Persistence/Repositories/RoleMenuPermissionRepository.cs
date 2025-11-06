using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Persistence.Repositories
{
    public class RoleMenuPermissionRepository : IRoleMenuPermissionRepository
    {
        private readonly SchoolManagementDbContext _context;

        public RoleMenuPermissionRepository(SchoolManagementDbContext context)
        {
            _context = context;
        }

        public async Task<RoleMenuPermission> GetByRoleAndMenuAsync(Guid roleId, Guid menuId)
        {
            return await _context.RoleMenuPermissions
                .Include(rmp => rmp.Role)
                .Include(rmp => rmp.Menu)
                .FirstOrDefaultAsync(rmp => rmp.RoleId == roleId && rmp.MenuId == menuId && !rmp.IsDeleted);
        }

        public async Task<IEnumerable<RoleMenuPermission>> GetByRoleAsync(Guid roleId)
        {
            return await _context.RoleMenuPermissions
                .Include(rmp => rmp.Menu)
                .Where(rmp => rmp.RoleId == roleId && !rmp.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoleMenuPermission>> GetByMenuAsync(Guid menuId)
        {
            return await _context.RoleMenuPermissions
                .Include(rmp => rmp.Role)
                .Where(rmp => rmp.MenuId == menuId && !rmp.IsDeleted)
                .ToListAsync();
        }

        public async Task<RoleMenuPermission> CreateAsync(RoleMenuPermission roleMenuPermission)
        {
            _context.RoleMenuPermissions.Add(roleMenuPermission);
            return roleMenuPermission;
        }

        public async Task<RoleMenuPermission> UpdateAsync(RoleMenuPermission roleMenuPermission)
        {
            _context.RoleMenuPermissions.Update(roleMenuPermission);
            return roleMenuPermission;
        }

        public async Task DeleteAsync(Guid roleId, Guid menuId)
        {
            var permission = await GetByRoleAndMenuAsync(roleId, menuId);
            if (permission != null)
            {
                permission.MarkAsDeleted();
                _context.RoleMenuPermissions.Update(permission);
            }
        }

        public async Task DeleteByRoleAsync(Guid roleId)
        {
            var permissions = await GetByRoleAsync(roleId);
            foreach (var permission in permissions)
            {
                permission.MarkAsDeleted();
                _context.RoleMenuPermissions.Update(permission);
            }
        }

        public async Task DeleteByMenuAsync(Guid menuId)
        {
            var permissions = await GetByMenuAsync(menuId);
            foreach (var permission in permissions)
            {
                permission.MarkAsDeleted();
                _context.RoleMenuPermissions.Update(permission);
            }
        }
    }
}
