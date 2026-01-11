using System;
using System.Linq.Expressions;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Enums;
using Blog.Infrastructure.Identity.Contexts;
using Blog.Infrastructure.Identity.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Utilities;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbSet<ApplicationUser> _applicationUser;
    private readonly IdentityContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<SystemRole> _roleManager;
    private readonly IConfiguration _configuration;
    private static int _userSequence = 1;
    private List<RoleEnums> RoleAdmins = new List<RoleEnums> { RoleEnums.Admin };

    public UserRepository(IdentityContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<SystemRole> roleManager, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _applicationUser = dbContext.Set<ApplicationUser>();
    }

    #region Read Methods

    IQueryable<User> ISearchRepository<User, Guid>.AsQueryable()
    {
        return _dbContext.Set<User>().AsQueryable();
    }

    public async Task<string> GetLatestCodeGenerateAsync(CancellationToken cancellationToken, bool hasTransaction = false)
    {
        string prefix = "USR";
        string newCode = string.Empty;
        bool codeExists;

        do
        {
            string nextSequence = _userSequence.ToString().PadLeft(3, '0'); // Generates '001', '002', etc.
            newCode = prefix + nextSequence;
            codeExists = await _dbContext.Set<ApplicationUser>().Where(x => x.Code == newCode).AnyAsync();
            _userSequence++;
        }
        while (codeExists);

        return newCode;
    }

    public async Task<IReadOnlyList<User>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var users = new List<User>();
        var applicationUsers = await _applicationUser
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.Created)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        // Transfer data of ApplicationUser to User
        foreach (var applicationUser in applicationUsers)
        {
            if (applicationUser.IsDeleted)
            {
                continue;
            }
            var isAdmin = await CheckCurrentRoleAdminAsync(applicationUser.Id.AsGuid());
            users.Add(new User
            {
                Id = applicationUser.Id.AsGuid(),
                Code = applicationUser.Code ?? string.Empty,
                Username = applicationUser.UserName ?? string.Empty,
                FullName = applicationUser?.FirstName?.ToFullName(applicationUser?.LastName ?? string.Empty) ?? string.Empty,
                Email = applicationUser?.Email ?? string.Empty,
                EmailConfirmed = applicationUser?.EmailConfirmed ?? false,
                PhoneNumber = applicationUser?.PhoneNumber ?? string.Empty,
                PhoneNumberConfirmed = applicationUser?.PhoneNumberConfirmed ?? false,
                IsLocked = applicationUser?.LockoutEnabled ?? false,
                IsDeleted = applicationUser?.IsDeleted ?? false,
                IsAdmin = isAdmin,

                Created = applicationUser?.Created ?? DateTime.UtcNow,
                CreatedBy = applicationUser?.CreatedBy ?? string.Empty,
                LastModified = applicationUser?.LastModified,
                LastModifiedBy = applicationUser?.LastModifiedBy ?? string.Empty,
            });
        }

        return users.AsReadOnly();
    }

    public async Task<IReadOnlyList<User>> SearchAsync(Expression<Func<ApplicationUser, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var users = new List<User>();
        var applicationUsers = await _applicationUser
                    .Where(x => !x.IsDeleted)
                    .Where(predicate)
                    .OrderByDescending(x => x.Created)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        // Transfer data of ApplicationUser to User
        foreach (var applicationUser in applicationUsers)
        {
            if (applicationUser.IsDeleted)
            {
                continue;
            }
            var isAdmin = await CheckCurrentRoleAdminAsync(applicationUser.Id.AsGuid());
            users.Add(ConvertToUser(applicationUser, isAdmin));
        }

        return users.AsReadOnly();
    }

    private static User ConvertToUser(ApplicationUser? applicationUser)
    {
        return new User
        {
            Id = applicationUser.Id.AsGuid(),
            Code = applicationUser.Code ?? string.Empty,
            Username = applicationUser.UserName ?? string.Empty,
            FullName = applicationUser?.FirstName?.ToFullName(applicationUser?.LastName ?? string.Empty) ?? string.Empty,
            Email = applicationUser?.Email ?? string.Empty,
            EmailConfirmed = applicationUser?.EmailConfirmed ?? false,
            PhoneNumber = applicationUser?.PhoneNumber ?? string.Empty,
            PhoneNumberConfirmed = applicationUser?.PhoneNumberConfirmed ?? false,
            IsLocked = applicationUser?.LockoutEnabled ?? false,
            IsDeleted = applicationUser?.IsDeleted ?? false,

            Created = applicationUser?.Created ?? DateTime.UtcNow,
            CreatedBy = applicationUser?.CreatedBy ?? string.Empty,
            LastModified = applicationUser?.LastModified,
            LastModifiedBy = applicationUser?.LastModifiedBy ?? string.Empty,
        };
    }

    private static User ConvertToUser(ApplicationUser? applicationUser, bool isAdmin)
    {
        var user = ConvertToUser(applicationUser);
        user.IsAdmin = isAdmin;
        return user;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var applicationUsers = await _applicationUser.ToListAsync(cancellationToken);
        if (applicationUsers == null || !applicationUsers.Any())
        {
            return null;
        }

        // Transfer data of ApplicationUser to User
        var users = new List<User>();
        foreach (var applicationUser in applicationUsers)
        {
            if (applicationUser.IsDeleted)
            {
                continue;
            }
            users.Add(ConvertToUser(applicationUser));
        }

        return users.AsReadOnly();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken)
    {
        var applicationUsers = await _applicationUser.Where(predicate).ToListAsync(cancellationToken);
        if (applicationUsers == null || !applicationUsers.Any())
        {
            return null;
        }

        // Transfer data of ApplicationUser to User
        var users = new List<User>();
        foreach (var applicationUser in applicationUsers)
        {
            if (applicationUser.IsDeleted)
            {
                continue;
            }
            users.Add(ConvertToUser(applicationUser));
        }

        return users.AsReadOnly();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().Where(predicate).ToListAsync();
    }

    public async Task<ApplicationUser> GetByPhoneNumberAsync(string phone, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        var applicationUsers = await _applicationUser.ToListAsync(cancellationToken);

        if (includedDeleted)
        {
            return applicationUsers
                .Where(x => x.PhoneNumber == phone)
                .FirstOrDefault();
        }

        return applicationUsers
            .Where(x => !x.IsDeleted && x.PhoneNumber == phone)
            .FirstOrDefault();
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        var applicationUser = await _applicationUser.FindAsync(id.ToString());
        if (applicationUser == null)
        {
            throw new Exception("Not found application user by id");
        }

        // Transfer data of ApplicationUser to User
        var user = ConvertToUser(applicationUser);

        return user;
    }

    public async Task<ApplicationUser> GetApplicationUserByUsernameAsync(string username, bool includedDeleted = false)
    {
        ApplicationUser? applicationUser;

        if (includedDeleted)
            applicationUser = await _applicationUser.FirstOrDefaultAsync(x => x.UserName == username);
        else
            applicationUser = await _applicationUser.FirstOrDefaultAsync(x => x.UserName == username && !x.IsDeleted);

        if (applicationUser == null)
            return null;

        return applicationUser;
    }

    public async Task<ApplicationUser> GetApplicationUserByEmailAsync(string email, bool includedDeleted = false)
    {
        ApplicationUser? applicationUser;

        if (includedDeleted)
            applicationUser = await _applicationUser.FirstOrDefaultAsync(x => x.Email == email);
        else
            applicationUser = await _applicationUser.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);

        if (applicationUser == null)
            return null;

        return applicationUser;
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var applicationUser = await _applicationUser.FirstOrDefaultAsync(x => x.UserName == username);

        if (applicationUser == null || applicationUser.IsDeleted)
            return null;

        // Transfer data of ApplicationUser to User
        var user = ConvertToUser(applicationUser);

        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser == null || applicationUser.IsDeleted)
        {
            return null;
        }

        // Transfer data of ApplicationUser to User
        var user = ConvertToUser(applicationUser);

        return user;
    }

    public async Task<ErrorCodeEnum> ValidateExistingAsync(string username, string email, Guid? userId = null, bool checkLock = true)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null)
        {
            if (user.IsDeleted)
            {
                return ErrorCodeEnum.USE_ERR_005;
            }

            if (checkLock && user.LockoutEnabled)
            {
                return ErrorCodeEnum.USE_ERR_004;
            }

            return ErrorCodeEnum.USE_ERR_002;
        }

        user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return ErrorCodeEnum.USE_ERR_002;
        }

        if (userId != null)
        {
            user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ErrorCodeEnum.USE_ERR_001;
            }
        }

        return ErrorCodeEnum.COM_SUC_000;
    }

    public Task<bool> AllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AllAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().Where(predicate).AnyAsync();
    }

    public async virtual Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _applicationUser.Where(predicate).AnyAsync();
    }

    public async virtual Task<int> CountAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().CountAsync(predicate, cancellationToken);
    }

    public async virtual Task<int> CountAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _applicationUser.CountAsync(predicate, cancellationToken);
    }

    public async virtual Task<TResult> MaxAsync<TResult>(Expression<Func<User, TResult>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().MaxAsync(predicate, cancellationToken);
    }

    public async virtual Task<TResult> MinAsync<TResult>(Expression<Func<User, TResult>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().MinAsync(predicate, cancellationToken);
    }

    #endregion

    #region Write Methods

    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(entity.Username);
        if (userWithSameUserName != null)
        {
            throw new Exception($"Username '{entity.Username}' is already taken.");
        }

        // Transfer data of User to ApplicationUser
        var splitFullName = StringUtils.SplitToFirstAndLast(entity.FullName);
        var user = new ApplicationUser
        {
            Email = entity.Email,
            EmailConfirmed = entity.EmailConfirmed,
            FirstName = splitFullName.Item1,
            LastName = splitFullName.Item2,
            UserName = entity.Username,
            PhoneNumber = entity.PhoneNumber,
            PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
            LockoutEnabled = entity.IsLocked
        };
        var userWithSameEmail = await _userManager.FindByEmailAsync(entity.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, entity.Password);

            if (user.LockoutEnabled)
            {
                var optionLockout = _configuration.GetOptionsExt<LockoutOptions>("IdentityServiceOptions:Lockout");
                _userManager.Options.Lockout = optionLockout;
                var lockoutEnd = new DateTimeOffset(DateTime.UtcNow.AddTicks(optionLockout.DefaultLockoutTimeSpan.Ticks));
                user.LockoutEnd = lockoutEnd;
                await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            }

            if (result.Succeeded)
            {
                if (entity.Roles.Any())
                {
                    foreach (var roleName in entity.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, roleName.ToString());
                    }
                }
                var userResult = await _userManager.FindByNameAsync(entity.Username);
                return new User
                {
                    Id = userResult.Id.AsGuid(),
                    Username = userResult.UserName,
                    FullName = userResult.FirstName.ToFullName(userResult.LastName),
                    Email = userResult.Email,
                    EmailConfirmed = userResult.EmailConfirmed,
                    PhoneNumber = userResult.PhoneNumber,
                    PhoneNumberConfirmed = userResult.PhoneNumberConfirmed,
                    IsLocked = userResult.LockoutEnabled,
                    IsDeleted = userResult.IsDeleted,

                    Created = userResult.Created,
                    CreatedBy = userResult.CreatedBy,
                    LastModified = userResult.LastModified,
                    LastModifiedBy = userResult.LastModifiedBy,
                };
            }
            else
            {
                throw new Exception($"{result.Errors}");
            }
        }
        else
        {
            throw new Exception($"Email {entity.Email} is already registered.");
        }

    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(entity.Username);

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteRangeAsync(IList<User> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDeleteAsync(User entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        var userEntity = await _applicationUser.FindAsync(entity.Id.ToString());
        userEntity.IsDeleted = true;
        userEntity.LastModified = DateTime.UtcNow;
        _applicationUser.Update(userEntity);

        if (!hasTransaction)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        // Transfer data of User to ApplicationUser
        var userEntity = await _applicationUser.FindAsync(entity.Id.ToString());
        var splitFullName = StringUtils.SplitToFirstAndLast(entity.FullName);
        userEntity.UserName = entity.Username;
        userEntity.NormalizedUserName = entity.Username.ToUpper();
        userEntity.FirstName = splitFullName.Item1;
        userEntity.LastName = splitFullName.Item2;
        userEntity.Email = entity.Email;
        userEntity.NormalizedEmail = entity.Email.ToUpper();
        userEntity.EmailConfirmed = entity.EmailConfirmed;
        userEntity.PhoneNumber = entity.PhoneNumber;
        userEntity.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
        userEntity.LastModified = entity.LastModified;
        userEntity.LastModifiedBy = entity.LastModifiedBy;

        _applicationUser.Update(userEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> AddRangeAsync(IList<User> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRangeAsync(IList<User> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        var user = await _userManager.FindByNameAsync(entity.Username);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IList<User> entities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SoftDeleteRangeAsync(IList<User> entities, CancellationToken cancellationToken, bool hasTransaction = false)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ResetPasswordAsync(Guid userId, string password)
    {
        var account = await _userManager.FindByIdAsync(userId.ToString());
        if (account == null)
        {
            throw new ApiException($"Not found account.");
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(account);
        await _userManager.ResetPasswordAsync(account, token, password);
        return true;
    }

    public async Task<bool> CheckPasswordAsync(Guid userId, string oldPassword)
    {
        var account = await _userManager.FindByIdAsync(userId.ToString());
        if (account == null)
        {
            throw new ApiException($"Not found account.");
        }
        return await _userManager.CheckPasswordAsync(account, oldPassword);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var account = await _userManager.FindByIdAsync(userId.ToString());
        if (account == null)
        {
            throw new ApiException($"Not found account.");
        }
        var result = await _userManager.ChangePasswordAsync(account, oldPassword, newPassword);
        return result.Succeeded;
    }

    #endregion

    private async Task<bool> CheckCurrentRoleAdminAsync(Guid userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roleList = _roleManager.Roles.ToList();
            var roleNamesByUser = await _userManager.GetRolesAsync(user);
            var isAdmin = roleNamesByUser.ToList().Any(x => x == RoleEnums.Admin.ToString());
            return isAdmin;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public IQueryable<User> QueryWithIncludes(bool includeDeleted = false, params Expression<Func<User, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> Query(bool includeDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken, bool includedDeleted = false)
    {
        throw new NotImplementedException();
    }
}
