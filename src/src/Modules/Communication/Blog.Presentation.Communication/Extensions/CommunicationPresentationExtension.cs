using System;
using Blog.EventBus.Extensions;
using Blog.Infrastructure.Communication;
using Blog.Presentation.Communication.Consumer;
using Blog.Service.Communication.Extensions;
using Blog.SignalR.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Presentation.Communication.Extensions;

public static class CommunicationPresentationExtension
{
    public static void AddCommunicationExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureLayer(configuration);
        services.AddServiceLayer();

        services.AddMessaging(configuration, typeof(ICommunicationServiceMarker).Assembly);
        services.AddNotificationSignalR(configuration);
    }
}
