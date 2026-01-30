using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Requests;

public class BlogLikeRequest : BlogLikeDto
{
    public Guid? Id { get; set; }
}
