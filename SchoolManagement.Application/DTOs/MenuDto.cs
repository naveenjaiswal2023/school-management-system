using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string Component { get; set; }
        public string Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public Guid? ParentMenuId { get; set; }
        public string ParentMenuName { get; set; }
        public IEnumerable<MenuDto> SubMenus { get; set; }
    }
}
