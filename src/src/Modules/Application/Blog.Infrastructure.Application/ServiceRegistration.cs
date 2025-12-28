using System;
using Blog.Infrastructure.Application.Context;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Application.Repositories;
using Blog.Infrastructure.Application.UnitOfWorks;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Repositories.Common;
using Blog.Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Application;

public static class ServiceRegistration
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("UserDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("application"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("identity"), b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName)));
        }

        services.AddTransient<IApplicationUnitOfWork, ApplicationUnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepositoryAsync<,>));
        services.AddTransient<IBannerRepository, BannerRepository>();
        services.AddTransient<IBlogRepository, BlogRepository>();
        services.AddTransient<IBlogTagRepository, BlogTagRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IBlogLikeRepository, BlogLikeRepository>();
        services.AddTransient<ICommentLikeRepository, CommentLikeRepository>();
        services.AddTransient<ITagRepository, TagRepository>();
    }
}
