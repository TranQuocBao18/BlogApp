using System;

namespace Blog.Domain.Shared.Contracts;

public class CommentLikeCreatedIntegrationEvent
{
    public Guid BlogId { get; set; }
    public Guid? CommentAuthorId { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = default!;

}
