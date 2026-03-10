using System;

namespace Blog.Domain.Identity.Requests;

public class LogoutRequest
{
    public Guid UserId { get; set; }
    public string IPAddress { get; set; } = string.Empty;
}
