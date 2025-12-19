using System;
using Blog.Domain.Shared.Common;

namespace Blog.Domain.Application.Entities;

public class Tag : BaseEntityWithAudit
{
    public required string Name { get; set; }
}
