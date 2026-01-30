using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface IBlogTagService
{
    Task<Response<Guid>> CreateBlogTagAsync(BlogTagRequest blogTagRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteBlogTagAsync(Guid? id, CancellationToken cancellationToken);
}
