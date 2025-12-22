using System;
using System.ComponentModel.DataAnnotations;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Banner : BaseEntityWithAudit
{
    [Key]
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual string? CreatedBy { get; set; }
}
