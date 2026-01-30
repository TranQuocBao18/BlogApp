using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries;

public class GetBlogsPublishedQuery : IRequest<Response<IReadOnlyList<BlogResponse>>>
{
    public string? Search { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
