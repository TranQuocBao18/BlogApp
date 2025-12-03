using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Blog.Presentation.Shared.Extensions;

public static class ServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // c.IncludeXmlComments(string.Format(@"{0}Ntech.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Tran Bao Blog API",
                Description = "This Api will be responsible for overall data distribution and authorization.",
                Contact = new OpenApiContact
                {
                    Name = "tranbao",
                    Email = "tqbao251002",
                    Url = new Uri("https://gmail.com"),
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
            });
        });
    }

    public static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            // Specify the default API Version as 1.0
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // If the client hasn't specified the API version in the request, use the default API version number 
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
        });
    }

    public static void AddOpenTelemetryExtension(this IServiceCollection services, IWebHostEnvironment environment, ConfigurationManager configuration, ILoggingBuilder logging)
    {
        services.Configure<OtlpExporterOptions>(o => o.Headers = $"x-otlp-api-key=SecretKey@1234");
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            Assembly.GetEntryAssembly()?.GetName().Name ?? "Ntech.Presentation.Shared",
            serviceVersion: Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
        logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });
        services
            .AddMetrics()
            .AddOpenTelemetry()
            .ConfigureResource(c => c.AddService("Ntech.Presentation.Shared"))
            .WithMetrics(provider =>
            {
                provider.SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();
                provider.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel", "System.Net.Http", "Ntech.Presentation.Shared");
            })
            .WithTracing(options =>
            {
                if (environment.IsDevelopment())
                {
                    options.SetSampler<AlwaysOnSampler>();
                }
                options.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
            });
        if (!string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            services.Configure<OpenTelemetryLoggerOptions>(options => options.AddOtlpExporter())
                .ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter())
                .ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }
    }
}
