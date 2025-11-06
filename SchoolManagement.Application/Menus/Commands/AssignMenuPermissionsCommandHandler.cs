using MediatR;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Commands
{
    public class AssignMenuPermissionsCommandHandler : IRequestHandler<AssignMenuPermissionsCommand, AssignMenuPermissionsResponse>
    {
        private readonly IMenuPermissionService _menuPermissionService;

        public AssignMenuPermissionsCommandHandler(IMenuPermissionService menuPermissionService)
        {
            _menuPermissionService = menuPermissionService;
        }

        public async Task<AssignMenuPermissionsResponse> Handle(AssignMenuPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var menuPermissions = request.MenuPermissions.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new MenuPermissions
                    {
                        CanView = kvp.Value.CanView,
                        CanAdd = kvp.Value.CanAdd,
                        CanEdit = kvp.Value.CanEdit,
                        CanDelete = kvp.Value.CanDelete,
                        CanExport = kvp.Value.CanExport,
                        CanPrint = kvp.Value.CanPrint,
                        CanApprove = kvp.Value.CanApprove,
                        CanReject = kvp.Value.CanReject
                    });

                await _menuPermissionService.AssignMenuPermissionsToRoleAsync(request.RoleId, menuPermissions);

                return new AssignMenuPermissionsResponse
                {
                    Message = "Menu permissions assigned successfully",
                    Success = true,
                    PermissionsAssigned = menuPermissions.Count
                };
            }
            catch (Exception ex)
            {
                return new AssignMenuPermissionsResponse
                {
                    Message = $"Error assigning permissions: {ex.Message}",
                    Success = false
                };
            }
        }
    }
}
