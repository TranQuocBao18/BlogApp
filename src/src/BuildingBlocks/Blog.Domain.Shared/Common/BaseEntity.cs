using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Shared.Common;

public abstract class BaseEntity : IEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    public virtual bool IsDeleted { get; set; }
}
