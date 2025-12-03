using System;

namespace Blog.Model.Dto.Identity.Requests;

public class CreateAccountRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
