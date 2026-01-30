using System;
using System.Linq.Expressions;
using Blog.Domain.Identity.Entities;

namespace Blog.Domain.Identity.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetRolesAsync();
    Task<List<Role>> GetRolesAsync(Expression<Func<SystemRole, bool>> predicate, CancellationToken cancellationToken);
    Task<List<Role>> GetRolesByUserIdAsync(Guid userId);
    Task<bool> GrantRolesByUserIdAsync(Guid userId, List<string> roleNames);
    Task<Guid> CreateRoleWithCodeAsync(SystemRole role, CancellationToken cancellationToken);
    Task<bool> AddUserToRoleByUserIdAsync(User user, SystemRole role, bool hasTransaction = false);
    Task<bool> RemoveUserFromGroupByUserIdAsync(User user, Role group, bool hasTransaction = false);
    Task<bool> AnyUserWithRoleAsync(string roleName);
    Task<bool> DeleteRole(string roleName, CancellationToken cancellationToken = default!, bool hasTransaction = false);
    Task<IList<string>> GetUserIdsByRoleIdAsync(string roleId, CancellationToken cancellationToken);
}
