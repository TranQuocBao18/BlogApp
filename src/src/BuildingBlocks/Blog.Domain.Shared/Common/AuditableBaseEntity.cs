using System;

namespace Blog.Domain.Shared.Common;

public abstract class AuditableBaseEntity : BaseEntity, IEntity<Guid>
{
    public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
}
