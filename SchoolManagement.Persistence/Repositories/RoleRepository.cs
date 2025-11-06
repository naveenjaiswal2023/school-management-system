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
    public class RoleRepository : IRoleRepository
    {
        private readonly SchoolManagementDbContext _context;

        public RoleRepository(SchoolManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            return await _context.Roles
                .Include(r => r.RoleMenuPermissions)
                .ThenInclude(rmp => rmp.Menu)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.Level)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            return await _context.Roles
                .Where(r => !r.IsDeleted && r.IsActive)
                .OrderBy(r => r.Level)
                .ToListAsync();
        }

        public async Task<Role> CreateAsync(Role role)
        {
            _context.Roles.Add(role);
            return role;
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            return role;
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await GetByIdAsync(id);
            if (role != null && !role.IsSystemRole)
            {
                role.MarkAsDeleted();
                _context.Roles.Update(role);
            }
        }
    }
}
