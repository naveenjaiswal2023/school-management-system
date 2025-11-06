using MediatR;
using SchoolManagement.Application.Auth.Commands;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Auth.Handler
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _unitOfWork.AuthRepository.GetRefreshTokenAsync(request.RefreshToken);
            if (refreshToken == null || !refreshToken.IsActive)
                throw new AuthenticationException("Invalid refresh token");

            var user = await _unitOfWork.AuthRepository.GetByIdAsync(refreshToken.UserId);
            if (user == null || !user.IsActive)
                throw new AuthenticationException("User not found or inactive");

            // Revoke old refresh token
            refreshToken.Revoke();
            await _unitOfWork.AuthRepository.RevokeRefreshTokenAsync(refreshToken);

            // Generate new tokens
            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken(
                newRefreshTokenValue,
                DateTime.UtcNow.AddDays(7),
                user.Id);

            user.AddRefreshToken(newRefreshToken);
            user.RecordLogin();

            await _unitOfWork.AuthRepository.SaveRefreshTokenAsync(newRefreshToken);
            await _unitOfWork.AuthRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            string roleName = Enum.IsDefined(typeof(UserType), user.UserType)
            ? ((UserType)user.UserType).ToString()
            : "Unknown";

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshTokenValue,
                User = new UserDto
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = new List<string> { roleName }
                }
            };
        }
    }
}
