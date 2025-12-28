using System;
using System.ComponentModel.DataAnnotations;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class BlogLike : BaseEntityWithAudit
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public virtual BlogEntity Blog { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
