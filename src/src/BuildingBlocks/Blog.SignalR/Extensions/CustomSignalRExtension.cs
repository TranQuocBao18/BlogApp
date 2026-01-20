using System;
using Blog.Shared.Auth;
using Blog.Shared.Notification;
using Blog.SignalR.Core;
using Blog.SignalR.Core.Stores;
using Blog.SignalR.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blog.SignalR.Extensions;

public static class CustomSignalRExtension
{
    private const string SIGNALRCORS_POLICY = "SignalRCorsPolicy";
    public static ISignalRServerBuilder AddCustomSignalR(this IServiceCollection services, Action<SignalROptions> optionsAction = null)
    {
        var signalROptions = new SignalROptions();
        optionsAction?.Invoke(signalROptions);

        services.AddCors(options => options.AddPolicy(SIGNALRCORS_POLICY, builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                // .WithOrigins(signalROptions.AllowOrigins.Split(";"))
                .AllowCredentials();

            if (signalROptions.AllowOrigins == "*")
            {
                builder.SetIsOriginAllowed(host => true);
            }
            else
            {
                builder.WithOrigins(signalROptions.AllowOrigins.Split(";"));
            }
        }));

        //Use to set HubCallerContext.UserIdentifier from ClaimTypes.NameIdentifier in DefaultUserIdProvider
        //so Implement IUserIdProvider to change
        if (signalROptions.GetUserIdFunc != null)
        {
            services.AddSingleton(new Func<IServiceProvider, IUserIdProvider>((sp) =>
            {
                return new CustomUserIdProvider(signalROptions.GetUserIdFunc);
            }));
        }

        var signalrBuilder = services
            .AddScoped<IClientInfoProvider, HttpContextClientInfoProvider>()
            .AddScoped<IOnlineClientProvider, OnlineClientProvider>()
            //singleton
            .AddSingleton<IOnlineClientStore, InMemoryOnlineClientStore>()
            .AddSingleton<IOnlineClientManager, OnlineClientManager>()
            .AddSingleton<IRealTimeNotifier, SignalRRealTimeNotifier>()
            .AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

        services.TryAddSingleton<IUserGroupManager, DefaultUserGroupManager>();

        return signalrBuilder;
    }
}

public class CustomUserIdProvider : IUserIdProvider
{
    private readonly Func<HubConnectionContext, string> _func;
    public CustomUserIdProvider(Func<HubConnectionContext, string> func)
    {
        _func = func;
    }

    public string GetUserId(HubConnectionContext connection)
    {
        return _func(connection);
    }
}
