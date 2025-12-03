using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Requests;
using Blog.Model.Dto.Identity.Responses;

namespace Blog.Service.Identity.Interfaces;

public interface IAccountService
{
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
    Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
    Task<Response<string>> ConfirmEmailAsync(string userId, string code);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest model, string origin);
    Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest model);
    Task<Response<ProfileResponse>> GetProfileAsync(Guid userId);
    Task<Response<string>> CreateAccountAsync(CreateAccountRequest request);
}
