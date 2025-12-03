using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands.Handlers;

public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, Response<Guid>>
{
    private readonly IUserService _service;

    public UpsertUserCommandHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<Guid>> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        var userRequest = request.Payload;
        if (userRequest == null || userRequest.Id == null || userRequest.Id == Guid.Empty)
        {
            return await _service.CreateUserAsync(userRequest, cancellationToken);
        }
        else
        {
            return await _service.UpdateUserAsync(userRequest, cancellationToken);
        }
    }
}
