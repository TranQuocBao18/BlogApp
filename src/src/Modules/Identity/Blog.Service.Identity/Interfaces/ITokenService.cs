using System;
using Blog.Domain.Identity.Entities;

namespace Blog.Service.Identity.Interfaces;

public interface ITokenService
{
    RefreshToken GenerateRefreshToken(string ipAddress);

    Task<string> GenerateJWToken(ApplicationUser user);
}
