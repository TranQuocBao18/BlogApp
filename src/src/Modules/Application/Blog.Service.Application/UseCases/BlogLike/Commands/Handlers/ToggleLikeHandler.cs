using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.BlogLike.Commands.Handlers;

public class ToggleLikeHandler : IRequestHandler<ToggleLikeCommand, Response<bool>>
{
    private readonly IBlogLikeService _service;

    public ToggleLikeHandler(IBlogLikeService service)
    {
        _service = service;
    }

    public async Task<Response<bool>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var blogId = request.Id;
        return await _service.ToggleLikeAsync(blogId, cancellationToken);
    }
}
