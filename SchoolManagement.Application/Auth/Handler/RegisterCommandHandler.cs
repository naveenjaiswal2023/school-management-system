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
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.AuthRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new ValidationException("Email is already registered");

            var hashedPassword = _passwordService.HashPassword(request.Password);

            var user = new User(
                request.Username,
                request.Email,
                request.FirstName,
                request.LastName,
                hashedPassword,
                (UserType)request.Role); // Cast UserRole to UserType

            //var accessToken = _tokenService.GenerateAccessToken(user);
            //var refreshTokenValue = _tokenService.GenerateRefreshToken();

            //var refreshToken = new RefreshToken(
            //    refreshTokenValue,
            //    DateTime.UtcNow.AddDays(7),
            //    user.Id);

            //user.AddRefreshToken(refreshToken);

            //await _unitOfWork.AuthRepository.AddAsync(user);
            //await _unitOfWork.AuthRepository.SaveRefreshTokenAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            string roleName = Enum.IsDefined(typeof(UserType), user.UserType)
            ? ((UserType)user.UserType).ToString()
            : "Unknown";

            return new AuthResponseDto
            {
                //AccessToken = accessToken,
                //RefreshToken = refreshTokenValue,
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
