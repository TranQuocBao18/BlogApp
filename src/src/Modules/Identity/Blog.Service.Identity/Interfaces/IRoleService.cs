using System;
using Blog.Domain.Identity.Entities;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;

namespace Blog.Service.Identity.Interfaces;

public interface IRoleService
{
    Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesAsync(CancellationToken cancellationToken);
    Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> AssignRolesByUserIdAsync(Guid userId, bool hasAdmin, CancellationToken cancellationToken);
    Task<Response<Guid>> CreateRole(RoleRequest request, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateRole(RoleRequest request, CancellationToken cancellationToken);
    Task<bool> AssignUserToRoleByUserIdAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    Task<Response<Guid>> DeleteRole(Guid roleId, CancellationToken cancellationToken);
}
