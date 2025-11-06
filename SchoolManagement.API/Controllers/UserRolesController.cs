using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.UserRoles.Commands;
using SchoolManagement.Application.UserRoles.Queries;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin,HRManager")]
    public class UserRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get user's roles
        /// </summary>
        [HttpGet("{userId}/roles")]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetUserRoles(Guid userId)
        {
            var query = new GetUserRolesQuery { UserId = userId };
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }

        /// <summary>
        /// Assign role to user
        /// </summary>
        [HttpPost("{userId}/roles")]
        public async Task<ActionResult<AssignRoleToUserResponse>> AssignRoleToUser(
            Guid userId, AssignRoleToUserCommand command)
        {
            command.UserId = userId;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Revoke role from user
        /// </summary>
        [HttpDelete("{userId}/roles/{roleId}")]
        public async Task<ActionResult<RevokeRoleFromUserResponse>> RevokeRoleFromUser(
            Guid userId, Guid roleId)
        {
            var command = new RevokeRoleFromUserCommand { UserId = userId, RoleId = roleId };
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Update user role (extend expiry, activate/deactivate)
        /// </summary>
        [HttpPut("{userId}/roles/{roleId}")]
        public async Task<ActionResult<UpdateUserRoleResponse>> UpdateUserRole(
            Guid userId, Guid roleId, UpdateUserRoleCommand command)
        {
            command.UserId = userId;
            command.RoleId = roleId;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
