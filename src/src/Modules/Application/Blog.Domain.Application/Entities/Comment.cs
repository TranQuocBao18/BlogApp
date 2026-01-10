using System;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Comment : BaseEntityWithAudit
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public required string Content { get; set; }
    public int LikeCount { get; set; } = 0;
    public virtual BlogEntity? Blog { get; set; }
    public virtual User? User { get; set; }
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> ChildComments { get; set; } = new List<Comment>();
    public virtual ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
}
