using System;
using Blog.Domain.Identity.Requests;
using Blog.Domain.Identity.Responses;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using Grpc.Core.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Identity.UseCases.Identity.Commands.Handlers;

public class RefreshTokenHanlder : IRequestHandler<RefreshTokenCommand, Response<AuthenticationResponse>>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<RefreshTokenHanlder> _logger;

    public RefreshTokenHanlder(IAccountService accountService, ILogger<RefreshTokenHanlder> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<Response<AuthenticationResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var authenticationRequest = new RefreshTokenRequest()
        {
            RefreshToken = request.RefreshToken,
        };
        return await _accountService.RefreshTokenAsync(authenticationRequest, request.IPAddress, cancellationToken);
    }
}
