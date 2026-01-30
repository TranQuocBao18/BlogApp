using System;

namespace Blog.Domain.Identity.Requests;

public class RoleRequest
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public string? Code { get; set; }
}
