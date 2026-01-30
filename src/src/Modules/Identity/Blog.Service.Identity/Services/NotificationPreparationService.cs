using System;
using Blog.Domain.Identity.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Dtos;
using Blog.Utilities.Extensions;

namespace Blog.Service.Identity.Services;

public class NotificationPreparationService : INotificationPreparationService
{
    private readonly IIdentityUnitOfWork _identityUnitOfWork;

    public NotificationPreparationService(IIdentityUnitOfWork identityUnitOfWork)
    {
        _identityUnitOfWork = identityUnitOfWork;
    }

    public async Task<Response<GroupUserDto>> GetUsersByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var groupFromDb = await _identityUnitOfWork.RoleRepository.GetRolesAsync(g => g.Id == groupId.ToString(), cancellationToken);
        if (groupFromDb == null)
            return new Response<GroupUserDto>(ErrorCodeEnum.ROG_ERR_001);
        var groupUsers = await _identityUnitOfWork.RoleRepository.GetUserIdsByRoleIdAsync(groupId.ToString(), cancellationToken);
        var response = new GroupUserDto
        {
            GroupId = groupId,
            UserIds = groupUsers.Select(x => x.AsGuid()).ToList()
        };

        return new Response<GroupUserDto>(response);
    }
}
