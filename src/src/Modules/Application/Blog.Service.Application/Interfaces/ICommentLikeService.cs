using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ICommentLikeService
{
    // Task<Response<Guid>> CreateCommentLikeAsync(CommentLikeRequest commentLikeRequest, CancellationToken cancellationToken);
    // Task<Response<bool>> DeleteCommentLikeAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<bool>> ToggleLikeAsync(Guid commentId, CancellationToken cancellationToken);
}
