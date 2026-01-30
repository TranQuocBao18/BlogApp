using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Responses;

public class BlogResponse : BlogDto
{
    public Guid Id { get; set; }
}
