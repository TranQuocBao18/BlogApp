using System;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Domain.Shared.Contracts;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Interfaces;
using Blog.Shared.Auth;
using Blog.Utilities.Extensions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class BlogLikeService : IBlogLikeService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<BlogLikeService> _logger;

    public BlogLikeService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<BlogLikeService> logger
        )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Infrastructure.Shared.Wrappers.Response<bool>> ToggleLikeAsync(Guid blogId, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var blogEntity = await _applicationUnitOfWork.BlogRepository.GetByIdAsync(blogId, cancellationToken);
            if (blogEntity == null)
            {
                _logger.LogError("Blog not found");
                return new Infrastructure.Shared.Wrappers.Response<bool>(ErrorCodeEnum.BLOG_ERR_001);
            }

            var currentUserId = _securityContextAccessor.UserId;
            var currentUserName = _securityContextAccessor.Email;
            var existingLike = await _applicationUnitOfWork.BlogLikeRepository.GetAsync(x => x.BlogId == blogId && x.UserId == currentUserId, cancellationToken);
            bool isLikedNow;
            if (existingLike != null)
            {
                // Case 1: Aldready like => Unlike
                await _applicationUnitOfWork.BlogLikeRepository.DeleteAsync(existingLike, cancellationToken, true);

                blogEntity.LikeCount = blogEntity.LikeCount > 0 ? (blogEntity.LikeCount - 1) : 0;

                isLikedNow = false;
            }
            else
            {
                // Case 2: Not like yet => Like
                var newLike = new BlogLike
                {
                    BlogId = blogId,
                    UserId = currentUserId
                };

                await _applicationUnitOfWork.BlogLikeRepository.AddAsync(newLike, cancellationToken, true);

                blogEntity.LikeCount += 1;

                isLikedNow = true;

                await _publishEndpoint.Publish(new LikeCreatedIntegrationEvent
                {
                    BlogId = blogId,
                    AuthorId = currentUserId,
                    AuthorName = currentUserName,
                    BlogAuthorId = blogEntity.CreatedBy!.AsGuid()
                });
            }
            await _applicationUnitOfWork.BlogRepository.UpdateAsync(blogEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Infrastructure.Shared.Wrappers.Response<bool>(isLikedNow);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    // public async Task<Response<Guid>> CreateBlogLikeAsync(BlogLikeRequest blogLikeRequest, CancellationToken cancellationToken)
    // {
    //     await _applicationUnitOfWork.BeginTransactionAsync();
    //     try
    //     {
    //         var currentUserId = _securityContextAccessor.UserId;
    //         var likeEntity = _mapper.Map<BlogLike>(blogLikeRequest);
    //         likeEntity.Created = _dateTimeService.NowUtc;
    //         likeEntity.CreatedBy = currentUserId.ToString();

    //         likeEntity.UserId = currentUserId;

    //         var likeResponse = await _applicationUnitOfWork.BlogLikeRepository.AddAsync(likeEntity, cancellationToken, true);
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

    // public async Task<Response<bool>> DeleteBlogLikeAsync(Guid? id, CancellationToken cancellationToken)
    // {
    //     await _applicationUnitOfWork.BeginTransactionAsync();
    //     try
    //     {
    //         var currentUserId = _securityContextAccessor.UserId;

    //         var blogLikeEntity = await _applicationUnitOfWork.BlogLikeRepository.GetByIdAsync(id!.Value, cancellationToken);
    //         if (blogLikeEntity == null)
    //         {
    //             _logger.LogError("Like not found");
    //             return new Response<bool>(ErrorCodeEnum.LIKE_ERR_001);
    //         }

    //         blogLikeEntity.LastModified = _dateTimeService.NowUtc;
    //         blogLikeEntity.LastModifiedBy = currentUserId.ToString();

    //         await _applicationUnitOfWork.BlogLikeRepository.SoftDeleteAsync(blogLikeEntity, cancellationToken, true);
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
