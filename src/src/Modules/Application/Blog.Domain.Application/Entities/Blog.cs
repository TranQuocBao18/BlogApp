using System;
using Blog.Domain.Application.Enum;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class BlogEntity : BaseEntityWithAudit
{
    public Guid CategoryId { get; set; }
    public Guid BannerId { get; set; }
    public required string Tittle { get; set; }
    public required string Content { get; set; }
    public string? Slug { get; set; }
    public BlogStatus Status { get; set; }
    public virtual Banner? Banner { get; set; }

}
