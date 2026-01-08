using System;

namespace Blog.Domain.Shared.Contracts;

public class LikeCreatedIntegrationEvent
{
    public Guid BlogId { get; set; }
    public Guid? BlogAuthorId { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = default!;
}
