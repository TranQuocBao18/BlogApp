using System;
using Blog.Shared.Caching;
using Blog.SignalR.Hubs;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.SignalR.Extensions;

public static class SignalRExtension
{
    public static IServiceCollection AddNotificationSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        var signalRSetting = configuration.GetOptionsExt<SignalRSettingOptions>(SignalRSettingOptions.SettingKey);
        var signalRBuilder = services.AddCustomSignalR(options =>
        {
            options.AllowOrigins = signalRSetting.Origins;
            options.GetUserIdFunc = (hubContext) =>
            {
                return hubContext.User?.FindFirst("uid")?.Value?.Trim();
            };
        });
        // if (signalRSetting.IsBackplaneEnabled)
        // {
        //     signalRBuilder.AddRedisBackplane(configuration);
        // }

        services.Configure<SignalRSettingOptions>(configuration.GetSection(SignalRSettingOptions.SettingKey));
        return services;
    }

    // public static ISignalRServerBuilder AddRedisBackplane(this ISignalRServerBuilder signalRServerBuilder, IConfiguration configuration)
    // {
    //     var redisCacheOptions = configuration.GetOptionsExt<CacheOptions>("Redis");
    //     if (redisCacheOptions != null && redisCacheOptions.Enabled)
    //     {
    //         signalRServerBuilder.AddRedisBackplane(configuration);
    //     }
    //     return signalRServerBuilder;
    // }

    private const string NOTIFICATION_HUB_PATH = "/api/hubs";

    public static IApplicationBuilder UseNotificationSignalR(this IApplicationBuilder app, IWebHostEnvironment environment, IConfiguration configuration)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<CommonHub>(NOTIFICATION_HUB_PATH, options =>
            {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents;
            });

            endpoints.MapHub<CommonHub>(NOTIFICATION_HUB_PATH + "/unauth", options =>
            {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents;
            });
        });
        return app;
    }

    public static void HandleSignalRAuthorizeToken(this JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                var request = context.HttpContext.Request;
                var path = request.Path;

                var accessToken = request.Query["access_token"];

                if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/api/hubs"))
                {
                    context.Token = accessToken;
                }
                else
                {
                    var authHeader = request.Headers["Authorization"].ToString();
                    if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                }

                await Task.CompletedTask;
            }
        };
    }
}
