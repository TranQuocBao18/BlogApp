using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Queries.Handlers;

public class GetRolesByUserIdHandler : IRequestHandler<GetRolesByUserIdQuery, PagedResponse<IReadOnlyList<RolesResponse>>>
{
    private readonly IRoleService _service;

    public GetRolesByUserIdHandler(IRoleService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetRolesAsync(cancellationToken);
    }
}
