using System;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands.Handlers;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Response<bool>>
{
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;

    public ForgotPasswordHandler(IAccountService accountService, IUserService userService)
    {
        _accountService = accountService;
        _userService = userService;
    }

    public async Task<Response<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var ipClient = request.IPAddress;
        var payload = request.Payload;
        var user = await _userService.GetUserByEmailAsync(payload.Email);

        if (user == null || !user.Succeeded)
        {
            return new Response<bool>(ErrorCodeEnum.USE_ERR_003);
        }
        await _userService.ResetPasswordUserAsync(user.Data.Id, cancellationToken);
        var sentEmail = await _accountService.ForgotPasswordAsync(payload, ipClient);
        return new Response<bool>(sentEmail);
    }
}
