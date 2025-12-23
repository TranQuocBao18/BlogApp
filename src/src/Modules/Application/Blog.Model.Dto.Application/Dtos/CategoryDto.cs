using System;

namespace Blog.Model.Dto.Application.Dtos;

public class CategoryDto
{
    public required string Name { get; set; }
    public string? Slug { get; set; }
}
