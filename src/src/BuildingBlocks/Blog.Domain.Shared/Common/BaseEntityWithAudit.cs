using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Shared.Common;

public abstract class BaseEntityWithAudit : IEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }

    public virtual bool IsDeleted { get; set; }
    public virtual string? CreatedBy { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual string? LastModifiedBy { get; set; }
    public virtual DateTime? LastModified { get; set; }
}
