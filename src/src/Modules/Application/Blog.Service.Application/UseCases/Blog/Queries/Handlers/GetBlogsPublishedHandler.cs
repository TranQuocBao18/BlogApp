using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries.Handlers;

public class GetBlogsPublishedHandler : IRequestHandler<GetBlogsPublishedQuery, Response<IReadOnlyList<BlogResponse>>>
{
    private readonly IBlogService _service;
    public GetBlogsPublishedHandler(IBlogService service)
    {
        _service = service;
    }

    public async Task<Response<IReadOnlyList<BlogResponse>>> Handle(GetBlogsPublishedQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetPublishedBlogsAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}
