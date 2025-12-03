using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Commands.Handlers;

public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, Response<Guid>>
{
    private readonly IRoleService _roleService;

    public DeleteRoleHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Response<Guid>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await _roleService.DeleteRole(request.Id, cancellationToken);
    }
}
