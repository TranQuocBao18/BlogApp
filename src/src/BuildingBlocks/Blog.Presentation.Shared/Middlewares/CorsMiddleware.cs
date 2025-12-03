using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Blog.Presentation.Shared.Middlewares;

public class CorsMiddleware
{
    private readonly RequestDelegate _next;

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return _next(httpContext);
    }
}

public static class CorsMiddlewareExtensions
{
    public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsMiddleware>();
    }
}
