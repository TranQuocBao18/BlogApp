using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Tag.Commands.Handlers;

public class DeleteTagByIdHandler : IRequestHandler<DeleteTagByIdCommand, Response<bool>>
{
    private readonly ITagService _service;

    public DeleteTagByIdHandler(ITagService service)
    {
        _service = service;
    }

    public Task<Response<bool>> Handle(DeleteTagByIdCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteTagAsync(request.Id, cancellationToken);
    }
}
