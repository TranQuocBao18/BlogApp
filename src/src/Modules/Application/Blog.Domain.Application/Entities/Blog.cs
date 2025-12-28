using System;
using Blog.Domain.Application.Enum;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class BlogEntity : BaseEntityWithAudit
{
    public Guid CategoryId { get; set; }
    public Guid? BannerId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public string? Slug { get; set; }
    public BlogStatus Status { get; set; }
    public int LikeCount { get; set; } = 0;
    public virtual Banner? Banner { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
    public virtual ICollection<BlogLike> Likes { get; set; } = new List<BlogLike>();
}
