using System;

namespace Blog.Domain.Identity.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public string? Code { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? CreatedBy { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; } = DateTime.Now;
}
