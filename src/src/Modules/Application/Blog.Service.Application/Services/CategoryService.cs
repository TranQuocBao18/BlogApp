using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Interfaces;
using Blog.Shared.Auth;
using Blog.Utilities;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<CategoryService> logger
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Response<Guid>> CreateCategoryAsync(CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDuplicateName = await _applicationUnitOfWork.CategoryRepository.AnyAsync(x => x.Name == categoryRequest.Name, cancellationToken);
            if (isDuplicateName)
            {
                _logger.LogError("Tittle is existing");
                return new Response<Guid>(ErrorCodeEnum.CAT_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var categoryEntity = _mapper.Map<Category>(categoryRequest);
            categoryEntity.Created = _dateTimeService.NowUtc;
            categoryEntity.CreatedBy = currentUserId.ToString();

            // Generate slug from title and ensure uniqueness
            var baseSlug = StringUtils.GenerateSlug(categoryRequest.Name, 450);
            var slug = baseSlug;
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = Guid.NewGuid().ToString();
            }

            var suffix = 1;
            while (await _applicationUnitOfWork.CategoryRepository.AnyAsync(x => x.Slug == slug, cancellationToken))
            {
                slug = string.Concat(baseSlug, "-", suffix++);
                if (slug.Length > 450)
                {
                    slug = slug.Substring(0, 450).Trim('-');
                }
            }

            categoryEntity.Slug = slug;

            var categoryResponse = await _applicationUnitOfWork.CategoryRepository.AddAsync(categoryEntity, cancellationToken, true);
            if (categoryResponse == null || categoryResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Category fail");
                return new Response<Guid>(ErrorCodeEnum.CAT_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            return new Response<Guid>(categoryResponse.Id);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteCategoryAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var categoryEntity = await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (categoryEntity == null)
            {
                _logger.LogError("Category not found");
                return new Response<bool>(ErrorCodeEnum.CAT_ERR_001);
            }

            categoryEntity.LastModified = _dateTimeService.NowUtc;
            categoryEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.CategoryRepository.SoftDeleteAsync(categoryEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await _applicationUnitOfWork.RollbackAsync();
            throw new ApiException(ex.Message);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<CategoryResponse>>> GetCategoriesAsync(int pageNumber, int pageSize, string searchName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchName))
        {
            var totalItems = await _applicationUnitOfWork.CategoryRepository.CountAsync(x => !x.IsDeleted, cancellationToken);
            var categories = await _applicationUnitOfWork.CategoryRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);
            var categoriesResponse = _mapper.Map<IReadOnlyList<CategoryResponse>>(categories);
            return new PagedResponse<IReadOnlyList<CategoryResponse>>(categoriesResponse, pageNumber, pageSize, totalItems);
        }
        else
        {
            var totalItems = await _applicationUnitOfWork.CategoryRepository.CountAsync(x => !x.IsDeleted && x.Name.Contains(searchName), cancellationToken);
            var categories = await _applicationUnitOfWork.CategoryRepository.SearchAsync(x => !x.IsDeleted && x.Name.Contains(searchName), pageNumber, pageSize, cancellationToken);
            var categoriesResponse = _mapper.Map<IReadOnlyList<CategoryResponse>>(categories);
            return new PagedResponse<IReadOnlyList<CategoryResponse>>(categoriesResponse, pageNumber, pageSize, totalItems);
        }
    }

    public async Task<Response<CategoryResponse>> GetCategoryByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var categoryEntity = await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(id!.Value, cancellationToken);

            if (categoryEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Response<CategoryResponse>(ErrorCodeEnum.CAT_ERR_001);
            }

            var categoryResponse = _mapper.Map<CategoryResponse>(categoryEntity);
            return new Response<CategoryResponse>(categoryResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateCategoryAsync(CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDuplicateName = await _applicationUnitOfWork.CategoryRepository.AnyAsync(x => x.Name == categoryRequest.Name, cancellationToken);
            if (isDuplicateName)
            {
                _logger.LogError("Tittle is existing");
                return new Response<Guid>(ErrorCodeEnum.CAT_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var categoryEntity = await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(categoryRequest.Id!.Value, cancellationToken);

            if (categoryEntity == null)
            {
                _logger.LogError("Category not found");
                return new Response<Guid>(ErrorCodeEnum.CAT_ERR_001);
            }

            categoryEntity.Name = categoryRequest.Name;
            categoryEntity.LastModified = _dateTimeService.NowUtc;
            categoryEntity.LastModifiedBy = currentUserId.ToString();

            // Generate slug from title and ensure uniqueness
            var baseSlug = StringUtils.GenerateSlug(categoryRequest.Name, 450);
            var slug = baseSlug;
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = Guid.NewGuid().ToString();
            }

            var suffix = 1;
            while (await _applicationUnitOfWork.CategoryRepository.AnyAsync(x => x.Slug == slug, cancellationToken))
            {
                slug = string.Concat(baseSlug, "-", suffix++);
                if (slug.Length > 450)
                {
                    slug = slug.Substring(0, 450).Trim('-');
                }
            }

            categoryEntity.Slug = slug;

            var categoryResponse = _mapper.Map<CategoryResponse>(categoryEntity);

            await _applicationUnitOfWork.CategoryRepository.UpdateAsync(categoryEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<Guid>(categoryRequest.Id!.Value);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }
}
