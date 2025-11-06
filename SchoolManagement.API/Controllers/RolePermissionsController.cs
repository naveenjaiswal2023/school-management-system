using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Menus.Commands;
using SchoolManagement.Application.RolePermissions.Commands;
using SchoolManagement.Application.Roles.Queries;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMenuPermissionService _menuPermissionService;

        public RolePermissionsController(IMediator mediator, IMenuPermissionService menuPermissionService)
        {
            _mediator = mediator;
            _menuPermissionService = menuPermissionService;
        }

        /// <summary>
        /// Get role's menu permissions
        /// </summary>
        [HttpGet("{roleId}/menu-permissions")]
        public async Task<ActionResult<IEnumerable<RoleMenuPermissionDto>>> GetRoleMenuPermissions(Guid roleId)
        {
            var query = new GetRoleMenuPermissionsQuery(roleId);
            var permissions = await _mediator.Send(query);
            return Ok(permissions);
        }

        /// <summary>
        /// Assign menu permissions to role
        /// </summary>
        [HttpPost("{roleId}/menu-permissions")]
        public async Task<ActionResult<AssignMenuPermissionsResponse>> AssignMenuPermissions(
        Guid roleId,  [FromBody] AssignMenuPermissionsCommand command)
        {
            command.RoleId = roleId;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }



        /// <summary>
        /// Update specific menu permission for role
        /// </summary>
        [HttpPut("{roleId}/menu-permissions/{menuId}")]
        public async Task<ActionResult<UpdateMenuPermissionResponse>> UpdateMenuPermission(
            Guid roleId, Guid menuId, UpdateMenuPermissionCommand command)
        {
            command.RoleId = roleId;
            command.MenuId = menuId;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Revoke menu permissions from role
        /// </summary>
        [HttpDelete("{roleId}/menu-permissions")]
        public async Task<ActionResult<RevokeMenuPermissionsResponse>> RevokeMenuPermissions(
            Guid roleId, RevokeMenuPermissionsCommand command)
        {
            command.RoleId = roleId;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
