using System;
using System.Linq.Expressions;
using Blog.Domain.Identity.Entities;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Interfaces;

namespace Blog.Domain.Identity.Interfaces;

public interface IUserRepository : IGenericRepository<User, Guid>
{
    Task<ApplicationUser> GetApplicationUserByUsernameAsync(string username, bool includedDeleted = false);
    Task<ApplicationUser> GetApplicationUserByEmailAsync(string username, bool includedDeleted = false);
    Task<IReadOnlyList<User>> SearchAsync(Expression<Func<ApplicationUser, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<ApplicationUser> GetByPhoneNumberAsync(string phone, CancellationToken cancellationToken, bool includedDeleted = false);
    Task<ErrorCodeEnum> ValidateExistingAsync(string username, string email, Guid? userId = null, bool checkLock = true);
    Task<bool> AllAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> ResetPasswordAsync(Guid userId, string password);
    Task<bool> CheckPasswordAsync(Guid userId, string oldPassword);
    Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    Task<string> GetLatestCodeGenerateAsync(CancellationToken cancellationToken, bool hasTransaction = false);
}
