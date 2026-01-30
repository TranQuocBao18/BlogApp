using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Queries.Handlers;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, PagedResponse<IReadOnlyList<RolesResponse>>>
{
    private readonly IRoleService _service;

    public GetRolesHandler(IRoleService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<RolesResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetRolesAsync(cancellationToken);
    }
}
