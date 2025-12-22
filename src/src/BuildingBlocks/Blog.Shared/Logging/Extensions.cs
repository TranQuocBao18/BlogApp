using System;
using Microsoft.Extensions.Hosting;
using Serilog;
using Blog.Utilities.Extensions;
using Blog.Shared.Types;
using Blog.Shared.Logging.Enrichers;

namespace Blog.Shared.Logging;

public static class Extensions
{
    public static IHostBuilder UseLoggingExt(this IHostBuilder hostBuilder, string applicationName = "")
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            var appOptions = context.Configuration.GetOptions<AppOptions>("App");
            var loggingOptions = context.Configuration.GetOptions<LoggingOptions>("Logging");
            applicationName = appOptions?.Name ?? applicationName;
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration, "Logging")
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", appOptions.Name)
                .Enrich.With<OpenTracingContextLogEventEnricher>();

            Configure(loggerConfiguration, loggingOptions.Seq, loggingOptions);
        });

        return hostBuilder;
    }

    public static void Configure(LoggerConfiguration loggerConfiguration, SeqOptions seq, LoggingOptions loggingOptions)
    {
        if (seq.Enabled)
        {
            loggerConfiguration.WriteTo.Seq(seq.Url, apiKey: seq.ApiKey);
        }

        if (loggingOptions.ConsoleEnabled)
        {
            loggerConfiguration.WriteTo
                .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Properties:j}] {Message:lj}{NewLine}{Exception}");
        }
    }
}
