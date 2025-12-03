using System;

namespace Blog.Model.Dto.Identity.Requests;

public class RoleRequest
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public string? Code { get; set; }
}
