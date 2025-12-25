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

public class TagService : ITagService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<TagService> _logger;

    public TagService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<TagService> logger
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Response<TagResponse>> CreateTagAsync(TagRequest tagRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDuplicateTagName = await _applicationUnitOfWork.TagRepository.AnyAsync(x => x.Name == tagRequest.Name, cancellationToken);
            if (isDuplicateTagName)
            {
                _logger.LogError("Tag is existing");
                return new Response<TagResponse>(ErrorCodeEnum.TAG_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var tagEntity = _mapper.Map<Tag>(tagRequest);
            tagEntity.Created = _dateTimeService.NowUtc;
            tagEntity.CreatedBy = currentUserId.ToString();

            var tagResponse = await _applicationUnitOfWork.TagRepository.AddAsync(tagEntity, cancellationToken, true);
            if (tagResponse == null || tagResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Tag fail");
                return new Response<TagResponse>(ErrorCodeEnum.TAG_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            var tagDto = _mapper.Map<TagResponse>(tagResponse);
            return new Response<TagResponse>(tagDto);

        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteTagAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var tagEntity = await _applicationUnitOfWork.TagRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (tagEntity == null)
            {
                _logger.LogError("Tag not found");
                return new Response<bool>(ErrorCodeEnum.TAG_ERR_001);
            }

            tagEntity.LastModified = _dateTimeService.NowUtc;
            tagEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.TagRepository.SoftDeleteAsync(tagEntity, cancellationToken, true);
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

    public async Task<Response<TagResponse>> GetTagByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var tagEntity = await _applicationUnitOfWork.TagRepository.GetByIdAsync(id!.Value, cancellationToken);

            if (tagEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Response<TagResponse>(ErrorCodeEnum.BLOG_ERR_001);
            }

            var tagResponse = _mapper.Map<TagResponse>(tagEntity);
            return new Response<TagResponse>(tagResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<TagResponse>>> GetTagsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var totalItems = await _applicationUnitOfWork.TagRepository.CountAsync(x => !x.IsDeleted, cancellationToken);

        var tags = await _applicationUnitOfWork.TagRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);

        var tagsResponse = _mapper.Map<IReadOnlyList<TagResponse>>(tags);

        return new PagedResponse<IReadOnlyList<TagResponse>>(tagsResponse, pageNumber, pageSize, totalItems);
    }

    public async Task<Response<TagResponse>> UpdateTagAsync(TagRequest tagRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDuplicateTagName = await _applicationUnitOfWork.TagRepository.AnyAsync(x => x.Name == tagRequest.Name, cancellationToken);
            if (isDuplicateTagName)
            {
                _logger.LogError("Tag is existing");
                return new Response<TagResponse>(ErrorCodeEnum.TAG_ERR_002);
            }
            var currentUserId = _securityContextAccessor.UserId;
            var tagEntity = await _applicationUnitOfWork.TagRepository.GetByIdAsync(tagRequest.Id, cancellationToken);

            if (tagEntity == null)
            {
                _logger.LogError("Tag not found");
                return new Response<TagResponse>(ErrorCodeEnum.TAG_ERR_001);
            }

            tagEntity.Name = tagRequest.Name;
            tagEntity.LastModified = _dateTimeService.NowUtc;
            tagEntity.LastModifiedBy = currentUserId.ToString();

            var tagResponse = _mapper.Map<TagResponse>(tagEntity);

            await _applicationUnitOfWork.TagRepository.UpdateAsync(tagEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<TagResponse>(tagResponse);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }
}
