using System;

namespace Blog.Model.Dto.Application.Dtos;

public class BlogDto
{
    public Guid CategoryId { get; set; }
    public Guid BannerId { get; set; }
    public required string Tittle { get; set; }
    public required string Content { get; set; }
    public string? Slug { get; set; }
}
