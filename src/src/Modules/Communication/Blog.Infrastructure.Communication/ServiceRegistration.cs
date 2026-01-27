using System;
using Blog.Domain.Shared.Contracts;
using Blog.EventBus.Extensions;
using Blog.Infrastructure.Communication.Consumer;
using Blog.Infrastructure.Communication.Contexts;
using Blog.Infrastructure.Communication.Interfaces;
using Blog.Infrastructure.Communication.Repositories;
using Blog.Infrastructure.Communication.UnitOfWorks;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Blog.Infrastructure.Shared.Services;
using Blog.SignalR.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Communication;

public static class ServiceRegistration
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        if (configuration.GetValue<bool>("UseInMemomryDatabase"))
        {
            services.AddDbContext<CommunicationDbContext>(options => options.UseInMemoryDatabase("CommunicationDb"));
        }
        else
        {
            services.AddDbContext<CommunicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("communication"), b => b.MigrationsAssembly(typeof(CommunicationDbContext).Assembly.FullName)));
        }

        services.AddTransient<ICommunicationUnitOfWork, CommunicationUnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepositoryAsync<,>));
        services.AddTransient<INotificationMessageRepository, NotificationMessageRepository>();

        services.AddMessaging(configuration, typeof(ICommunicationServiceMarker).Assembly);
        services.AddNotificationSignalR(configuration);
    }
}
