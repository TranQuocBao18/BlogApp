using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Blog.Shared.Auth;

public class TokenManager : ITokenManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public TokenManager(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        private string GetCurrentToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
            return StringValues.IsNullOrEmpty(authorizationHeader)
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }
        public string GetCurrentUserIdAsync()
        {
            var stream = GetCurrentToken();
            if (!string.IsNullOrWhiteSpace(stream))
            {
                var tokens = GetJwtSecurityToken(stream);
                var id = tokens.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    return id;
                }
            }
            return string.Empty;
        }

        private JwtSecurityToken GetJwtSecurityToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            return tokenS;
        }
}
