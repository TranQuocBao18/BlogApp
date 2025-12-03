using System;

namespace Blog.Model.Dto.Shared.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual DateTime? LastModified { get; set; }
}
