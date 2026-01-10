using System;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Responses;

public class CommentResponse : CommentDto
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public int ReplyCount { get; set; }
    public bool? IsLikeByCurrentUser { get; set; }
    public ICollection<CommentResponse> ChildComments { get; set; } = new List<CommentResponse>();
}
