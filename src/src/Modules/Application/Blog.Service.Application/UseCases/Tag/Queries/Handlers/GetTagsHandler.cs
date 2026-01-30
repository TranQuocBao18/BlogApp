using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Tag.Queries.Handlers;

public class GetTagsHandler : IRequestHandler<GetTagsQuery, Response<IReadOnlyList<TagResponse>>>
{
    private readonly ITagService _service;

    public GetTagsHandler(ITagService service)
    {
        _service = service;
    }

    public async Task<Response<IReadOnlyList<TagResponse>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetTagsAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}
