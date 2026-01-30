using System;

namespace Blog.Domain.Identity.Requests;

public class CreateAccountRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
