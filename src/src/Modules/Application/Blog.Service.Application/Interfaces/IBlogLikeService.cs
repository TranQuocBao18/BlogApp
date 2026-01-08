using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface IBlogLikeService
{
    // Task<Response<Guid>> CreateBlogLikeAsync(BlogLikeRequest bloglikeRequest, CancellationToken cancellationToken);
    // Task<Response<bool>> DeleteBlogLikeAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<bool>> ToggleLikeAsync(Guid blogId, CancellationToken cancellationToken);
}
