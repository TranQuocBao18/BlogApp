using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ITagService
{
    Task<PagedResponse<IReadOnlyList<TagResponse>>> GetTagsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Response<TagResponse>> GetTagByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<TagResponse>> CreateTagAsync(TagRequest tagRequest, CancellationToken cancellationToken);
    Task<Response<TagResponse>> UpdateTagAsync(TagRequest tagRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteTagAsync(Guid? id, CancellationToken cancellationToken);
}
