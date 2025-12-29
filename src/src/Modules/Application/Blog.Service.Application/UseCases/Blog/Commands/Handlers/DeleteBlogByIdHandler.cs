using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Commands.Handlers;

public class DeleteBlogByIdHandler : IRequestHandler<DeleteBlogByIdCommand, Response<bool>>
{
    private readonly IBlogService _service;

    public DeleteBlogByIdHandler(IBlogService service)
    {
        _service = service;
    }

    public async Task<Response<bool>> Handle(DeleteBlogByIdCommand request, CancellationToken cancellationToken)
    {
        var blogId = request.Id;
        return await _service.DeleteBlogAsync(blogId, cancellationToken);
    }
}
