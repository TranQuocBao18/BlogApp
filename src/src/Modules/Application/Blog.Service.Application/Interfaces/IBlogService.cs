using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface IBlogService
{
    Task<PagedResponse<IReadOnlyList<BlogResponse>>> GetBlogsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Response<BlogResponse>> GetBlogByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<BlogResponse>> GetBlogByBlogSlugAsync(string slug, CancellationToken cancellationToken);
    Task<Response<BlogResponse>> CreateBlogAsync(BlogRequest blogRequest, CancellationToken cancellationToken);
    Task<Response<BlogResponse>> UpdateBlogAsync(BlogRequest blogRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteBlogAsync(Guid? id, CancellationToken cancellationToken);

}
