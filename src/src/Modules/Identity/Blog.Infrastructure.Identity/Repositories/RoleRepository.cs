using System;
using System.Linq.Expressions;
using Blog.Domain.Identity.Entities;
using Blog.Infrastructure.Identity.Contexts;
using Blog.Domain.Identity.Interfaces;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Identity.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DbSet<SystemRole> _identityRole;
    private readonly DbSet<IdentityUserRole<string>> _identityUserRole;
    private readonly DbSet<IdentityRoleClaim<string>> _identityRoleClaim;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<SystemRole> _roleManager;
    private readonly DbContext _dbContext;
    private static int _roleSequence = 1;

    public RoleRepository(IdentityContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<SystemRole> roleManager)
    {
        _identityRole = dbContext.Set<SystemRole>();
        _identityUserRole = dbContext.Set<IdentityUserRole<string>>();
        _userManager = userManager;
        _roleManager = roleManager;
        _identityRoleClaim = dbContext.Set<IdentityRoleClaim<string>>();
        _dbContext = dbContext;
    }

    public async Task<IList<string>> GetUserIdsByRoleIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var groupUsers = await _identityUserRole.Where(x => x.RoleId == roleId).ToListAsync(cancellationToken);
        return groupUsers.Select(x => x.UserId).ToList();
    }

    public async Task<bool> DeleteRole(string roleName, CancellationToken cancellationToken = default!, bool hasTransaction = false)
    {
        var role = await _identityRole.FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);
        if (role == null)
        {
            return false;
        }
        _identityRole.Remove(role);
        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        return true;
    }

    public async Task<bool> AnyUserWithRoleAsync(string roleName)
    {
        var role = await _identityRole.FirstOrDefaultAsync(x => x.Name == roleName);
        if (role == null)
        {
            return false;
        }
        var userRoles = await _identityUserRole.Where(x => x.RoleId == role.Id).ToListAsync();
        return userRoles != null && userRoles.Count > 0;
    }

    public async Task<Guid> CreateRoleWithCodeAsync(SystemRole role, CancellationToken cancellationToken)
    {
        var code = await GetLatestCodeGenerateAsync(cancellationToken);
        role.Code = code;

        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded == false)
        {
            throw new ApiException($"{result.Errors}");
        }

        return Guid.Parse(role.Id);
    }

    private async Task<string> GetLatestCodeGenerateAsync(CancellationToken cancellationToken)
    {
        string prefix = "R";
        string newCode = string.Empty;
        bool codeExists;

        do
        {
            string nextSequence = _roleSequence.ToString().PadLeft(3, '0'); // Generates '001', '002', etc.
            newCode = prefix + nextSequence;
            codeExists = await _identityRole.Where(x => x.Code == newCode).AnyAsync(cancellationToken);
            _roleSequence++;
        }
        while (codeExists);

        return newCode;
    }

    public async Task<List<Role>> GetRolesAsync()
    {
        var identityRoles = await _identityRole.ToListAsync();
        if (identityRoles == null || identityRoles.Count == 0)
        {
            throw new ApiException("Identity Role is empty.");
        }

        var roles = new List<Role>();
        foreach (var role in identityRoles)
        {
            roles.Add(ConvertToRole(role));
        }
        return roles;
    }

    private static Role ConvertToRole(SystemRole? role)
    {
        return new()
        {
            Id = role.Id.AsGuid(),
            Code = role.Code ?? string.Empty,
            RoleName = role?.Name ?? string.Empty,
            Created = role.Created,
            CreatedBy = role.CreatedBy,
            IsDeleted = role.IsDeleted,
            LastModified = role.LastModified,
            LastModifiedBy = role.LastModifiedBy,
        };
    }

    public async Task<List<Role>> GetRolesAsync(Expression<Func<SystemRole, bool>> predicate, CancellationToken cancellationToken = default!)
    {
        var roles = await _identityRole.Where(predicate).ToListAsync(cancellationToken);
        var result = roles.Select(ConvertToRole).ToList();
        return result;
    }

    public async Task<List<Role>> GetRolesByUserIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var roleList = _roleManager.Roles.ToList();
        var roleNamesByUser = await _userManager.GetRolesAsync(user);
        var roles = new List<Role>();
        foreach (var roleName in roleNamesByUser)
        {
            var role = roleList.FirstOrDefault(x => x.Name == roleName);
            roles.Add(ConvertToRole(role));
        }
        return roles;
    }

    public async Task<bool> RemoveUserFromGroupByUserIdAsync(User user, Role group, bool hasTransaction = false)
    {
        try
        {
            var userRole = await _identityUserRole.FirstOrDefaultAsync(x => x.UserId == user.Id.ToString() && x.RoleId == group.Id.ToString());
            if (userRole != null)
            {
                _identityUserRole.Remove(userRole);
            }

            if (!hasTransaction)
            {
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new ApiException($"{ex.Message}");
        }
    }

    public async Task<bool> AddUserToRoleByUserIdAsync(User user, SystemRole role, bool hasTransaction = false)
    {
        try
        {
            _identityUserRole.Add(new IdentityUserRole<string>
            {
                UserId = user.Id.ToString(),
                RoleId = role.Id
            });

            if (!hasTransaction)
            {
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new ApiException($"{ex.Message}");
        }
    }

    public async Task<bool> GrantRolesByUserIdAsync(Guid userId, List<string> roleNames)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }
            var roleNamesByUser = await _userManager.GetRolesAsync(user);
            if (roleNamesByUser == null || !roleNamesByUser.Any())
            {
                return false;
            }
            var removeRolesOfUser = await _userManager.RemoveFromRolesAsync(user, roleNamesByUser);

            var rolesAssign = _roleManager.Roles.ToList().Where(x => roleNames.Contains(x.Name));
            if (rolesAssign == null || !rolesAssign.Any())
            {
                return false;
            }
            var newRoleNames = rolesAssign.Select(x => x.Name);
            var addRolesOfUser = await _userManager.AddToRolesAsync(user, newRoleNames);

            return addRolesOfUser.Succeeded;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
