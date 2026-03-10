using System;

namespace Blog.Domain.Identity.Requests;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
