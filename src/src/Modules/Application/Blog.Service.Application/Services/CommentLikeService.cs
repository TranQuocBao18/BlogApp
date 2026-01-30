using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Shared.Contracts;
using Blog.Domain.Application.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Domain.Application.Requests;
using Blog.Service.Application.Interfaces;
using Blog.Shared.Auth;
using Blog.Utilities.Extensions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class CommentLikeService : ICommentLikeService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IPublishEndpoint _publishEndpoint;
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

    public async Task<Infrastructure.Shared.Wrappers.Response<bool>> ToggleLikeAsync(Guid commentId, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(commentId, cancellationToken);
            if (commentEntity == null)
            {
                _logger.LogError("Comment not found");
                return new Infrastructure.Shared.Wrappers.Response<bool>(ErrorCodeEnum.COMM_ERR_001);
            }

            var currentUserId = _securityContextAccessor.UserId;
            var currentUserName = _securityContextAccessor.Email;
            var existingLike = await _applicationUnitOfWork.CommentLikeRepository.GetAsync(x => x.CommentId == commentId && x.UserId == currentUserId, cancellationToken);
            bool isLikedNow;
            if (existingLike != null)
            {
                // Case 1: Aldready like => Unlike
                await _applicationUnitOfWork.CommentLikeRepository.DeleteAsync(existingLike, cancellationToken, true);

                commentEntity.LikeCount = commentEntity.LikeCount > 0 ? (commentEntity.LikeCount - 1) : 0;

                isLikedNow = false;
            }
            else
            {
                // Case 2: Not like yet => Like
                var newLike = new CommentLike
                {
                    CommentId = commentId,
                    UserId = currentUserId
                };

                await _applicationUnitOfWork.CommentLikeRepository.AddAsync(newLike, cancellationToken, true);

                commentEntity.LikeCount += 1;

                isLikedNow = true;

                await _publishEndpoint.Publish(new CommentLikeCreatedIntegrationEvent
                {
                    BlogId = commentEntity.BlogId,
                    AuthorId = currentUserId,
                    AuthorName = currentUserName,
                    CommentAuthorId = commentEntity.CreatedBy!.AsGuid()
                });
            }
            await _applicationUnitOfWork.CommentRepository.UpdateAsync(commentEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Infrastructure.Shared.Wrappers.Response<bool>(isLikedNow);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
        ;
    }

    // public async Task<Response<Guid>> CreateCommentLikeAsync(CommentLikeRequest commentLikeRequest, CancellationToken cancellationToken)
    // {
    //     await _applicationUnitOfWork.BeginTransactionAsync();
    //     try
    //     {
    //         var currentUserId = _securityContextAccessor.UserId;
    //         var likeEntity = _mapper.Map<CommentLike>(commentLikeRequest);
    //         likeEntity.Created = _dateTimeService.NowUtc;
    //         likeEntity.CreatedBy = currentUserId.ToString();

    //         likeEntity.UserId = currentUserId;

    //         var likeResponse = await _applicationUnitOfWork.CommentLikeRepository.AddAsync(likeEntity, cancellationToken, true);
    //         if (likeResponse == null || likeResponse.Id == Guid.Empty)
    //         {
    //             _logger.LogError("Create Like fail");
    //             return new Response<Guid>(ErrorCodeEnum.LIKE_ERR_003);
    //         }

    //         await _applicationUnitOfWork.CommitAsync();
    //         return new Response<Guid>(likeResponse.Id);
    //     }
    //     catch (Exception ex)
    //     {
    //         await _applicationUnitOfWork.RollbackAsync();
    //         _logger.LogError(ex.Message);
    //         throw new ApiException(ex.Message);
    //     }
    // }

    // public async Task<Response<bool>> DeleteCommentLikeAsync(Guid? id, CancellationToken cancellationToken)
    // {
    //     await _applicationUnitOfWork.BeginTransactionAsync();
    //     try
    //     {
    //         var currentUserId = _securityContextAccessor.UserId;

    //         var commentLikeEntity = await _applicationUnitOfWork.CommentLikeRepository.GetByIdAsync(id!.Value, cancellationToken);
    //         if (commentLikeEntity == null)
    //         {
    //             _logger.LogError("Like not found");
    //             return new Response<bool>(ErrorCodeEnum.LIKE_ERR_001);
    //         }

    //         commentLikeEntity.LastModified = _dateTimeService.NowUtc;
    //         commentLikeEntity.LastModifiedBy = currentUserId.ToString();

    //         await _applicationUnitOfWork.CommentLikeRepository.SoftDeleteAsync(commentLikeEntity, cancellationToken, true);
    //         await _applicationUnitOfWork.CommitAsync();

    //         return new Response<bool>(true);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         await _applicationUnitOfWork.RollbackAsync();
    //         throw new ApiException(ex.Message);
    //     }
    // }
}
