using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Commands.Handlers;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Response<AuthenticationResponse>>
{
    private readonly IAccountService _accountService;

    public LoginUserHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Response<AuthenticationResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var authenticationRequest = new AuthenticationRequest()
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe,
        };
        return await _accountService.AuthenticateAsync(authenticationRequest, request.IPAddress);
    }
}
