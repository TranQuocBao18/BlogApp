using System;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Requests;

public class BlogTagRequest : BlogTagDto
{
    public Guid? Id { get; set; }
}
