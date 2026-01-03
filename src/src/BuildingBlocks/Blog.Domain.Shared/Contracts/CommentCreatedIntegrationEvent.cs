using System;

namespace Blog.Domain.Shared.Contracts;

public class CommentCreatedIntegrationEvent
{
    public Guid BlogId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ParentId { get; set; }
    public string Content { get; set; }
}
