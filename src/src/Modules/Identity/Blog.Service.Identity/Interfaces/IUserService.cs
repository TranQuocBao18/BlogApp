using System;
using Blog.Domain.Identity.Entities;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Requests;
using Blog.Model.Dto.Identity.Responses;

namespace Blog.Service.Identity.Interfaces;

public interface IUserService
{
    Task<PagedResponse<IReadOnlyList<UsersResponse>>> GetUsersAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<Response<UserResponse>> GetUserByIdAsync(Guid UserId, CancellationToken cancellationToken);
    Task<Response<User>> GetUserByEmailAsync(string email);
    Task<Response<Guid>> CreateUserAsync(UserRequest User, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateUserAsync(UserRequest User, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteUserAsync(Guid UserId, CancellationToken cancellationToken);
    Task<Response<string>> ResetPasswordUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<Response<bool>> ChangePasswordUserAsync(string oldPassword, string newPassword, CancellationToken cancellationToken);
}
