using System;
using System.Reflection;
using Blog.EventBus.Configurations;
using Blog.Utilities.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blog.EventBus.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration, Assembly? consumerAssembly = null)
    {
        var settings = configuration.GetOptionsExt<RabbitMqSettings>("RabbitMq");

        var logger = services.BuildServiceProvider().GetService<ILogger>();
        logger?.LogInformation($"[MassTransit] Configuring RabbitMQ: Host={settings.Host}, VirtualHost={settings.VirtualHost}, Username={settings.Username}");

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            if (consumerAssembly != null)
            {
                logger?.LogInformation($"[MassTransit] Adding consumers from assembly: {consumerAssembly.FullName}");
                x.AddConsumers(consumerAssembly);

                // Log all found consumers
                var consumerTypes = consumerAssembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition().Name.Contains("IConsumer")))
                    .ToList();

                foreach (var consumerType in consumerTypes)
                {
                    logger?.LogInformation($"[MassTransit] Found consumer: {consumerType.FullName}");
                }
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                logger?.LogInformation("[MassTransit] Connecting to RabbitMQ...");

                cfg.Host(settings.Host, settings.VirtualHost, h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(context);
                logger?.LogInformation("[MassTransit] RabbitMQ configured successfully");
            });
        });

        logger?.LogInformation("[MassTransit] MassTransit setup completed");

        return services;
    }
}
