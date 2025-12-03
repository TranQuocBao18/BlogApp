using System;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Domain.Shared.Common;
using Blog.Domain.Shared.Enums;

namespace Blog.Domain.Identity.Entities;

public class User : AuditableBaseEntity
{
    public string Username { get; set; }
    public string? Code { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsLocked { get; set; }
    public List<RoleEnums> Roles { get; set; }

    [NotMapped]
    public bool IsAdmin { get; set; }
}
