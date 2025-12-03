using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Commands.Handlers;

public class UpsertRoleHandler : IRequestHandler<UpsertRoleCommand, Response<Guid>>
{
    private readonly IRoleService _service;

    public UpsertRoleHandler(IRoleService service)
    {
        _service = service;
    }

    public async Task<Response<Guid>> Handle(UpsertRoleCommand request, CancellationToken cancellationToken)
    {
        var roleRequest = request.Payload;

        if (roleRequest == null || roleRequest.Id == null || roleRequest.Id == Guid.Empty)
        {
            return await _service.CreateRole(roleRequest, cancellationToken);
        }

        return await _service.UpdateRole(roleRequest, cancellationToken);
    }
}
