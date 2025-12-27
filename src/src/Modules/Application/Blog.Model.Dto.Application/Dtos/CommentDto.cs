using System;

namespace Blog.Model.Dto.Application.Dtos;

public class CommentDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public required string Content { get; set; }
}
