using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Queries.Handlers;

public class GetListCommentByBlodIdHandler : IRequestHandler<GetListCommentByBlogIdQuery, PagedResponse<IReadOnlyList<CommentResponse>>>
{
    private readonly ICommentService _service;

    public GetListCommentByBlodIdHandler(ICommentService service)
    {
        _service = service;
    }

    public async Task<PagedResponse<IReadOnlyList<CommentResponse>>> Handle(GetListCommentByBlogIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetListCommentByBlogIdAsync(request.Id, request.PageNumber, request.PageSize, cancellationToken);
    }
}
