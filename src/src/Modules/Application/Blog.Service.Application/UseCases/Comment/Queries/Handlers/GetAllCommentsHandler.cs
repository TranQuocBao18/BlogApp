using Blog.Domain.Application.Responses;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Queries.Handlers;

public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsQuery, Response<IReadOnlyList<CommentResponse>>>
{
    private readonly ICommentService _service;

    public GetAllCommentsHandler(ICommentService service)
    {
        _service = service;
    }

    public async Task<Response<IReadOnlyList<CommentResponse>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllCommentsAsync(request.PageNumber, request.PageSize, request.Search, cancellationToken);
    }
}