using System;

namespace Blog.Infrastructure.Shared.Interfaces;

public interface IAuthenticatedUserService
{
    string UserId { get; }
}
