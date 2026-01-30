using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Responses;

public class BlogLikeResponse : BlogLikeDto
{
    public Guid Id { get; set; }
}
