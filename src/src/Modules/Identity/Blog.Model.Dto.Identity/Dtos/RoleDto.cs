using System;

namespace Blog.Model.Dto.Identity.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;
    public string? CreatedBy { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; } = DateTime.Now;
}
