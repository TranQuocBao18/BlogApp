using System;

namespace Blog.Domain.Application.Dtos;

public class TagDto
{
    public required string Name { get; set; }
    public string? Slug { get; set; }
}
