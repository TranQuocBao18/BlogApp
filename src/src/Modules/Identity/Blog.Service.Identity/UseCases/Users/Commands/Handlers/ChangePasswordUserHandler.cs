using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands.Handlers;

public class ChangePasswordUserHandler : IRequestHandler<ChangePasswordUserCommand, Response<bool>>
{
    private readonly IUserService _service;

    public ChangePasswordUserHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<bool>> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
    {
        return await _service.ChangePasswordUserAsync(request.OldPassword, request.NewPassword, cancellationToken);
    }
}
