using MediatR;
using SchoolManagement.Application.Auth.Commands;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Auth.Handler
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _unitOfWork.AuthRepository.GetRefreshTokenAsync(request.RefreshToken);

            if (refreshToken == null || refreshToken.UserId != request.UserId)
                throw new AuthenticationException("Invalid refresh token");

            refreshToken.Revoke();
            await _unitOfWork.AuthRepository.RevokeRefreshTokenAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
