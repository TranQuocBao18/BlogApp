using System;
using System.Reflection;
using Blog.EventBus.Configurations;
using Blog.Utilities.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.EventBus.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration, Assembly? consumerAssembly = null)
    {
        var settings = configuration.GetOptionsExt<RabbitMqSettings>("RabbitMq");

        services.AddMassTransit(x =>
        {
            if (consumerAssembly != null)
            {
                x.AddConsumers(consumerAssembly);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(settings.Host, "/", h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
