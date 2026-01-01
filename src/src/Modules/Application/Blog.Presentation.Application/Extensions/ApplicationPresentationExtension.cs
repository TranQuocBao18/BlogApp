using System;
using Blog.Infrastructure.Application;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Services;
using Blog.Service.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Application.Extensions;

public static class ApplicationPresentationExtension
{
    public static void AddApplicationExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

        services.AddServiceLayer();
        services.AddInfrastructureLayer(configuration);
    }
}
