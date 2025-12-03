using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Commands.Handlers;

public class DeleteUserByIdHandler : IRequestHandler<DeleteUserByIdCommand, Response<bool>>
{
    private readonly IUserService _service;

    public DeleteUserByIdHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<bool>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Id;
        return await _service.DeleteUserAsync(userId, cancellationToken);
    }
}
