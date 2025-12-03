using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Blog.Shared.Auth;

public class TokenManagerMiddleware
{
    private readonly ITokenManager _tokenManager;
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenManagerMiddleware> _logger;
        private readonly SecurityHeadersPolicy _policy;

        public TokenManagerMiddleware(ITokenManager tokenManager,
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            SecurityHeadersPolicy policy)
        {
            _tokenManager = tokenManager;
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<TokenManagerMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _policy = policy;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
            var isValid = false;
            if (StringValues.IsNullOrEmpty(authorizationHeader))
            {
                isValid = true;
            }
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var token = authorizationHeader.Single().Split(" ").Last();
                if (!handler.CanReadToken(token))
                {
                    isValid = false;
                }
                else
                {
                    //var taskResult = await Task.WhenAll(_tokenManager.IsCurrentActiveTokenAsync() , _tokenManager.IsCurrentActiveUserAsync()).ConfigureAwait(false);
                    //isValid = taskResult.All(a => a);
                }
            }
            if (isValid)
            {
                var headers = context.Response.Headers;

                foreach (var (key, value) in _policy.SetHeaders)
                {
                    headers[key] = value;
                }

                foreach (var header in _policy.RemoveHeaders)
                {
                    headers.Remove(header);
                }
                await _next(context);
                return;
            }
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            _logger.LogWarning("The unauthorized request");

        }
}
