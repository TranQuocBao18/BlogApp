using System;
using Blog.Presentation.Shared.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Blog.Presentation.Shared.Extensions;

public static class AppExtension
{
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.Ntech.Presentation.Shared");
        });
    }

    public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
