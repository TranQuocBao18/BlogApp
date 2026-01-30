using System;

namespace Blog.Domain.Application.Dtos;

public class BlogTagDto
{
    public Guid BlogId { get; set; }
    public Guid TagId { get; set; }
}
