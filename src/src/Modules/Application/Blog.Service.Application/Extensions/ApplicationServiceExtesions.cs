using System;
using System.Reflection;
using Blog.Files.Interfaces;
using Blog.Files.Services;
using Blog.Infrastructure.Shared.Behaviours;
using Blog.Service.Application.Interfaces;
using Blog.Service.Application.Services;
using Blog.Shared.Auth;
using CloudinaryDotNet;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Service.Application.Extensions;

public static class ApplicationServiceExtesions
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
        services.AddScoped<IFileService>(sp => new FileService(null));
        services.AddScoped<IFileScannerService, FileScannerService>();
        services.AddSingleton<Cloudinary>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );
            return new Cloudinary(account);
        });

        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IBlogTagService, BlogTagService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IBlogLikeService, BlogLikeService>();
        services.AddScoped<ICommentLikeService, CommentLikeService>();
        services.AddScoped<ITagService, TagService>();
    }
}
