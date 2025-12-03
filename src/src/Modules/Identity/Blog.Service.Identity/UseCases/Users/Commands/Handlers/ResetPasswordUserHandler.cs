using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands.Handlers;

public class ResetPasswordUserHandler : IRequestHandler<ResetPasswordUserCommand, Response<string>>
{
    private readonly IUserService _service;

    public ResetPasswordUserHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<string>> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
    {
        return await _service.ResetPasswordUserAsync(request.UserId, cancellationToken);
    }
}
