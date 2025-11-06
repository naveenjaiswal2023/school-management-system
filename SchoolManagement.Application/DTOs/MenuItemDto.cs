using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string Component { get; set; }
        public string Type { get; set; }
        public int SortOrder { get; set; }
        public MenuPermissions Permissions { get; set; }
        public List<MenuItemDto> Children { get; set; } = new();
    }
}
