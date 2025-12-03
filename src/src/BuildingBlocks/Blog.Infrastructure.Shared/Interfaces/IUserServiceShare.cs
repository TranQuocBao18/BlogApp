using System;
using Blog.Model.Dto.Shared.Dtos;

namespace Blog.Infrastructure.Shared.Interfaces;

public interface IUserServiceShare
{
    Task<UserDto?> GetUserShareByIdAsync(Guid id, CancellationToken cancellationToken);
}
