using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Helpers;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Menus.Commands;
using SchoolManagement.Application.Menus.Queries;
using SchoolManagement.Domain.Entities;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenusController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMenuPermissionService _menuPermissionService;

        public MenusController(IMediator mediator, IMenuPermissionService menuPermissionService)
        {
            _mediator = mediator;
            _menuPermissionService = menuPermissionService;
        }

        /// <summary>
        /// Get user's accessible menus with permissions
        /// </summary>
        [HttpGet("user-menus")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetUserMenus()
        {
            var userId = GetCurrentUserId();
            var menus = await _menuPermissionService.GetUserMenusAsync(userId);
            return Ok(menus);
        }

        /// <summary>
        /// Get all menus (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetAllMenus()
        {
            var query = new GetAllMenusQuery();
            var menus = await _mediator.Send(query);
            return Ok(menus);
        }

        /// <summary>
        /// Get menu hierarchy
        /// </summary>
        [HttpGet("hierarchy")]
        //[Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetMenuHierarchy()
        {
            var query = new GetAllMenusQuery();
            var menus = await _mediator.Send(query);
            return Ok(menus);
        }

        /// <summary>
        /// Create new menu
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<CreateMenuResponse>> CreateMenu(CreateMenuCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetMenu), new { id = response.Id }, response);
            }

            return BadRequest(response);
        }

        // Existing method
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<MenuDto>> GetMenu(Guid id)
        {
            var query = new GetMenuByIdQuery { Id = id };
            var menu = await _mediator.Send(query);

            if (menu == null)
                return NotFound();

            return Ok(menu);
        }

        // Rename route for role-based fetch
        [HttpGet("role/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<MenuDto>> GetMenuByRoleId(Guid id)
        {
            var query = new GetMenuByIdQuery { Id = id };
            var menu = await _mediator.Send(query);

            if (menu == null)
                return NotFound();

            return Ok(menu);
        }


        /// <summary>
        /// Update menu
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<UpdateMenuResponse>> UpdateMenu(Guid id, UpdateMenuCommand command)
        {
            command.Id = id;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Delete menu
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<DeleteMenuResponse>> DeleteMenu(Guid id)
        {
            var command = new DeleteMenuCommand { Id = id };
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Check user's access to specific menu
        /// </summary>
        [HttpGet("{menuId}/access")]
        public async Task<ActionResult<MenuAccessDto>> CheckMenuAccess(Guid menuId)
        {
            var userId = GetCurrentUserId();
            var hasAccess = await _menuPermissionService.HasMenuAccessAsync(userId, menuId);
            var permissions = await _menuPermissionService.GetUserMenuPermissionsAsync(userId, menuId);

            return Ok(new MenuAccessDto
            {
                MenuId = menuId,
                HasAccess = hasAccess,
                Permissions = permissions
            });
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User not authenticated"));
        }
    }
}
