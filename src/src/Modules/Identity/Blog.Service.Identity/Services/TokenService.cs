using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Settings;
using Blog.Infrastructure.Identity.Helpers;
using Blog.Infrastructure.Identity.Interfaces;
using Blog.Service.Identity.Interfaces;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Blog.Service.Identity.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<SystemRole> _roleManager;
    private readonly IRoleRepository _roleRepository;
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings, RoleManager<SystemRole> roleManager, UserManager<ApplicationUser> userManager, IRoleRepository roleRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _roleManager = roleManager;
        _userManager = userManager;
        _roleRepository = roleRepository;
    }

    public async Task<string> GenerateJWToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id.AsGuid());

        var roleClaims = new List<Claim>();
        
        string ipAddress = IpHelper.GetIpAddress();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
            new Claim("ip", ipAddress),
            new Claim("role", roles.FirstOrDefault()?.RoleName ?? string.Empty),
            new Claim("group", roles.FirstOrDefault()?.Code ?? string.Empty),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        // convert random bytes to hex string
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}
