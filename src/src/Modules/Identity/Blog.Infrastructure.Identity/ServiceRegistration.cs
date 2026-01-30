using System;
using Blog.Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Identity.Interfaces;
using Blog.Infrastructure.Identity.Repositories;
using Blog.Infrastructure.Identity.UnitOfWorks;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;

namespace Blog.Infrastructure.Identity;

public static class ServiceRegistration
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseInMemoryDatabase("IdentityDb"), ServiceLifetime.Transient);
        }
        else
        {
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("identity"),
                b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)
            ), ServiceLifetime.Transient);
        }

        var optionLockout = configuration.GetOptions<LockoutOptions>("IdentityServiceOptions:Lockout");
        services.AddIdentity<ApplicationUser, SystemRole>(x =>
                {
                    x.Lockout = optionLockout;
                })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepositoryAsync<,>));
    }
}
