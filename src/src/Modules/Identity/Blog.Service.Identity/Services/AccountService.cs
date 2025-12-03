using System;
using System.Text;
using Blog.Domain.Identity.Entities;
using Blog.Domain.Shared.Enums;
using Blog.Infrastructure.Identity.Interfaces;
using Blog.Infrastructure.Shared.Constants;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Requests;
using Blog.Model.Dto.Identity.Responses;
using Blog.Model.Dto.Shared.Outbox;
using Blog.Service.Identity.Interfaces;
using Blog.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Blog.Service.Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<SystemRole> _roleManager;
    private readonly IIdentityUnitOfWork _identityUnitOfWork;
    private readonly IEmailService _emailService;
    private readonly IServiceProvider _serviceProvider;
    private ITokenService _tokenService => (ITokenService)_serviceProvider.GetService(typeof(ITokenService));

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<SystemRole> roleManager,
        IEmailService emailService,
        IIdentityUnitOfWork identityUnitOfWork,
        IServiceProvider serviceProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;

        this._emailService = emailService;
        _identityUnitOfWork = identityUnitOfWork;
        _serviceProvider = serviceProvider;
        _roleManager = roleManager;
    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
    {
        ApplicationUser? user = null;

        if (string.IsNullOrWhiteSpace(request.Email) == false)
        {
            user = await _identityUnitOfWork.UserRepository.GetApplicationUserByEmailAsync(request.Email, includedDeleted: true);
        }
        if (user == null)
        {
            user = await _identityUnitOfWork.UserRepository.GetApplicationUserByUsernameAsync(request.Email, includedDeleted: true);
        }
        if (user == null)
        {
            user = await _identityUnitOfWork.UserRepository.GetByPhoneNumberAsync(request.Email, CancellationToken.None, includedDeleted: true);
        }
        if (user == null)
        {
            return new Response<AuthenticationResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"No Accounts Registered with {request.Email}.");
        }

        if (user.IsDeleted == true)
        {
            return new Response<AuthenticationResponse>(ErrorCodeEnum.USE_ERR_012);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            return new Response<AuthenticationResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"Invalid Credentials for '{request.Email}'.");
        }
        if (user.IsDeleted == true)
        {
            return new Response<AuthenticationResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"Account is Deleted for '{request.Email}'.");
        }
        if (user.LockoutEnabled)
        {
            return new Response<AuthenticationResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"Account is Locked for '{request.Email}'.");
        }

        var jwtToken = await _tokenService.GenerateJWToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(ipAddress);

        AuthenticationResponse response = new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName,
            DisplayName = $"{user.FirstName} {user.LastName}",
            JWToken = jwtToken,
            RefreshToken = refreshToken.Token,
            IsVerified = user.EmailConfirmed,
        };

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        var roleNameByUser = rolesList.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(roleNameByUser))
        {
            response.GroupCode = _roleManager.Roles.FirstOrDefault(x => x.Name == roleNameByUser)?.Code ?? "";
        }
        response.Roles = rolesList.ToList();

        return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            throw new ApiException($"Username '{request.UserName}' is already taken.");
        }

        var splitFullName = StringUtils.SplitToFirstAndLast(request.FullName);
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = splitFullName.Item1,
            LastName = splitFullName.Item2,
            UserName = request.UserName
        };

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleEnums.Member.ToString());
                var verificationUri = await SendVerificationEmail(user, origin);
                //TODO: Attach Email Service here and configure it via appsettings
                await _emailService.SendAsync(new EmailRequest() { From = "tqbao251002@gmail.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
        else
        {
            throw new ApiException($"Email {request.Email} is already registered.");
        }
    }

    public async Task<Response<string>> CreateAccountAsync(CreateAccountRequest request)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
        if (userWithSameUserName != null)
        {
            throw new ApiException($"Username '{request.Username}' is already taken.");
        }

        var splitFullName = StringUtils.SplitToFirstAndLast(request.FullName);
        var user = new ApplicationUser
        {
            Email = request.Email,
            Code = request.Code,
            FirstName = splitFullName.Item1,
            LastName = splitFullName.Item2,
            UserName = request.Username,
            PhoneNumber = request.PhoneNumber,
        };

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleEnums.Member.ToString());
                return new Response<string>(user.Id);
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
        else
        {
            throw new ApiException($"Email {request.Email} is already created.");
        }
    }

    public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
        }
        else
        {
            throw new ApiException($"An error occured while confirming {user.Email}.");
        }
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest model, string origin)
    {
        var account = await _userManager.FindByEmailAsync(model.Email);

        // Always return ok response to prevent email enumeration
        if (account == null)
        {
            return false;
        }

        var emailRequest = new EmailRequest()
        {
            Body = $"Địa chỉ IP: {origin} đã yêu cầu cấp lại mật khẩu mới. Mật khẩu mới của bạn là: {IdentityConstant.PasswordDefault}",
            To = model.Email,
            Subject = "Yêu cầu cấp lại mật khẩu",
        };
        await _emailService.SendAsync(emailRequest);
        return true;
    }

    public async Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest model)
    {
        var account = await _userManager.FindByEmailAsync(model.Email);
        if (account == null)
        {
            throw new ApiException($"No Accounts Registered with {model.Email}.");
        }
        var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
        if (result.Succeeded)
        {
            return new Response<string>(model.Email, message: $"Password Resetted.");
        }
        else
        {
            throw new ApiException($"Error occured while reseting the password.");
        }
    }

    public async Task<Response<ProfileResponse>> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return new Response<ProfileResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"No Accounts Registered with {userId.ToString()}.");
        }
        if (user.IsDeleted == true)
        {
            return new Response<ProfileResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"Account is Deleted for '{userId.ToString()}'.");
        }
        if (user.LockoutEnabled)
        {
            return new Response<ProfileResponse>(ErrorCodeEnum.COM_ERR_000.ToString(), $"Account is Locked for '{userId.ToString()}'.");
        }

        ProfileResponse response = new();

        response.Id = userId;
        response.Email = user.Email;
        response.UserName = user.UserName;
        response.PhoneNumber = user.PhoneNumber;
        response.FullName = user.FirstName + " " + user.LastName;
        response.DisplayName = user.FirstName + " " + user.LastName;
        response.DisplayName = $"{user.FirstName} {user.LastName}";
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        return new Response<ProfileResponse>(response, $"Authenticated {user.UserName}");
    }

    #region Private methods

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/account/confirm-email/";
        var _enpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        //Email Service Call Here
        return verificationUri;
    }

    #endregion
}
