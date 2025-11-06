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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly SchoolManagementDbContext _context;

        public PermissionRepository(SchoolManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Permission> GetByIdAsync(Guid id)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Permission> GetByNameAsync(string name)
        {
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByModuleAsync(string module)
        {
            return await _context.Permissions
                .Where(p => p.Module == module && !p.IsDeleted)
                .OrderBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<Permission> CreateAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            return permission;
        }

        public async Task<Permission> UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            return permission;
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await GetByIdAsync(id);
            if (permission != null && !permission.IsSystemPermission)
            {
                permission.MarkAsDeleted();
                _context.Permissions.Update(permission);
            }
        }
    }
}
