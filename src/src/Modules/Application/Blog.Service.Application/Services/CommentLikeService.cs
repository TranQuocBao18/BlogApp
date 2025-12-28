using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Service.Application.Interfaces;
using Blog.Shared.Auth;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class CommentLikeService : ICommentLikeService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<CommentLikeService> _logger;

    public CommentLikeService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<CommentLikeService> logger
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Response<Guid>> CreateCommentLikeAsync(CommentLikeRequest commentLikeRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var likeEntity = _mapper.Map<CommentLike>(commentLikeRequest);
            likeEntity.Created = _dateTimeService.NowUtc;
            likeEntity.CreatedBy = currentUserId.ToString();

            likeEntity.UserId = currentUserId;

            var likeResponse = await _applicationUnitOfWork.CommentLikeRepository.AddAsync(likeEntity, cancellationToken, true);
            if (likeResponse == null || likeResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Like fail");
                return new Response<Guid>(ErrorCodeEnum.LIKE_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            return new Response<Guid>(likeResponse.Id);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteCommentLikeAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var commentLikeEntity = await _applicationUnitOfWork.CommentLikeRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (commentLikeEntity == null)
            {
                _logger.LogError("Like not found");
                return new Response<bool>(ErrorCodeEnum.LIKE_ERR_001);
            }

            commentLikeEntity.LastModified = _dateTimeService.NowUtc;
            commentLikeEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.CommentLikeRepository.SoftDeleteAsync(commentLikeEntity, cancellationToken, true);
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
