using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ICommentLikeService
{
    Task<Response<Guid>> CreateCommentLikeAsync(CommentLikeRequest commentLikeRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteCommentLikeAsync(Guid? id, CancellationToken cancellationToken);
}
