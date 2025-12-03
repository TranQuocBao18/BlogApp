using System;
using Blog.Infrastructure.Identity;
using Blog.Infrastructure.Shared;

using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Services;
using Blog.Presentation.Shared.Extensions;
using Blog.Service.Identity.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Identity.Extensions;

public static class IdentityPresentationExtension
{
    public static void AddIdentityExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

        services.AddIdentityAuthenticationLayer(configuration);

        services.AddInfrastructureLayer(configuration);
        services.AddSharedInfrastructure(configuration);
        services.AddServiceLayer();
        // services.AddOperationBuilderServices();

        services.AddSwaggerExtension();
        services.AddApiVersioningExtension();
    }
}
