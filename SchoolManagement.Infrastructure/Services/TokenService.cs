using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TokenService> _logger;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(
            IConfiguration configuration,
            IMemoryCache cache = null,
            ILogger<TokenService> logger = null)
        {
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
            _secretKey = _configuration["Jwt:SecretKey"];
            _issuer = _configuration["Jwt:Issuer"];
            _audience = _configuration["Jwt:Audience"];
        }

        public string GenerateAccessToken(User user)
        {
            // Optional caching
            if (_cache != null)
            {
                var cacheKey = $"access_token_{user.Id}_{user.Email}";
                if (_cache.TryGetValue(cacheKey, out string cachedToken))
                {
                    _logger?.LogInformation("Access token retrieved from cache for user {UserId}", user.Id);
                    return cachedToken;
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserType.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                }),
                Expires = DateTime.UtcNow.AddHours(1), // 1 hour expiry
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Cache the token if caching is available
            if (_cache != null)
            {
                var cacheKey = $"access_token_{user.Id}_{user.Email}";
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    Priority = CacheItemPriority.Normal
                };
                _cache.Set(cacheKey, tokenString, cacheOptions);
                _logger?.LogInformation("Access token generated and cached for user {UserId}", user.Id);
            }

            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<bool> ValidateAccessTokenAsync(string token)
        {
            // Optional caching for validation results
            if (_cache != null)
            {
                var cacheKey = $"token_validation_{token.GetHashCode()}";
                if (_cache.TryGetValue(cacheKey, out bool cachedResult))
                {
                    _logger?.LogInformation("Token validation result retrieved from cache");
                    return cachedResult;
                }
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                });

                // Cache the validation result if caching is available
                if (_cache != null)
                {
                    var cacheKey = $"token_validation_{token.GetHashCode()}";
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                        Priority = CacheItemPriority.High
                    };
                    _cache.Set(cacheKey, true, cacheOptions);
                }

                return true;
            }
            catch
            {
                if (_cache != null)
                {
                    var cacheKey = $"token_validation_{token.GetHashCode()}";
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                        Priority = CacheItemPriority.High
                    };
                    _cache.Set(cacheKey, false, cacheOptions);
                }
                return false;
            }
        }

        public DateTime GetTokenExpiration(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            return jwt.ValidTo;
        }
    }
}
