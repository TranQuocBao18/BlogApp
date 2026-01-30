using System;

namespace Blog.Domain.Application.Dtos;

public class CategoryDto
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public string? Slug { get; set; }
}
