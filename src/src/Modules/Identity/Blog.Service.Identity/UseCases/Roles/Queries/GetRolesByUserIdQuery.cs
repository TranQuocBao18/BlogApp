using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Queries;

public class GetRolesByUserIdQuery : IRequest<PagedResponse<IReadOnlyList<RolesResponse>>>
{
    public Guid UserId { get; set; }
}
