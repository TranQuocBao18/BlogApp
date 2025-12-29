using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Commands.Handlers;

public class UpdateBlogHandler : IRequestHandler<UpdateBlogCommand, Response<Guid>>
{
    private readonly IBlogService _service;
    public UpdateBlogHandler(IBlogService service)
    {
        _service = service;
    }

    public async Task<Response<Guid>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blogRequest = request.Payload;
        return await _service.UpdateBlogAsync(blogRequest, cancellationToken);
    }
}
