using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Responses;

public class CommentLikeResponse : CommentLikeDto
{
    public Guid Id { get; set; }
}
