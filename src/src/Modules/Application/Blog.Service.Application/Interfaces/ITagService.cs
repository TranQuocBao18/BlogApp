using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ITagService
{
    Task<PagedResponse<IReadOnlyList<TagResponse>>> GetTagsAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<Response<TagResponse>> GetTagByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<Guid>> CreateTagAsync(TagRequest tagRequest, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateTagAsync(TagRequest tagRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteTagAsync(Guid? id, CancellationToken cancellationToken);
}
