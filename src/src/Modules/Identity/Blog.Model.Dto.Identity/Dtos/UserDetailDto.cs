using System;

namespace Blog.Model.Dto.Identity.Dtos;

public class UserDetailDto
{
    public bool PhoneNumberConfirmed { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsLocked { get; set; }
    public List<string>? Roles { get; set; }
}
