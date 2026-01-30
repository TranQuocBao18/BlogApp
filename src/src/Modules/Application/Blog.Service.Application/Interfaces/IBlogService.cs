using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface IBlogService
{
    Task<PagedResponse<IReadOnlyList<BlogResponse>>> GetBlogsAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<PagedResponse<IReadOnlyList<BlogResponse>>> GetPublishedBlogsAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<PagedResponse<IReadOnlyList<BlogResponse>>> GetPublishedBlogsByCategoryIdAsync(Guid Id, int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<Response<BlogResponse>> GetBlogByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<BlogResponse>> GetBlogByBlogSlugAsync(string slug, CancellationToken cancellationToken);
    Task<Response<Guid>> CreateBlogAsync(BlogRequest blogRequest, CancellationToken cancellationToken);
    Task<Response<Guid>> CreateBlogWithoutTransactionAsync(BlogRequest blogRequest, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateBlogAsync(BlogRequest blogRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteBlogAsync(Guid? id, CancellationToken cancellationToken);

}
