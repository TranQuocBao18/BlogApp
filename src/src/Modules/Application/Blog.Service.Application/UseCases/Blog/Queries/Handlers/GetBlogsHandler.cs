using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries.Handlers;

public class GetBlogsHandler : IRequestHandler<GetBlogsQuery, Response<IReadOnlyList<BlogResponse>>>
{
    private readonly IBlogService _service;
    public GetBlogsHandler(IBlogService service)
    {
        _service = service;
    }

    public async Task<Response<IReadOnlyList<BlogResponse>>> Handle(GetBlogsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetBlogsAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}
