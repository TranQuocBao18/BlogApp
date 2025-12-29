using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries.Handlers;

public class GetBlogByIdHandler : IRequestHandler<GetBlogByIdQuery, Response<BlogResponse>>
{
    private readonly IBlogService _service;

    public GetBlogByIdHandler(IBlogService service)
    {
        _service = service;
    }

    public async Task<Response<BlogResponse>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blogId = request.Id;
        return await _service.GetBlogByIdAsync(blogId, cancellationToken);
    }
}
