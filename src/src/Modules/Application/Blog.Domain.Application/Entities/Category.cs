using System;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Category : BaseEntityWithAudit
{
    public required string Name { get; set; }
    public string? Slug { get; set; }
    public ICollection<BlogEntity>? Blogs { get; set; }
}
