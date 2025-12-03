using System;
using Serilog.Core;
using Serilog.Events;
using OpenTracing.Util;

namespace Blog.Shared.Logging.Enrichers;

public class OpenTracingContextLogEventEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var tracer = GlobalTracer.Instance;
            if (tracer?.ActiveSpan == null)
            {
                return;
            }
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("TraceId", tracer.ActiveSpan.Context.TraceId));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SpanId", tracer.ActiveSpan.Context.SpanId));
        }
}
