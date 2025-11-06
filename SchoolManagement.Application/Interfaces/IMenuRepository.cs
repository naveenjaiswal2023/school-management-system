using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetByIdAsync(Guid id);
        Task<IEnumerable<Menu>> GetAllAsync();
        Task<IEnumerable<Menu>> GetActiveMenusAsync();
        Task<IEnumerable<Menu>> GetMenusByParentAsync(Guid? parentId);
        Task<Menu> CreateAsync(Menu menu);
        Task<Menu> UpdateAsync(Menu menu);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<Menu>> GetMenuHierarchyAsync();
    }
}
