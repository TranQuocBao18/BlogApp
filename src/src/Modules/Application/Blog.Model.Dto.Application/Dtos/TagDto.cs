using System;

namespace Blog.Model.Dto.Application.Dtos;

public class TagDto
{
    public required string Name { get; set; }
    public string? Slug { get; set; }
}
