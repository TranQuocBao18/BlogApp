using System;
using Blog.Domain.Identity.Requests;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Identity.UseCases.Identity.Commands.Handlers;

public class LogoutHandler : IRequestHandler<LogoutCommand, Response<bool>>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<LogoutHandler> _logger;

    public LogoutHandler(IAccountService accountService, ILogger<LogoutHandler> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var logoutRequest = new LogoutRequest
        {
            UserId = request.UserId,
            IPAddress = request.IPAddress
        };
        _logger.LogInformation($"[LogoutCommandHandler] Logout for user: {request.UserId}");
        return await _accountService.LogoutAsync(logoutRequest, request.IPAddress, cancellationToken);
    }
}
