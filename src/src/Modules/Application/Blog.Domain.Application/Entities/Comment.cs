using System;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Comment : BaseEntityWithAudit
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public Guid ParentId { get; set; }
    public required string Content { get; set; }
    public virtual Blog? Blog { get; set; }
    public virtual User? User { get; set; }
    public virtual Comment? Parent { get; set; }

}
