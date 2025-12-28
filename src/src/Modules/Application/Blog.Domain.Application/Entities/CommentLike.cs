using System;
using System.ComponentModel.DataAnnotations;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class CommentLike : BaseEntityWithAudit
{
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public virtual Comment Comment { get; set; } = null!;
    public virtual User? User { get; set; } = null!;
}
