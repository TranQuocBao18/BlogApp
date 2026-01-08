using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Commands.Handlers;

public class UpsertCommentHandler : IRequestHandler<UpsertCommentCommand, Response<Guid>>
{
    private readonly ICommentService _service;

    public UpsertCommentHandler(ICommentService service)
    {
        _service = service;
    }

    public async Task<Response<Guid>> Handle(UpsertCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.Payload!.Id.HasValue)
        {
            return await _service.UpdateCommentAsync(request.Payload, cancellationToken);
        }
        return await _service.CreateCommentAsync(request.Payload, cancellationToken);
    }
}
