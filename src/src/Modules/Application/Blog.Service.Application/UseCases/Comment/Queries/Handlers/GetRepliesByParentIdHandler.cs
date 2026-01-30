using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Queries.Handlers;

public class GetRepliesByParentIdHandler : IRequestHandler<GetRepliesByParentIdQuery, PagedResponse<IReadOnlyList<CommentResponse>>>
{
    private readonly ICommentService _service;

    public GetRepliesByParentIdHandler(ICommentService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<CommentResponse>>> Handle(GetRepliesByParentIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetRepliesByParentIdAsync(request.Id, request.BlogId, request.PageNumber, request.PageSize, cancellationToken);
    }
}
