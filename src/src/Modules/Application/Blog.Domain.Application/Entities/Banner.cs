using System;
using System.ComponentModel.DataAnnotations;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Banner : BaseEntityWithAudit
{
    public required string PublicId { get; set; }
    public required string Url { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public string ETag { get; set; } = default!;
}
