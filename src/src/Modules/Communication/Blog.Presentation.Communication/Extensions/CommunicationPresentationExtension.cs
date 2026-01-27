using System;
using Blog.Infrastructure.Communication;
using Blog.Service.Communication.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Communication.Extensions;

public static class CommunicationPresentationExtension
{
    public static void AddCommunicationExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureLayer(configuration);
        services.AddServiceLayer();
    }
}
