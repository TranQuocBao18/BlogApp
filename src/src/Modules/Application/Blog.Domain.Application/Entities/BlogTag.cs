using System;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class BlogTag : BaseEntityWithAudit
{
    public Guid BlogId { get; set; }
    public Guid TagId { get; set; }
    public virtual BlogEntity? Blog { get; set; }
    public virtual Tag? Tag { get; set; }
}
