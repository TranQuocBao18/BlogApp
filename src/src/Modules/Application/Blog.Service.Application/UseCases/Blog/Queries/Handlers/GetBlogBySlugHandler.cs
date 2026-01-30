using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Responses;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Queries.Handlers;

public class GetBlogBySlugHandler : IRequestHandler<GetBlogBySlugQuery, Response<BlogResponse>>
{
    private readonly IBlogService _service;

    public GetBlogBySlugHandler(IBlogService service)
    {
        _service = service;
    }

    public async Task<Response<BlogResponse>> Handle(GetBlogBySlugQuery request, CancellationToken cancellationToken)
    {
        var blogSlug = request.Slug;
        return await _service.GetBlogByBlogSlugAsync(blogSlug, cancellationToken);
    }
}
