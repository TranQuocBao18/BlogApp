using System;

namespace Blog.Domain.Identity.Dtos;

public class GroupUserDto
{
    public Guid GroupId { get; set; }
    public IList<Guid> UserIds { get; set; } = [];
}
