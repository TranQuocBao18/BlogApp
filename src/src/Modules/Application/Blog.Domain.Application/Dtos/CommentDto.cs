using System;
using Blog.Domain.Identity.Entities;

namespace Blog.Domain.Application.Dtos;

public class CommentDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public required string Content { get; set; }
    public virtual User? User { get; set; }

}
