using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Identity.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Code { get; set; }
    public bool IsDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; } = DateTime.UtcNow;
    public virtual List<RefreshToken>? RefreshTokens { get; set; }
}
