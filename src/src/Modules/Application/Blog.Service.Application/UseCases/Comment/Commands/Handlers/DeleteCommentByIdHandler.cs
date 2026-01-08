using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Comment.Commands.Handlers;

public class DeleteCommentByIdHandler : IRequestHandler<DeleteCommentByIdCommand, Response<bool>>
{
    private readonly ICommentService _service;

    public DeleteCommentByIdHandler(ICommentService service)
    {
        _service = service;
    }

    public Task<Response<bool>> Handle(DeleteCommentByIdCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteCommentAsync(request.Id, cancellationToken);
    }
}
