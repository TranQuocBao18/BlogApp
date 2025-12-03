using System;
using System.Text.Json.Serialization;

namespace Blog.Model.Dto.Identity.Responses;

public class AuthenticationResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string GroupCode { get; set; }
    public List<string> Roles { get; set; }
    public bool IsVerified { get; set; }
    public string JWToken { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
}
