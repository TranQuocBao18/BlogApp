using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Requests;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Identity.Commands.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Response<string>>
{
    private readonly IAccountService _accountService;

    public RegisterUserHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerRequest = new RegisterRequest()
        {
            FullName = request.FullName,
            UserName = request.Email,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
        };
        return await _accountService.RegisterAsync(registerRequest, request.Origin);
    }
}
