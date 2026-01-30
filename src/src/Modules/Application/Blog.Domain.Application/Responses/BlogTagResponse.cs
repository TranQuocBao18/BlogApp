using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Responses;

public class BlogTagResponse : BlogTagDto
{
    public Guid Id { get; set; }
}
