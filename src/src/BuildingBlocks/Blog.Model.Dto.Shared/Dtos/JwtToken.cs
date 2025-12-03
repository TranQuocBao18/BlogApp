using System;

namespace Blog.Model.Dto.Shared.Dtos;

public class JwtToken
{
    public string jti { get; set; }
    public string email { get; set; }
    public string uid { get; set; }
    public string ip { get; set; }
    public string iss { get; set; }
    public string aud { get; set; }
    public long exp { get; set; }
}

public class JwtTokenWithRoles : JwtToken
    {
        public List<string> roles { get; set; } = new List<string>();
    }

    public class JwtTokenWithRole : JwtToken
    {
        public string roles { get; set; }
    }

    public class JwtTokenWithPermissions : JwtToken
    {
        public List<string> permissions { get; set; } = new List<string>();
    }

    public class JwtTokenWithPermission : JwtToken
    {
        public string permissions { get; set; }
    }
