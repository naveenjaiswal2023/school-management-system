using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class CachedTokenService : ITokenService
    {
        private readonly TokenService _tokenService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedTokenService> _logger;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public CachedTokenService(
            TokenService tokenService,
            IMemoryCache cache,
            ILogger<CachedTokenService> logger)
        {
            _tokenService = tokenService;
            _cache = cache;
            _logger = logger;
        }

        public string GenerateAccessToken(User user)
        {
            var cacheKey = $"access_token_{user.Id}_{user.Email}";

            if (_cache.TryGetValue(cacheKey, out string cachedToken))
            {
                _logger.LogInformation("Access token retrieved from cache for user {UserId}", user.Id);
                return cachedToken;
            }

            var token = _tokenService.GenerateAccessToken(user);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiration,
                Priority = CacheItemPriority.Normal
            };

            _cache.Set(cacheKey, token, cacheOptions);
            _logger.LogInformation("Access token generated and cached for user {UserId}", user.Id);

            return token;
        }

        public string GenerateRefreshToken()
        {
            return _tokenService.GenerateRefreshToken();
        }

        public async Task<bool> ValidateAccessTokenAsync(string token)
        {
            var cacheKey = $"token_validation_{token.GetHashCode()}";

            if (_cache.TryGetValue(cacheKey, out bool cachedResult))
            {
                _logger.LogInformation("Token validation result retrieved from cache");
                return cachedResult;
            }

            var isValid = await _tokenService.ValidateAccessTokenAsync(token);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                Priority = CacheItemPriority.High
            };

            _cache.Set(cacheKey, isValid, cacheOptions);
            return isValid;
        }

        public DateTime GetTokenExpiration(string token)
        {
            return _tokenService.GetTokenExpiration(token);
        }
    }
}
