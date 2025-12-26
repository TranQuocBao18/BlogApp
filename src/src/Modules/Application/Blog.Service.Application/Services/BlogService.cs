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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class BlogService : IBlogService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<BlogService> _logger;
    private readonly IConfiguration _configuration;

    public BlogService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<BlogService> logger,
        IConfiguration configuration
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Response<Guid>> CreateBlogAsync(BlogRequest blogRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDuplicateTitle = await _applicationUnitOfWork.BlogRepository.AnyAsync(x => x.Tittle == blogRequest.Tittle, cancellationToken);
            if (isDuplicateTitle)
            {
                _logger.LogError("Tittle is existing");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var blogEntity = _mapper.Map<BlogEntity>(blogRequest);
            blogEntity.Created = _dateTimeService.NowUtc;
            blogEntity.CreatedBy = currentUserId.ToString();

            // Generate slug from title and ensure uniqueness
            var baseSlug = StringUtils.GenerateSlug(blogRequest.Tittle, 450);
            var slug = baseSlug;
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = Guid.NewGuid().ToString();
            }

            var suffix = 1;
            while (await _applicationUnitOfWork.BlogRepository.AnyAsync(x => x.Slug == slug, cancellationToken))
            {
                slug = string.Concat(baseSlug, "-", suffix++);
                if (slug.Length > 450)
                {
                    slug = slug.Substring(0, 450).Trim('-');
                }
            }

            blogEntity.Slug = slug;

            var blogResponse = await _applicationUnitOfWork.BlogRepository.AddAsync(blogEntity, cancellationToken, true);
            if (blogResponse == null || blogResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Blog fail");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            return new Response<Guid>(blogResponse.Id);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    // Create without starting/committing transaction; caller manages unit of work transaction.
    public async Task<Response<Guid>> CreateBlogWithoutTransactionAsync(BlogRequest blogRequest, CancellationToken cancellationToken)
    {
        try
        {
            var isDuplicateTitle = await _applicationUnitOfWork.BlogRepository.AnyAsync(x => x.Tittle == blogRequest.Tittle, cancellationToken);
            if (isDuplicateTitle)
            {
                _logger.LogError("Tittle is existing");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var blogEntity = _mapper.Map<BlogEntity>(blogRequest);
            blogEntity.Created = _dateTimeService.NowUtc;
            blogEntity.CreatedBy = currentUserId.ToString();

            var baseSlug = StringUtils.GenerateSlug(blogRequest.Tittle, 450);
            var slug = baseSlug;
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = Guid.NewGuid().ToString();
            }

            var suffix = 1;
            while (await _applicationUnitOfWork.BlogRepository.AnyAsync(x => x.Slug == slug, cancellationToken))
            {
                slug = string.Concat(baseSlug, "-", suffix++);
                if (slug.Length > 450)
                {
                    slug = slug.Substring(0, 450).Trim('-');
                }
            }

            blogEntity.Slug = slug;

            var blogResponse = await _applicationUnitOfWork.BlogRepository.AddAsync(blogEntity, cancellationToken, true);
            if (blogResponse == null || blogResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Blog fail");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_003);
            }

            return new Response<Guid>(blogResponse.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteBlogAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var blogEntity = await _applicationUnitOfWork.BlogRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (blogEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Response<bool>(ErrorCodeEnum.BLOG_ERR_001);
            }

            blogEntity.LastModified = _dateTimeService.NowUtc;
            blogEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.BlogRepository.SoftDeleteAsync(blogEntity, cancellationToken, true);
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

    public async Task<Response<BlogResponse>> GetBlogByBlogSlugAsync(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var blogEntity = await _applicationUnitOfWork.BlogRepository.GetByBlogSlugAsync(slug, cancellationToken);

            if (blogEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Response<BlogResponse>(ErrorCodeEnum.BLOG_ERR_001);
            }

            var blogResponse = _mapper.Map<BlogResponse>(blogEntity);
            return new Response<BlogResponse>(blogResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<BlogResponse>> GetBlogByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var blogEntity = await _applicationUnitOfWork.BlogRepository.GetByIdAsync(id!.Value, cancellationToken);

            if (blogEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Response<BlogResponse>(ErrorCodeEnum.BLOG_ERR_001);
            }

            var blogResponse = _mapper.Map<BlogResponse>(blogEntity);
            return new Response<BlogResponse>(blogResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<BlogResponse>>> GetBlogsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var totalItems = await _applicationUnitOfWork.BlogRepository.CountAsync(x => !x.IsDeleted, cancellationToken);

        var blogs = await _applicationUnitOfWork.BlogRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);

        var blogsResponse = _mapper.Map<IReadOnlyList<BlogResponse>>(blogs);

        return new PagedResponse<IReadOnlyList<BlogResponse>>(blogsResponse, pageNumber, pageSize, totalItems);
    }

    public async Task<Response<Guid>> UpdateBlogAsync(BlogRequest blogRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDuplicateTitle = await _applicationUnitOfWork.BlogRepository.AnyAsync(x => x.Tittle == blogRequest.Tittle, cancellationToken);
            if (isDuplicateTitle)
            {
                _logger.LogError("Tittle is existing");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var blogEntity = await _applicationUnitOfWork.BlogRepository.GetByIdAsync(blogRequest.Id, cancellationToken);

            if (blogEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_001);
            }

            blogEntity.Tittle = blogRequest.Tittle;
            blogEntity.Content = blogRequest.Content;
            blogEntity.BannerId = blogRequest.BannerId;
            blogEntity.CategoryId = blogRequest.CategoryId;
            blogEntity.Slug = blogRequest.Slug;
            blogEntity.Status = blogRequest.Status;
            blogEntity.LastModified = _dateTimeService.NowUtc;
            blogEntity.LastModifiedBy = currentUserId.ToString();

            var isDuplicateSlug = await _applicationUnitOfWork.BlogRepository.AnyAsync(x => x.Slug == blogRequest.Slug, cancellationToken);
            if (isDuplicateSlug)
            {
                _logger.LogError("Slug is existing");
                return new Response<Guid>(ErrorCodeEnum.BLOG_ERR_006);
            }

            await _applicationUnitOfWork.BlogRepository.UpdateAsync(blogEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<Guid>(blogRequest.Id);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }
}
