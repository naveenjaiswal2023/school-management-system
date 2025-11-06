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
    public class RevokeAllTokensCommandHandler : IRequestHandler<RevokeAllTokensCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeAllTokensCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RevokeAllTokensCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.AuthRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException("User not found");

            foreach (var token in user.RefreshTokens)
            {
                if (token.IsActive)
                {
                    token.Revoke();
                    await _unitOfWork.AuthRepository.RevokeRefreshTokenAsync(token);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
