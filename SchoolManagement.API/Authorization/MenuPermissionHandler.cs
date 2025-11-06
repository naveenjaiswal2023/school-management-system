using Microsoft.AspNetCore.Authorization;
using SchoolManagement.Application.Interfaces;
using System.Security.Claims;

namespace SchoolManagement.API.Authorization
{
    public class MenuPermissionHandler : AuthorizationHandler<MenuPermissionAttribute>
    {
        private readonly IMenuPermissionService _menuPermissionService;
        private readonly IMenuRepository _menuRepository;

        public MenuPermissionHandler(
            IMenuPermissionService menuPermissionService,
            IMenuRepository menuRepository)
        {
            _menuPermissionService = menuPermissionService;
            _menuRepository = menuRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MenuPermissionAttribute requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                context.Fail();
                return;
            }

            // Find menu by name
            var menus = await _menuRepository.GetAllAsync();
            var menu = menus.FirstOrDefault(m => m.Name.Equals(requirement.MenuName, StringComparison.OrdinalIgnoreCase));

            if (menu == null)
            {
                context.Fail();
                return;
            }

            // Check if user has access to the menu
            var hasAccess = await _menuPermissionService.HasMenuAccessAsync(userId, menu.Id);
            if (!hasAccess)
            {
                context.Fail();
                return;
            }

            // Check specific permissions if required
            if (requirement.RequiredPermissions?.Length > 0)
            {
                var userPermissions = await _menuPermissionService.GetUserMenuPermissionsAsync(userId, menu.Id);

                foreach (var requiredPermission in requirement.RequiredPermissions)
                {
                    var hasPermission = requiredPermission.ToLower() switch
                    {
                        "view" => userPermissions.CanView,
                        "add" => userPermissions.CanAdd,
                        "edit" => userPermissions.CanEdit,
                        "delete" => userPermissions.CanDelete,
                        "export" => userPermissions.CanExport,
                        "print" => userPermissions.CanPrint,
                        "approve" => userPermissions.CanApprove,
                        "reject" => userPermissions.CanReject,
                        _ => false
                    };

                    if (!hasPermission)
                    {
                        context.Fail();
                        return;
                    }
                }
            }

            context.Succeed(requirement);
        }
    }
}
