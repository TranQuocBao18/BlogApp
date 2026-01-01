using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Tag.Commands.Handlers;

public class UpsertTagHandler : IRequestHandler<UpsertTagCommand, Response<Guid>>
{
    private readonly ITagService _service;

    public UpsertTagHandler(ITagService service)
    {
        _service = service;
    }

    public Task<Response<Guid>> Handle(UpsertTagCommand request, CancellationToken cancellationToken)
    {
        if (request.Payload!.Id.HasValue)
        {
            return _service.UpdateTagAsync(request.Payload, cancellationToken);
        }
        return _service.CreateTagAsync(request.Payload, cancellationToken);
    }
}
