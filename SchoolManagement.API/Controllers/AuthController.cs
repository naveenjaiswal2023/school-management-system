using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Extensions;
using SchoolManagement.Application.Auth.Commands;
using SchoolManagement.Application.Auth.Queries;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Domain.Exceptions;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        //{
        //    try
        //    {
        //        var command = new LoginCommand
        //        {
        //            Email = request.Email,
        //            Password = request.Password
        //        };

        //        var result = await _mediator.Send(command);
        //        return Ok(result);
        //    }
        //    catch (AuthenticationException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpPost("register")]
        //public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        //{
        //    try
        //    {
        //        var command = new RegisterCommand
        //        {
        //            Email = request.Email,
        //            Password = request.Password,
        //            FirstName = request.FirstName,
        //            LastName = request.LastName,
        //            Role = (Domain.Enums.UserType)request.Role
        //        };

        //        var result = await _mediator.Send(command);
        //        return Ok(result);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpPost("refresh")]
        //public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        //{
        //    try
        //    {
        //        var command = new RefreshTokenCommand
        //        {
        //            RefreshToken = request.RefreshToken
        //        };

        //        var result = await _mediator.Send(command);
        //        return Ok(result);
        //    }
        //    catch (AuthenticationException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //}

        //[Authorize]
        //[HttpPost("logout")]
        //public async Task<ActionResult> Logout()
        //{
        //    // Implementation for logout (revoke refresh tokens)
        //    return Ok(new { message = "Logged out successfully" });
        //}

        /// <summary>
        /// Authenticates a user and returns JWT tokens
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            var command = new RegisterCommand
            {
                Username= request.Username,
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = (Domain.Enums.UserType)request.Role
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Refreshes the access token using refresh token
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var command = new RefreshTokenCommand
            {
                RefreshToken = request.RefreshToken
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Logs out the user by revoking the refresh token
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            var command = new LogoutCommand
            {
                RefreshToken = request.RefreshToken,
                UserId = User.GetUserId()
            };

            await _mediator.Send(command);
            return Ok(new { message = "Logged out successfully" });
        }

        /// <summary>
        /// Revokes all refresh tokens for the current user
        /// </summary>
        [Authorize]
        [HttpPost("revoke-all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> RevokeAllTokens()
        {
            var command = new RevokeAllTokensCommand
            {
                UserId = User.GetUserId()
            };

            await _mediator.Send(command);
            return Ok(new { message = "All tokens revoked successfully" });
        }

        /// <summary>
        /// Gets the current user's profile
        /// </summary>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var query = new GetUserProfileQuery
            {
                UserId = User.GetUserId()
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Validates the current token (useful for checking if user is still authenticated)
        /// </summary>
        [Authorize]
        [HttpGet("validate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult ValidateToken()
        {
            return Ok(new
            {
                message = "Token is valid",
                userId = User.GetUserId(),
                email = User.GetUserEmail(),
                role = User.GetUserRole()
            });
        }
    }
}
