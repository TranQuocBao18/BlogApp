using System;
using System.ComponentModel.DataAnnotations;
using Blog.Domain.Identity.Entities;

namespace Blog.Domain.Application.Entities;

public class Like
{
    [Key]
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual string? CreatedBy { get; set; }
    public virtual Blog? Blog { get; set; }
    public virtual User? User { get; set; }
    public virtual Comment? Comment { get; set; }
}
