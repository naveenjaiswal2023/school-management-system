using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Roles.Commands;
using SchoolManagement.Application.Roles.Queries;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var query = new GetAllRolesQuery();
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(Guid id)
        {
            var query = new GetRoleByIdQuery(id); 
            var role = await _mediator.Send(query);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        /// <summary>
        /// Create new role
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CreateRoleResponse>> CreateRole(CreateRoleCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetRole), new { id = response.Id }, response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Update role
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateRoleResponse>> UpdateRole(Guid id, UpdateRoleCommand command)
        {
            command.Id = id;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Delete role
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteRoleResponse>> DeleteRole(Guid id)
        {
            var command = new DeleteRoleCommand { Id = id };
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
