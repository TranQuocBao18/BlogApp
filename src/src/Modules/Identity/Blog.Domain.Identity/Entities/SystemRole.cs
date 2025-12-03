using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Identity.Entities;

public class SystemRole : IdentityRole
{
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool CanDeleted { get; set; } = false;
    public string? CreatedBy { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; } = DateTime.Now;
}
