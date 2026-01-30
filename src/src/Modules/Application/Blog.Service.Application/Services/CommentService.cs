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
using Blog.Domain.Application.Responses;
using Blog.Service.Application.Interfaces;
using Blog.Shared.Auth;
using Blog.Utilities.Extensions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<CommentService> _logger;

    public CommentService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IPublishEndpoint publishEndpoint,
        IDateTimeService dateTimeService,
        ILogger<CommentService> logger
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _publishEndpoint = publishEndpoint;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Infrastructure.Shared.Wrappers.Response<Guid>> CreateCommentAsync(CommentRequest commentRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var currentUserName = _securityContextAccessor.Email;
            var commentEntity = _mapper.Map<Comment>(commentRequest);
            commentEntity.Created = _dateTimeService.NowUtc;
            commentEntity.CreatedBy = currentUserId.ToString();

            var commentResponse = await _applicationUnitOfWork.CommentRepository.AddAsync(commentEntity, cancellationToken, true);
            if (commentResponse == null || commentResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Comment fail");
                return new Infrastructure.Shared.Wrappers.Response<Guid>(ErrorCodeEnum.COMM_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();

            if (commentRequest.ParentId == Guid.Empty)
            {
                var blog = await _applicationUnitOfWork.BlogRepository.GetByIdAsync(commentRequest.BlogId, cancellationToken);
                await _publishEndpoint.Publish(new CommentCreatedIntegrationEvent
                {
                    BlogId = commentRequest.BlogId,
                    AuthorId = currentUserId,
                    AuthorName = currentUserName,
                    BlogAuthorId = blog.CreatedBy?.AsGuid(),
                    Content = commentRequest.Content
                });
            }
            else
            {
                await _publishEndpoint.Publish(new CommentCreatedIntegrationEvent
                {
                    BlogId = commentRequest.BlogId,
                    AuthorId = currentUserId,
                    AuthorName = currentUserName,
                    ParentId = commentRequest.ParentId,
                    Content = commentRequest.Content
                });
            }
            return new Infrastructure.Shared.Wrappers.Response<Guid>(commentResponse.Id);

        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Infrastructure.Shared.Wrappers.Response<bool>> DeleteCommentAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (commentEntity == null)
            {
                _logger.LogError("Comment not found");
                return new Infrastructure.Shared.Wrappers.Response<bool>(ErrorCodeEnum.COMM_ERR_001);
            }

            commentEntity.LastModified = _dateTimeService.NowUtc;
            commentEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.CommentRepository.SoftDeleteAsync(commentEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Infrastructure.Shared.Wrappers.Response<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await _applicationUnitOfWork.RollbackAsync();
            throw new ApiException(ex.Message);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetListCommentByBlogIdAsync(Guid blogId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var currentUserId = _securityContextAccessor.UserId;
        var totalItems = await _applicationUnitOfWork.CommentRepository.CountAsync(x => !x.IsDeleted && x.ParentId == null, cancellationToken);
        var comments = await _applicationUnitOfWork.CommentRepository.GetCommentsByBlogIdAsync(blogId, currentUserId, pageNumber, pageSize, cancellationToken);
        var commentResponse = new List<CommentResponse>();
        foreach (var (commentEntity, replyCount, isLiked) in comments)
        {
            var commentDto = _mapper.Map<CommentResponse>(commentEntity);
            commentDto.ReplyCount = replyCount;
            commentDto.IsLikeByCurrentUser = isLiked;
            commentResponse.Add(commentDto);
        }
        return new PagedResponse<IReadOnlyList<CommentResponse>>(commentResponse, pageNumber, pageSize, totalItems);
    }

    public async Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetRepliesByParentIdAsync(Guid parentId, Guid blogId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var currentUserId = _securityContextAccessor.UserId;
        var totalItems = await _applicationUnitOfWork.CommentRepository.CountAsync(x => !x.IsDeleted && x.BlogId == blogId && x.ParentId == parentId, cancellationToken);
        var replies = await _applicationUnitOfWork.CommentRepository.GetRepliesByParentIdAsync(parentId, blogId, currentUserId, cancellationToken);
        var commentResponse = new List<CommentResponse>();
        foreach (var (commentEntity, replyCount, isLiked) in replies)
        {
            var commentDto = _mapper.Map<CommentResponse>(commentEntity);
            commentDto.ReplyCount = replyCount;
            commentDto.IsLikeByCurrentUser = isLiked;
            commentResponse.Add(commentDto);
        }
        return new PagedResponse<IReadOnlyList<CommentResponse>>(commentResponse, pageNumber, pageSize, totalItems);
    }

    // public async Task<Response<CommentResponse>> GetCommentByIdAsync(Guid? id, CancellationToken cancellationToken)
    // {
    //     try
    //     {
    //         var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(id!.Value, cancellationToken);

    //         if (commentEntity == null)
    //         {
    //             _logger.LogError("Comment not found");
    //             return new Response<CommentResponse>(ErrorCodeEnum.COMM_ERR_001);
    //         }

    //         var commentResponse = _mapper.Map<CommentResponse>(commentEntity);
    //         return new Response<CommentResponse>(commentResponse);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         throw new ApiException(ex.Message);
    //     }
    // }

    // public async Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetCommentsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    // {
    //     var totalItems = await _applicationUnitOfWork.CommentRepository.CountAsync(x => !x.IsDeleted, cancellationToken);

    //     var comments = await _applicationUnitOfWork.CommentRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);

    //     var commentsResponse = _mapper.Map<IReadOnlyList<CommentResponse>>(comments);

    //     return new PagedResponse<IReadOnlyList<CommentResponse>>(commentsResponse, pageNumber, pageSize, totalItems);
    // }

    public async Task<Infrastructure.Shared.Wrappers.Response<Guid>> UpdateCommentAsync(CommentRequest commentRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(commentRequest.Id!.Value, cancellationToken);

            if (commentEntity == null)
            {
                _logger.LogError("Comment not found");
                return new Infrastructure.Shared.Wrappers.Response<Guid>(ErrorCodeEnum.COMM_ERR_001);
            }

            if (commentEntity.CreatedBy != currentUserId.ToString())
            {
                _logger.LogError("Can't modify comment because of authorize");
                return new Infrastructure.Shared.Wrappers.Response<Guid>(ErrorCodeEnum.COMM_ERR_003);
            }

            commentEntity.Content = commentRequest.Content;
            commentEntity.LastModified = _dateTimeService.NowUtc;
            commentEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.CommentRepository.UpdateAsync(commentEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Infrastructure.Shared.Wrappers.Response<Guid>(commentRequest.Id.Value);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }
}
