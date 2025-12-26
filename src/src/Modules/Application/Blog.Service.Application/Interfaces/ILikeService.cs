using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ILikeService
{
    Task<Response<IReadOnlyList<LikeResponse>>> GetLikesOfBlogByBlogIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<IReadOnlyList<LikeResponse>>> GetLikesOfUserByBlogIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<LikeResponse>> CreateLikeAsync(LikeRequest likeRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteLikeAsync(Guid? id, CancellationToken cancellationToken);
}
