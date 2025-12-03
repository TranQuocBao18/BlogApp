using System;
using System.Security.Claims;
using Blog.Infrastructure.Shared.Constants;
using Blog.Infrastructure.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Blog.Infrastructure.Shared.Services;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    private readonly ILogger<AuthenticatedUserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, ILogger<AuthenticatedUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Guid UserGuidId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.Session?.GetString("UserId");
                return Guid.Parse(userId);
            }
        }

        public string UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.Session?.GetString("UserId");
                return userId;
            }
        }

        public string JwtToken
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];
            }
        }

        public string IPAddressClient
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                var isAuthenticated = _httpContextAccessor.HttpContext?.User?.Identities?.FirstOrDefault()?.IsAuthenticated;
                if (!isAuthenticated.HasValue)
                {
                    return false;
                }

                return isAuthenticated.Value;
            }
        }

        public IEnumerable<string> Permissions
        {
            get
            {
                var permissions = _httpContextAccessor.HttpContext?.User?
                    .FindAll(IdentityConstant.Permission)
                    .Select(x => x.Value);
                return permissions;
            }
        }

        public string Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
                return role;
            }
        }
}
