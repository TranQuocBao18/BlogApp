using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Requests;

public class CommentLikeRequest : CommentLikeDto
{
    public Guid? Id { get; set; }
}
