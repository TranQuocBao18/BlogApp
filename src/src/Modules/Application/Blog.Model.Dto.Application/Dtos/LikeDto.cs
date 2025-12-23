using System;

namespace Blog.Model.Dto.Application.Dtos;

public class LikeDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
}
