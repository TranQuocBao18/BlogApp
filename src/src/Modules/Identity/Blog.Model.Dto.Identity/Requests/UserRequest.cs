using System;

namespace Blog.Model.Dto.Identity.Requests;

public class UserRequest
{
    public Guid? Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string Fullname { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public bool? IsAdmin { get; set; }
}
