using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface IBlogLikeService
{
    // Task<Response<Guid>> CreateBlogLikeAsync(BlogLikeRequest bloglikeRequest, CancellationToken cancellationToken);
    // Task<Response<bool>> DeleteBlogLikeAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<bool>> ToggleLikeAsync(Guid blogId, CancellationToken cancellationToken);
}
