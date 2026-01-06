using System.Security.Claims;
using System.Text.Json.Serialization;
using Blog.Presentation.Application.Extensions;
using Blog.Presentation.Communication.Extensions;
using Blog.Presentation.Identity.Extensions;
using Blog.Presentation.Shared.Extensions;
using Blog.Presentation.Shared.Helpers;
using Blog.Presentation.Shared.Options;
using Blog.SignalR.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

var AllowSpecificOrigins = "_AllowSpecificOrigins";
var configuration = builder.Configuration;
var environment = builder.Environment;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    options.AddPolicy(name: AllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.Configure<CookieAuthenticationOptions>(o =>
{
    o.LoginPath = PathString.Empty;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddIdentityExtension(configuration);
builder.Services.AddApplicationExtension(configuration);
builder.Services.AddCommunicationExtension(configuration);

builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHealthChecks();
builder.Services.AddOptions();
builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(o =>
{
    o.ViewLocationFormats.Clear();
    o.ViewLocationFormats.Add("~/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
    o.ViewLocationFormats.Add("~/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
});

builder.Services.AddSession();
builder.Services.Configure<AppSettingOption>(configuration.GetSection("AppSetting"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseRouting();

// app.UseStaticFiles();

app.UseCors(corsPolicyBuilder =>
   corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
);
app.UseCors(AllowSpecificOrigins);

app.UseSession();
app.UseAuthentication();
app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("JWToken");

    if (!string.IsNullOrEmpty(JWToken))
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Request.Headers["Authorization"] = "Bearer " + JWToken;
        }
        else
        {
            context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
        }

        if (JwtTokenHelper.IsAuthenticated(context.Session))
        {
            var appIdentity = new ClaimsIdentity(JwtTokenHelper.Claims);
            context.User = new ClaimsPrincipal(appIdentity);
        }
    }
    await next();
});

app.UseAuthorization();
app.UseSwaggerExtension();
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");

app.UseNotificationSignalR(environment, configuration);

app.MapControllers().RequireCors(AllowSpecificOrigins);

app.Run();
