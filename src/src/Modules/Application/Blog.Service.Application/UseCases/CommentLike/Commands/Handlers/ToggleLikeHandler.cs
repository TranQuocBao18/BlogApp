using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.CommentLike.Commands.Handlers;

public class ToggleLikeHandler : IRequestHandler<ToggleLikeCommand, Response<bool>>
{
    private readonly ICommentLikeService _service;

    public ToggleLikeHandler(ICommentLikeService service)
    {
        _service = service;
    }

    public async Task<Response<bool>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var blogId = request.Id;
        return await _service.ToggleLikeAsync(blogId, cancellationToken);
    }
}
