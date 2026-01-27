using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ICommentService
{
    // Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetCommentsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    // Task<Response<CommentResponse>> GetCommentByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetListCommentByBlogIdAsync(Guid blogId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetRepliesByParentIdAsync(Guid parentId, Guid blogId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Response<Guid>> CreateCommentAsync(CommentRequest commentRequest, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateCommentAsync(CommentRequest commentRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteCommentAsync(Guid? id, CancellationToken cancellationToken);
}
