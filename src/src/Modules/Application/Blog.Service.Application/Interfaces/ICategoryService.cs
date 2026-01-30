using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Domain.Application.Responses;

namespace Blog.Service.Application.Interfaces;

public interface ICategoryService
{
    Task<PagedResponse<IReadOnlyList<CategoryResponse>>> GetCategoriesAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken);
    Task<Response<CategoryResponse>> GetCategoryByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<Guid>> CreateCategoryAsync(CategoryRequest categoryRequest, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateCategoryAsync(CategoryRequest categoryRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteCategoryAsync(Guid? id, CancellationToken cancellationToken);
}
