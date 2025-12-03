using System;
using System.Reflection;
using System.Text;
using Blog.Domain.Shared.Settings;
using Blog.Infrastructure.Shared.Behaviours;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using Blog.Service.Identity.Services;
using Blog.Shared.Auth;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Blog.Service.Identity.Extensions;

public static class IdentityServiceExtensions
{
    public static void AddIdentityAuthenticationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JWTSettings"));
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            var keyJwt = configuration["JWTSettings:Key"];
            o.RequireHttpsMetadata = true;
            o.SaveToken = true;
            o.IncludeErrorDetails = true;

            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidIssuer = configuration["JWTSettings:Issuer"],
                ValidAudience = configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyJwt)),
                RequireSignedTokens = false,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                SaveSigninToken = true,
                ClockSkew = TimeSpan.Zero
            };
            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    var result = JsonConvert.SerializeObject(new Response<string>(context.Response.StatusCode.ToString(), "Error Server"));
                    return context.Response.WriteAsync(result);
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    var result = JsonConvert.SerializeObject(new Response<string>(context.Response.StatusCode.ToString(), "You are not Authorized"));
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    var result = JsonConvert.SerializeObject(new Response<string>(context.Response.StatusCode.ToString(), "You are not authorized to access this resource"));
                    return context.Response.WriteAsync(result);
                },
            };
        });
    }

    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();
        services.AddHttpContextAccessor();
        //services.AddAuthorization();

        services.AddTransient<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<INotificationPreparationService, NotificationPreparationService>();
        services.AddScoped<IUserServiceShare, UserService>();
    }
}
