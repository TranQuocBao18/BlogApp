using System;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Tag : BaseEntityWithAudit
{
    public required string Name { get; set; }
    public string? Slug { get; set; }
    public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
}
