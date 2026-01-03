using System;

namespace Blog.EventBus.Abstractions;

public interface IIntegrationEvent
{
    public record IntegrationEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
