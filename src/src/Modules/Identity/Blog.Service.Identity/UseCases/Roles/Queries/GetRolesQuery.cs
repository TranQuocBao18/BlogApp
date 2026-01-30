using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Identity.Responses;
using MediatR;

namespace Blog.Service.Identity.UseCases.Roles.Queries;

public class GetRolesQuery : IRequest<PagedResponse<IReadOnlyList<RolesResponse>>>
{

}
