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
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class BlogTagService : IBlogTagService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<BlogTagService> _logger;

    public BlogTagService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<BlogTagService> logger
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Response<Guid>> CreateBlogTagAsync(BlogTagRequest blogTagRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var blogTagEntity = _mapper.Map<BlogTag>(blogTagRequest);
            blogTagEntity.Created = _dateTimeService.NowUtc;
            blogTagEntity.CreatedBy = currentUserId.ToString();

            var blogTagResponse = await _applicationUnitOfWork.BlogTagRepository.AddAsync(blogTagEntity, cancellationToken, true);
            if (blogTagResponse == null || blogTagResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create BlogTag fail");
                return new Response<Guid>(ErrorCodeEnum.BTAG_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            return new Response<Guid>(blogTagResponse.Id);

        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteBlogTagAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var blogTagEntity = await _applicationUnitOfWork.BlogTagRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (blogTagEntity == null)
            {
                _logger.LogError("BlogTag not found");
                return new Response<bool>(ErrorCodeEnum.BTAG_ERR_001);
            }

            blogTagEntity.LastModified = _dateTimeService.NowUtc;
            blogTagEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.BlogTagRepository.SoftDeleteAsync(blogTagEntity, cancellationToken, true);
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
}
