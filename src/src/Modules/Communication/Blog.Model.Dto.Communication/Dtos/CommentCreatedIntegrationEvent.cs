using System;

namespace Blog.Model.Dto.Communication.Dtos;

public class CommentCreatedIntegrationEvent
{
    public Guid? BlogAuthorId { get; set; }
    public Guid BlogId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ParentId { get; set; }
    public string AuthorName { get; set; } = default!;
    public string Content { get; set; } = default!;
}
