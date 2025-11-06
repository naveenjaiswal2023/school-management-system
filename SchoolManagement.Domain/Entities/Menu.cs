using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public string? Name { get; private set; }
        public string? DisplayName { get; private set; }
        public string? Description { get; private set; }
        public string? Icon { get; private set; }
        public string? Route { get; private set; }
        public string? Component { get; private set; }
        public int SortOrder { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsVisible { get; private set; }
        public MenuType Type { get; private set; }
        public Guid? ParentMenuId { get; private set; }

        // Navigation Properties
        public virtual Menu ParentMenu { get; private set; }
        public virtual ICollection<Menu> SubMenus { get; private set; }
        public virtual ICollection<RoleMenuPermission> RoleMenuPermissions { get; private set; }

        private Menu()
        {
            SubMenus = new List<Menu>();
            RoleMenuPermissions = new List<RoleMenuPermission>();
        }

        public Menu(string name, string displayName, string route, MenuType type,
                   string icon = null, string description = null, Guid? parentMenuId = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Route = route;
            Type = type;
            Icon = icon;
            Description = description;
            ParentMenuId = parentMenuId;
            IsActive = true;
            IsVisible = true;
            SortOrder = 0;

            SubMenus = new List<Menu>();
            RoleMenuPermissions = new List<RoleMenuPermission>();
        }

        public void UpdateDetails(string displayName, string description, string icon,
                                string route, string component)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Description = description;
            Icon = icon;
            Route = route;
            Component = component;
        }

        public void SetSortOrder(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        public void SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetParent(Guid? parentMenuId)
        {
            ParentMenuId = parentMenuId;
        }
    }
}
