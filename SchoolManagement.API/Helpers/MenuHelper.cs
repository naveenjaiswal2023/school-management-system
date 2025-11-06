using SchoolManagement.Application.DTOs;

namespace SchoolManagement.API.Helpers
{
    public static class MenuHelper
    {
        /// <summary>
        /// Converts menu hierarchy to JSON structure for frontend consumption
        /// </summary>
        public static object ToMenuJson(IEnumerable<MenuItemDto> menus)
        {
            return menus.Select(menu => new
            {
                id = menu.Id,
                name = menu.Name,
                displayName = menu.DisplayName,
                icon = menu.Icon,
                route = menu.Route,
                component = menu.Component,
                type = menu.Type,
                sortOrder = menu.SortOrder,
                permissions = new
                {
                    canView = menu.Permissions?.CanView ?? false,
                    canAdd = menu.Permissions?.CanAdd ?? false,
                    canEdit = menu.Permissions?.CanEdit ?? false,
                    canDelete = menu.Permissions?.CanDelete ?? false,
                    canExport = menu.Permissions?.CanExport ?? false,
                    canPrint = menu.Permissions?.CanPrint ?? false,
                    canApprove = menu.Permissions?.CanApprove ?? false,
                    canReject = menu.Permissions?.CanReject ?? false
                },
                children = menu.Children?.Any() == true ? ToMenuJson(menu.Children) : null
            }).ToList();
        }

        /// <summary>
        /// Generate breadcrumb navigation for a given route
        /// </summary>
        public static List<BreadcrumbItem> GenerateBreadcrumbs(string route, IEnumerable<MenuItemDto> menus)
        {
            var breadcrumbs = new List<BreadcrumbItem>();
            var pathSegments = route.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var currentPath = "";
            foreach (var segment in pathSegments)
            {
                currentPath += "/" + segment;
                var menu = FindMenuByRoute(currentPath, menus);
                if (menu != null)
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Title = menu.DisplayName,
                        Route = menu.Route,
                        Icon = menu.Icon
                    });
                }
            }

            return breadcrumbs;
        }

        private static MenuItemDto FindMenuByRoute(string route, IEnumerable<MenuItemDto> menus)
        {
            foreach (var menu in menus)
            {
                if (menu.Route?.Equals(route, StringComparison.OrdinalIgnoreCase) == true)
                    return menu;

                if (menu.Children?.Any() == true)
                {
                    var found = FindMenuByRoute(route, menu.Children);
                    if (found != null)
                        return found;
                }
            }
            return null;
        }
    }
}
