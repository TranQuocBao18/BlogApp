using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using MediatR;

namespace Blog.Service.Application.UseCases.Tag.Queries;

public class GetTagsQuery : IRequest<Response<IReadOnlyList<TagResponse>>>
{
    public string? Search { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
