using System;

namespace Blog.Shared.Auth;

public interface ISecurityContextAccessor
{
    Guid UserId { get; }
    string Email { get; }
    string Role { get; }
    List<string> Roles { get; }
    string JwtToken { get; }
    string IPAddressClient { get; }
    bool IsAuthenticated { get; }
}
