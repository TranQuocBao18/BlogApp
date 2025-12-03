using System;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Blog.Shared.Auth;

public static class AuthExtensions
{
    public static string GetClaim(this ClaimsPrincipal user, string type) => user.Claims.FirstOrDefault(f => f.Type == type)?.Value;

        public static IServiceCollection AddTokenManager(this IServiceCollection services, IConfiguration config)
        {
            var authOptions = config.GetOptionsExt<AuthOptions>("Auth");
            if (!authOptions.TokenManagerDisabled)
            {
                services.AddTransient<ITokenManager, TokenManager>();
            }
            else
            {
                // disable OWASP check which might effect to the real-performance of the whole system 
                services.AddTransient<ITokenManager, NoOpTokenManager>();
            }

            return services;
        }

        public static IApplicationBuilder UseTokenManager(this IApplicationBuilder app, IConfiguration config, SecurityHeadersBuilder securityHeadersBuilder = null)
        {
            var authOptions = config.GetOptions<AuthOptions>("Auth");
            if (!authOptions.TokenManagerDisabled)
            {
                var builder = securityHeadersBuilder ??
                              new SecurityHeadersBuilder().AddDefaultSecurePolicy();

                var policy = builder.Build();

                return app.UseMiddleware<TokenManagerMiddleware>(policy);
            }

            return app;
        }
}
