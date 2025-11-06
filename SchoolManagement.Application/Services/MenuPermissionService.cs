using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class MenuPermissionService : IMenuPermissionService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleMenuPermissionRepository _roleMenuPermissionRepository;
        private readonly IUserService _userService;

        public MenuPermissionService(
            IMenuRepository menuRepository,
            IRoleMenuPermissionRepository roleMenuPermissionRepository,
            IUserService userService)
        {
            _menuRepository = menuRepository;
            _roleMenuPermissionRepository = roleMenuPermissionRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<MenuItemDto>> GetUserMenusAsync(Guid userId)
        {
            var userRoles = await _userService.GetUserRolesAsync(userId);
            var userRoleIds = userRoles.Select(r => r.Id).ToList();

            var allMenus = await _menuRepository.GetMenuHierarchyAsync();
            var userMenus = new List<MenuItemDto>();

            foreach (var menu in allMenus.Where(m => m.ParentMenuId == null))
            {
                var menuItem = await BuildMenuItemAsync(menu, userRoleIds);
                if (menuItem != null)
                {
                    userMenus.Add(menuItem);
                }
            }

            return userMenus.OrderBy(m => m.SortOrder);
        }

        private async Task<MenuItemDto> BuildMenuItemAsync(Menu menu, List<Guid> userRoleIds)
        {
            var hasAccess = await CheckMenuAccessAsync(menu.Id, userRoleIds);
            if (!hasAccess || !menu.IsActive || !menu.IsVisible)
                return null;

            var menuItem = new MenuItemDto
            {
                Id = menu.Id,
                Name = menu.Name,
                DisplayName = menu.DisplayName,
                Icon = menu.Icon,
                Route = menu.Route,
                Component = menu.Component,
                Type = menu.Type.ToString(),
                SortOrder = menu.SortOrder,
                Children = new List<MenuItemDto>()
            };

            // Get permissions for this menu
            var permissions = await GetMenuPermissionsAsync(menu.Id, userRoleIds);
            menuItem.Permissions = permissions;

            // Recursively build child menus
            var childMenus = menu.SubMenus.Where(sm => sm.IsActive && sm.IsVisible);
            foreach (var childMenu in childMenus)
            {
                var childMenuItem = await BuildMenuItemAsync(childMenu, userRoleIds);
                if (childMenuItem != null)
                {
                    menuItem.Children.Add(childMenuItem);
                }
            }

            return menuItem;
        }

        public async Task<MenuPermissions> GetUserMenuPermissionsAsync(Guid userId, Guid menuId)
        {
            var userRoles = await _userService.GetUserRolesAsync(userId);
            var userRoleIds = userRoles.Select(r => r.Id).ToList();

            return await GetMenuPermissionsAsync(menuId, userRoleIds);
        }

        private async Task<MenuPermissions> GetMenuPermissionsAsync(Guid menuId, List<Guid> roleIds)
        {
            var permissions = new MenuPermissions();

            foreach (var roleId in roleIds)
            {
                var roleMenuPermission = await _roleMenuPermissionRepository.GetByRoleAndMenuAsync(roleId, menuId);
                if (roleMenuPermission != null)
                {
                    var rolePermissions = roleMenuPermission.GetPermissions();

                    // Combine permissions (OR operation - if any role grants permission, user has it)
                    permissions.CanView |= rolePermissions.CanView;
                    permissions.CanAdd |= rolePermissions.CanAdd;
                    permissions.CanEdit |= rolePermissions.CanEdit;
                    permissions.CanDelete |= rolePermissions.CanDelete;
                    permissions.CanExport |= rolePermissions.CanExport;
                    permissions.CanPrint |= rolePermissions.CanPrint;
                    permissions.CanApprove |= rolePermissions.CanApprove;
                    permissions.CanReject |= rolePermissions.CanReject;
                }
            }

            return permissions;
        }

        private async Task<bool> CheckMenuAccessAsync(Guid menuId, List<Guid> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var roleMenuPermission = await _roleMenuPermissionRepository.GetByRoleAndMenuAsync(roleId, menuId);
                if (roleMenuPermission?.CanView == true)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permissionName)
        {
            var userRoles = await _userService.GetUserRolesAsync(userId);

            // Implementation would check if any of the user's roles has the specified permission
            // This is a simplified version - in reality, you'd query the RolePermission table
            return userRoles.Any(role => role.IsActive);
        }

        public async Task<bool> HasMenuAccessAsync(Guid userId, Guid menuId)
        {
            var userRoles = await _userService.GetUserRolesAsync(userId);
            var userRoleIds = userRoles.Select(r => r.Id).ToList();

            return await CheckMenuAccessAsync(menuId, userRoleIds);
        }

        public async Task AssignMenuPermissionsToRoleAsync(Guid roleId, Dictionary<Guid, MenuPermissions> menuPermissions)
        {
            foreach (var kvp in menuPermissions)
            {
                var menuId = kvp.Key;
                var permissions = kvp.Value;

                var existingPermission = await _roleMenuPermissionRepository.GetByRoleAndMenuAsync(roleId, menuId);

                if (existingPermission != null)
                {
                    existingPermission.SetPermissions(permissions);
                    await _roleMenuPermissionRepository.UpdateAsync(existingPermission);
                }
                else
                {
                    var newPermission = new RoleMenuPermission(roleId, menuId, permissions);
                    await _roleMenuPermissionRepository.CreateAsync(newPermission);
                }
            }
        }

        public async Task RevokeMenuPermissionsFromRoleAsync(Guid roleId, IEnumerable<Guid> menuIds)
        {
            foreach (var menuId in menuIds)
            {
                await _roleMenuPermissionRepository.DeleteAsync(roleId, menuId);
            }
        }
    }
}
