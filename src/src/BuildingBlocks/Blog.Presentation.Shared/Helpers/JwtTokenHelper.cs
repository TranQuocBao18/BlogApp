using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blog.Domain.Shared.Enums;
using Blog.Infrastructure.Shared.Constants;
using Blog.Model.Dto.Shared.Dtos;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;

namespace Blog.Presentation.Shared.Helpers;

public static class JwtTokenHelper
{
    private static IJsonSerializer _serializer = new JsonNetSerializer();
    private static IDateTimeProvider _provider = new UtcDateTimeProvider();
    private static IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
    private static IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

    public static string Email { get; set; }
    public static string UserId { get; private set; }
    public static string IpAddressClient { get; set; }
    public static DateTime ExpiredTime { get; set; }
    public static List<RoleEnums> Roles { get; set; } = new List<RoleEnums>();
    public static List<string> Permission { get; set; } = new List<string>();
    public static List<Claim> Claims { get; private set; } = new List<Claim>();

    public static bool IsAuthenticated(ISession session)
    {
        try
        {
            var accessToken = session.GetString("JWToken");
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return false;
            }

            IJwtValidator _validator = new JwtValidator(_serializer, _provider);
            IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);

            var token = decoder.DecodeToObject<JwtToken>(accessToken);
            // Information user
            UserId = token.uid;
            Email = token.email;
            IpAddressClient = token.ip;

            var tokenRaw = TokenParser(accessToken);
            Claims = tokenRaw.Claims.ToList();
            var countRoles = tokenRaw.Claims.Count(x => x.Type == IdentityConstant.Roles);
            if (countRoles > 1) // List Roles => model List<string>
            {
                var tokenWithAuthorization = decoder.DecodeToObject<JwtTokenWithRoles>(accessToken);
                if (tokenWithAuthorization == null)
                {
                    return false;
                }

                var roles = tokenWithAuthorization.roles?.ToList();
                if (roles != null && roles.Any())
                {
                    Roles = roles.Select(x => (RoleEnums)Enum.Parse(typeof(RoleEnums), x)).ToList();
                }
            }
            else if (countRoles == 1)
            {
                var tokenWithAuthorization = decoder.DecodeToObject<JwtTokenWithRole>(accessToken);
                if (tokenWithAuthorization == null)
                {
                    return false;
                }

                Roles = new List<RoleEnums> { (RoleEnums)Enum.Parse(typeof(RoleEnums), tokenWithAuthorization.roles) };
            }

            // var countPermissions = tokenRaw.Claims.Count(x => x.Type == IdentityConstant.Permission);
            // if (countPermissions > 1)
            // {
            //     var tokenWithAuthorization = decoder.DecodeToObject<JwtTokenWithPermissions>(accessToken);
            //     if (tokenWithAuthorization == null)
            //     {
            //         return false;
            //     }

            //     Permission = tokenWithAuthorization.permissions?.ToList();
            // }
            // else if (countPermissions == 1)
            // {
            //     var tokenWithAuthorization = decoder.DecodeToObject<JwtTokenWithPermission>(accessToken);
            //     if (tokenWithAuthorization == null)
            //     {
            //         return false;
            //     }

            //     Permission = new List<string> { tokenWithAuthorization.permissions };
            // }

            // ExpiredTime
            var expiredTime = token.exp;
            // Convert ExpiredTime
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expiredTime);
            ExpiredTime = dateTimeOffset.LocalDateTime;

            // Validate token expired
            var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (currentTime > expiredTime)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private static JwtSecurityToken TokenParser(string streamToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(streamToken);
        var token = jsonToken as JwtSecurityToken;
        return token;
    }

    private static DateTime GetExpiryTimestamp(string accessToken)
    {
        try
        {
            IJwtValidator _validator = new JwtValidator(_serializer, _provider);
            IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
            var token = decoder.DecodeToObject<JwtToken>(accessToken);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
            return dateTimeOffset.LocalDateTime;
        }
        catch (TokenExpiredException)
        {
            return DateTime.MinValue;
        }
        catch (SignatureVerificationException)
        {
            return DateTime.MinValue;
        }
        catch (Exception)
        {
            // ... remember to handle the generic exception ...
            return DateTime.MinValue;
        }
    }
}
