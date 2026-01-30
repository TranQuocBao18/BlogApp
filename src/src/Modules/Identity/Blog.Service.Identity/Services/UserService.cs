using System;
using AutoMapper;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Identity.Interfaces;
using Blog.Infrastructure.Shared.Constants;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Dtos;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;
using Blog.Model.Dto.Shared.Dtos;
using Blog.Service.Identity.Interfaces;
using Blog.Shared.Auth;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Identity.Services;

public class UserService : IUserService, IUserServiceShare
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IIdentityUnitOfWork _identityUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IAccountService _accountService;
    private readonly IRoleService _roleService;

    public UserService(
        IMapper mapper,
        ILogger<UserService> logger,
        ISecurityContextAccessor securityContextAccessor,
        IIdentityUnitOfWork IdentityUnitOfWork,
        IAccountService accountService,
        IRoleService roleService)
    {
        _mapper = mapper;
        _logger = logger;
        _identityUnitOfWork = IdentityUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _accountService = accountService;
        _roleService = roleService;
    }

    public async Task<PagedResponse<IReadOnlyList<UsersResponse>>> GetUsersAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchName))
        {
            var totalUser = await _identityUnitOfWork.UserRepository.CountAsync(x => !x.IsDeleted, cancellationToken);
            var users = await _identityUnitOfWork.UserRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);
            var usersResponse = _mapper.Map<IReadOnlyList<UsersResponse>>(users);
            return new PagedResponse<IReadOnlyList<UsersResponse>>(usersResponse, pageNumber, pageSize, totalUser);
        }
        else
        {
            var totalUser = await _identityUnitOfWork.UserRepository.CountAsync(x => !x.IsDeleted && (x.FirstName.Contains(searchName) || x.LastName.Contains(searchName)), cancellationToken);
            var users = await _identityUnitOfWork.UserRepository.SearchAsync(x => !x.IsDeleted && (x.FirstName.Contains(searchName) || x.LastName.Contains(searchName)), pageNumber, pageSize, cancellationToken);
            var usersResponse = _mapper.Map<IReadOnlyList<UsersResponse>>(users);
            return new PagedResponse<IReadOnlyList<UsersResponse>>(usersResponse, pageNumber, pageSize, totalUser);
        }
    }

    public async Task<Response<UserResponse>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = await _identityUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
        if (userEntity == null)// || userEntity.IsDeleted)
        {
            return new Response<UserResponse>(ErrorCodeEnum.USE_ERR_001.ToString(), "User is not existing.");
        }
        var rolesByUserId = await _identityUnitOfWork.RoleRepository.GetRolesByUserIdAsync(userId);
        var userOveral = _mapper.Map<UserDto>(userEntity);
        var userDetail = _mapper.Map<UserDetailDto>(userEntity);
        userDetail.Roles = rolesByUserId?.Select(x => x.RoleName)?.ToList();
        var userResponse = new UserResponse()
        {
            UserDto = userOveral,
            UserDetailDto = userDetail
        };
        return new Response<UserResponse>(userResponse);
    }

    public async Task<Response<User>> GetUserByEmailAsync(string email)
    {
        var userEntity = await _identityUnitOfWork.UserRepository.GetByEmailAsync(email);
        if (userEntity == null) //|| userEntity.IsDeleted)
        {
            return new Response<User>(ErrorCodeEnum.USE_ERR_001.ToString(), "User is not existing.");
        }
        return new Response<User>(userEntity);
    }

    public async Task<Response<Guid>> CreateUserAsync(UserRequest user, CancellationToken cancellationToken)
    {
        try
        {
            var isDuplicateEmail = await _identityUnitOfWork.UserRepository.AnyAsync(x => x.Email == user.Email && !x.IsDeleted, cancellationToken);
            if (isDuplicateEmail)
            {
                return new Response<Guid>(ErrorCodeEnum.COM_ERR_002.ToString(), "Email is duplicate.");
            }

            var isDuplicateUsername = await _identityUnitOfWork.UserRepository.GetByUsernameAsync(user.Username) != null;
            if (isDuplicateUsername)
            {
                return new Response<Guid>(ErrorCodeEnum.COM_ERR_002.ToString(), "Username is duplicate.");
            }

            var requestAccount = new CreateAccountRequest()
            {
                Username = user.Username,
                Email = user.Email,
                FullName = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
            };
            var userResponse = await _accountService.CreateAccountAsync(requestAccount);
            if (!userResponse.Succeeded)
            {
                return new Response<Guid>(ErrorCodeEnum.USE_ERR_003);
            }

            var userId = Guid.Parse(userResponse.Data);
            if (user.IsAdmin == true)
            {
                await _roleService.AssignRolesByUserIdAsync(userId, true, cancellationToken);
            }

            // To-do Create User Department and User Position

            return new Response<Guid>(userId);
        }
        catch (Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateUserAsync(UserRequest user, CancellationToken cancellationToken)
    {
        try
        {
            var userEntity = await _identityUnitOfWork.UserRepository.GetByIdAsync(user.Id.Value, cancellationToken);
            if (userEntity == null)// || userEntity.IsDeleted)
            {
                return new Response<Guid>(ErrorCodeEnum.USE_ERR_001.ToString(), "User is not existing.");
            }

            userEntity.PhoneNumber = user.PhoneNumber;
            userEntity.FullName = user.Fullname;
            await _identityUnitOfWork.UserRepository.UpdateAsync(userEntity, cancellationToken, true);

            var hasAdmin = user.IsAdmin ?? false;
            await _roleService.AssignRolesByUserIdAsync(user.Id.Value, hasAdmin, cancellationToken);

            return new Response<Guid>(user.Id.Value);
        }
        catch (Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var userEntity = await _identityUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (userEntity == null)
            {
                return new Response<bool>(ErrorCodeEnum.USE_ERR_001);
            }
            await _identityUnitOfWork.UserRepository.SoftDeleteAsync(userEntity, cancellationToken);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<string>> ResetPasswordUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var userEntity = await _identityUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (userEntity == null)
            {
                return new Response<string>(ErrorCodeEnum.USE_ERR_001);
            }
            await _identityUnitOfWork.UserRepository.ResetPasswordAsync(userId, IdentityConstant.PasswordDefault);
            return new Response<string>(IdentityConstant.PasswordDefault);
        }
        catch (Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> ChangePasswordUserAsync(string oldPassword, string newPassword, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _securityContextAccessor.UserId;
            var userEntity = await _identityUnitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (userEntity == null)
            {
                return new Response<bool>(ErrorCodeEnum.USE_ERR_001);
            }
            var wrongPassword = await _identityUnitOfWork.UserRepository.CheckPasswordAsync(userId, oldPassword);
            if (!wrongPassword)
            {
                return new Response<bool>(ErrorCodeEnum.USE_ERR_006);
            }

            await _identityUnitOfWork.UserRepository.ChangePasswordAsync(userId, oldPassword, newPassword);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public async Task<UserDto?> GetUserShareByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userEntity = await _identityUnitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);
            if (userEntity is null)
                return null;

            return _mapper.Map<UserDto>(userEntity);
        }
        catch
        {
            return null;
        }
    }
}
