using System;
using System.Reflection;
using Blog.Infrastructure.Shared.Behaviours;
using Blog.Shared.Auth;
using Blog.Shared.Notification;
using Blog.SignalR.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Service.Communication.Extensions;

public static class CommunicationServiceExtensions
{
    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();
        services.AddHttpContextAccessor();
        services.AddAuthorization();

        services.AddHttpClient();

        // services.AddScoped<IUserNotificationService, NotificationService>();
        // services.AddScoped<ICommunicationService, CommunicationService>();
        // services.AddScoped<INotificationService, NotificationService>();
    }
}
