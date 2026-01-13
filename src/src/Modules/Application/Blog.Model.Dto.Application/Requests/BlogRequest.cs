using System;
using Blog.Domain.Application.Enum;
using Blog.Model.Dto.Application.Dtos;

namespace Blog.Model.Dto.Application.Requests;

public class BlogRequest
{
    public Guid? Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? BannerId { get; set; }

    // Basic info
    public required string Title { get; set; }
    public required string Content { get; set; }
    public string? Slug { get; set; }
    public string? MetaDescription { get; set; }
    public BlogStatus Status { get; set; }

    // Optional: Tags
    public List<Guid>? TagIds { get; set; }
}
