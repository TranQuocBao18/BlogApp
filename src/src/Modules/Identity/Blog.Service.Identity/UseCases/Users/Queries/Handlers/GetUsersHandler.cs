using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using Blog.Service.Identity.Interfaces;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Queries.Handlers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PagedResponse<IReadOnlyList<UsersResponse>>>
{
    private readonly IUserService _service;

    public GetUsersHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<UsersResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetUsersAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}
