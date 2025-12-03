using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Identity.Responses;
using MediatR;

namespace Blog.Service.Identity.UseCases.Users.Queries;

public class GetUsersQuery : IRequest<PagedResponse<IReadOnlyList<UsersResponse>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}
