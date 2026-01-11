using System;

namespace Blog.EventBus.Configurations;

public class RabbitMqSettings
{
    public string Host { get; set; } = "localhost";
    public string VirtualHost { get; set; } = "guest";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
}
