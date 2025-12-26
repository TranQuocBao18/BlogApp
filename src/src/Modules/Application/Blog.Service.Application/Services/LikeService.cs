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

public class LikeService : ILikeService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<LikeService> _logger;

    public LikeService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<LikeService> logger
        )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Response<LikeResponse>> CreateLikeAsync(LikeRequest likeRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var likeEntity = _mapper.Map<Like>(likeRequest);
            likeEntity.Created = _dateTimeService.NowUtc;
            likeEntity.CreatedBy = currentUserId.ToString();

            likeEntity.UserId = currentUserId;

            var likeResponse = await _applicationUnitOfWork.LikeRepository.AddAsync(likeEntity, cancellationToken, true);
            if (likeResponse == null || likeResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Like fail");
                return new Response<LikeResponse>(ErrorCodeEnum.LIKE_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            var likeDto = _mapper.Map<LikeResponse>(likeResponse);
            return new Response<LikeResponse>(likeDto);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteLikeAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var likeEntity = await _applicationUnitOfWork.LikeRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (likeEntity == null)
            {
                _logger.LogError("Like not found");
                return new Response<bool>(ErrorCodeEnum.LIKE_ERR_001);
            }

            likeEntity.LastModified = _dateTimeService.NowUtc;
            likeEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.LikeRepository.SoftDeleteAsync(likeEntity, cancellationToken, true);
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

    public async Task<Response<IReadOnlyList<LikeResponse>>> GetLikesOfUserByBlogIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var likes = await _applicationUnitOfWork.LikeRepository.GetAllAsync(x => x.BlogId == id && x.UserId == currentUserId, cancellationToken);
            var likeResponse = _mapper.Map<IReadOnlyList<LikeResponse>>(likes);
            return new Response<IReadOnlyList<LikeResponse>>(likeResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<IReadOnlyList<LikeResponse>>> GetLikesOfBlogByBlogIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var likes = await _applicationUnitOfWork.LikeRepository.GetAllAsync(x => x.BlogId == id, cancellationToken);
            var likeResponse = _mapper.Map<IReadOnlyList<LikeResponse>>(likes);
            return new Response<IReadOnlyList<LikeResponse>>(likeResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }

    }
}
