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
    public class MenuRepository : IMenuRepository
    {
        private readonly SchoolManagementDbContext _context;

        public MenuRepository(SchoolManagementDbContext context)
        {
            _context = context;
        }

        //public async Task<Menu> GetByIdAsync(Guid id)
        //{
        //    return await _context.Menus
        //        .Include(m => m.ParentMenu)
        //        .Include(m => m.SubMenus)
        //        .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        //}

        public async Task<IEnumerable<Menu>> GetByIdAsync(Guid roleId)
        {
            return await _context.Menus
                .Include(m => m.ParentMenu)
                .Include(m => m.SubMenus)
                .Where(m => !m.IsDeleted)
                .Where(m => m.RoleMenuPermissions.Any(rmp => rmp.RoleId == roleId && rmp.CanView))
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _context.Menus
                .Include(m => m.ParentMenu)
                .Include(m => m.SubMenus)
                .Where(m => !m.IsDeleted)
                .OrderBy(m => m.ParentMenuId)
                .ThenBy(m => m.SortOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetActiveMenusAsync()
        {
            return await _context.Menus
                .Include(m => m.ParentMenu)
                .Include(m => m.SubMenus)
                .Where(m => !m.IsDeleted && m.IsActive)
                .OrderBy(m => m.ParentMenuId)
                .ThenBy(m => m.SortOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetMenusByParentAsync(Guid? parentId)
        {
            return await _context.Menus
                .Where(m => m.ParentMenuId == parentId && !m.IsDeleted && m.IsActive)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();
        }

        public async Task<Menu> CreateAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            return menu;
        }

        public async Task<Menu> UpdateAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            return menu;
        }

        public async Task DeleteAsync(Guid id)
        {
            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (menu != null)
            {
                menu.MarkAsDeleted();
                _context.Menus.Update(menu);
            }
        }

        public async Task<IEnumerable<Menu>> GetMenuHierarchyAsync()
        {
            return await _context.Menus
                .Include(m => m.SubMenus.Where(sm => !sm.IsDeleted && sm.IsActive))
                .Where(m => !m.IsDeleted && m.IsActive)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();
        }
    }
}
