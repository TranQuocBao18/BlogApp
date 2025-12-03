using System;
using Blog.Domain.Identity.Entities;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Identity.Validators;

public class CustomUserValidator : IUserValidator<ApplicationUser>
{
    public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(manager);
        ArgumentNullException.ThrowIfNull(user);

        var errors = new List<IdentityError>();

        await ValidateUserName(errors, manager, user);
        await ValidateEmail(errors, manager, user);

        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }

    private async Task ValidateUserName(List<IdentityError> errors, UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var userName = user.UserName;

        if (string.IsNullOrWhiteSpace(userName))
        {
            errors.Add(new IdentityError
            {
                Code = ErrorCodeEnum.USE_ERR_010.ToString(),
                Description = ErrorCodeEnum.USE_ERR_010.GetEnumDescription()
            });
            return;
        }

        var existingUser = await manager.Users
            .Where(u => u.UserName == userName && u.Id != user.Id)
            .FirstOrDefaultAsync();

        if (existingUser != null && !existingUser.IsDeleted)
        {
            errors.Add(new IdentityError
            {
                Code = ErrorCodeEnum.USE_ERR_011.ToString(),
                Description = $"Username '{userName}' is already taken."
            });
        }
    }

    private async Task ValidateEmail(List<IdentityError> errors, UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var email = user.Email;

        if (string.IsNullOrWhiteSpace(email))
            return;

        var existingUser = await manager.Users
            .Where(u => u.Email == email && u.Id != user.Id)
            .FirstOrDefaultAsync();

        if (existingUser != null && !existingUser.IsDeleted)
        {
            errors.Add(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = $"Email '{email}' is already taken."
            });
        }
    }
}
