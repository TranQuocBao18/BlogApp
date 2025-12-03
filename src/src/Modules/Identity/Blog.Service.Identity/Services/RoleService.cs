using System;
using AutoMapper;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Enums;
using Blog.Infrastructure.Identity.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Requests;
using Blog.Model.Dto.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using Blog.Shared.Auth;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Identity.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly ILogger<RoleService> _logger;
    private readonly IIdentityUnitOfWork _identityUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly RoleManager<SystemRole> _roleManager;
    private List<RoleEnums> RoleAdmins = [RoleEnums.Admin];

    public RoleService(
        IMapper mapper,
        ILogger<RoleService> logger,
        IIdentityUnitOfWork identityUnitOfWork,
        ISecurityContextAccessor securityContextAccessor,
        RoleManager<SystemRole> roleManager)
    {
        _mapper = mapper;
        _logger = logger;
        _identityUnitOfWork = identityUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _roleManager = roleManager;
    }

    public async Task<Response<Guid>> DeleteRole(Guid groupId, CancellationToken cancellationToken)
    {
        var groupFromDb = await _roleManager.FindByIdAsync(groupId.ToString());
        if (groupFromDb is null)
        {
            return new Response<Guid>(ErrorCodeEnum.ROG_ERR_001);
        }

        var hasUser = await _identityUnitOfWork.RoleRepository.AnyUserWithRoleAsync(groupFromDb.Name);
        if (hasUser)
        {
            return new Response<Guid>(ErrorCodeEnum.ROG_ERR_007);
        }

        await _identityUnitOfWork.RoleRepository.DeleteRole(groupFromDb.Name, cancellationToken: cancellationToken);

        return new Response<Guid>(groupId);
    }

    public async Task<Response<Guid>> CreateRole(RoleRequest request, CancellationToken cancellationToken)
    {
        var currentUserLoggedInId = _securityContextAccessor.UserId.ToString();
        var role = request;

        var duplicateRoleCount = await _roleManager.Roles
            .Where(r => r.Name.Equals(role.Name) && !r.IsDeleted)
            .CountAsync(cancellationToken);

        if (duplicateRoleCount > 0)
        {
            throw new ApiException("Role is existing.");
        }

        var sysRole = new SystemRole
        {
            Name = request.Name.Trim(),
            CreatedBy = currentUserLoggedInId.ToString(),
            Created = DateTime.UtcNow,
            NormalizedName = request.Name.ToUpperInvariant(),
        };

        var result = await _identityUnitOfWork.RoleRepository.CreateRoleWithCodeAsync(sysRole, cancellationToken);

        if (result == Guid.Empty)
        {
            return new Response<Guid>(ErrorCodeEnum.ROG_ERR_002);
        }

        return new Response<Guid>(result);
    }

    public async Task<Response<Guid>> UpdateRole(RoleRequest request, CancellationToken cancellationToken)
    {
        await _identityUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserLoggedInId = _securityContextAccessor.UserId.ToString();
            var role = request;

            var roleEntity = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == request.Id.ToString());
            if (roleEntity is null)
            {
                throw new ApiException("Role is not existing.");
            }
            var duplicateRoleCount = await _roleManager.Roles
               .Where(r => r.Name.Equals(request.Name) && !r.IsDeleted)
               .CountAsync(cancellationToken);

            if (duplicateRoleCount > 1)
            {
                throw new ApiException("Role is existing.");
            }

            roleEntity.Name = request.Name;
            roleEntity.LastModifiedBy = currentUserLoggedInId.ToString();
            roleEntity.LastModified = DateTime.UtcNow;
            roleEntity.NormalizedName = request.Name.Trim().ToUpperInvariant();

            var result = await _roleManager.UpdateAsync(roleEntity);
            if (result.Succeeded == false)
            {
                throw new ApiException($"{result.Errors}");
            }

            await _identityUnitOfWork.CommitAsync();
            return new Response<Guid>(roleEntity.Id.AsGuid());
        }
        catch (Exception ex)
        {
            await _identityUnitOfWork.RollbackAsync();
            throw new ApiException(ex.Message);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _identityUnitOfWork.RoleRepository.GetRolesAsync();
        var rolesResponse = new List<RolesResponse>();
        foreach (var role in roles)
        {
            rolesResponse.Add(ConvertToRoleReponse(role));
        }
        return new PagedResponse<IReadOnlyList<RolesResponse>>(rolesResponse, 0, 1000, rolesResponse.Count());
    }

    private static RolesResponse ConvertToRoleReponse(Role role)
    {
        return new RolesResponse
        {
            Id = role.Id,
            RoleName = role.RoleName,
            Code = role.Code ?? string.Empty,
            Created = role.Created,
            IsDeleted = role.IsDeleted,
            CreatedBy = role.CreatedBy,
            LastModified = role.LastModified,
            LastModifiedBy = role.LastModifiedBy
        };
    }

    public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var rolesByUserId = await _identityUnitOfWork.RoleRepository.GetRolesByUserIdAsync(userId);
        var rolesResponse = new List<RolesResponse>();
        foreach (var role in rolesByUserId)
        {
            rolesResponse.Add(ConvertToRoleReponse(role));
        }
        return new PagedResponse<IReadOnlyList<RolesResponse>>(rolesResponse, 0, 1000, rolesResponse.Count());
    }

    private bool CheckCurrentRoleAdminInToken()
    {
        var roles = _securityContextAccessor.Roles;
        foreach (var role in roles)
        {
            var inValidRole = Enum.TryParse(role, out RoleEnums roleEnum);
            if (!inValidRole)
            {
                return false;
            }

            bool isAdmin = RoleAdmins.Contains(roleEnum);
            return isAdmin;
        }
        return false;
    }

    private async Task<List<RoleEnums>> GetCurrentUserRolesInDatabaseAsync()
    {
        var result = new List<RoleEnums>();
        var userId = _securityContextAccessor.UserId;
        var rolesByUserId = await _identityUnitOfWork.RoleRepository.GetRolesByUserIdAsync(userId);
        if (rolesByUserId == null || !rolesByUserId.Any())
        {
            return result;
        }
        foreach (var roleByUser in rolesByUserId)
        {
            var roleName = roleByUser.RoleName;
            var inValidRole = Enum.TryParse(roleName, out RoleEnums roleEnum);
            if (inValidRole)
            {
                result.Add(roleEnum);
            }
        }
        return result;
    }

    public async Task<bool> AssignUserToRoleByUserIdAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
    {
        var user = await _identityUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken) ?? throw new ApiException("User is not existing.");
        var roleFromDb = await _roleManager.FindByIdAsync(roleId.ToString()) ?? throw new ApiException("Role is not existing.");
        var res = await _identityUnitOfWork.RoleRepository.AddUserToRoleByUserIdAsync(user, roleFromDb);

        return res;
    }

    public async Task<bool> AssignRolesByUserIdAsync(Guid userId, bool hasAdmin, CancellationToken cancellationToken)
    {
        var isAssigned = false;
        if (hasAdmin)
        {
            var roleNames = new List<string>()
            {
                RoleEnums.Admin.ToString(),
                RoleEnums.Member.ToString()
            };
            isAssigned = await _identityUnitOfWork.RoleRepository.GrantRolesByUserIdAsync(userId, roleNames);
        }
        else
        {
            string roleName = RoleEnums.Member.ToString();
            isAssigned = await _identityUnitOfWork.RoleRepository.GrantRolesByUserIdAsync(userId, new List<string> { roleName });
        }
        return isAssigned;
    }
}
