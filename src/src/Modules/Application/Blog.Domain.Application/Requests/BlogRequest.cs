using System;
using Blog.Domain.Application.Enum;
using Blog.Domain.Application.Dtos;

namespace Blog.Domain.Application.Requests;

public class BlogRequest
{
    public Guid? Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? BannerId { get; set; }

    // Basic info
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? Slug { get; set; }
    public string? MetaDescription { get; set; }
    public BlogStatus Status { get; set; }

    // Optional: Tags
    public List<Guid>? TagIds { get; set; }
}
