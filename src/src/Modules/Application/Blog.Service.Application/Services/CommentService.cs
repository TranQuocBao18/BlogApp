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

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<CommentService> _logger;

    public CommentService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<CommentService> logger
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task<Response<Guid>> CreateCommentAsync(CommentRequest commentRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var commentEntity = _mapper.Map<Comment>(commentRequest);
            commentEntity.Created = _dateTimeService.NowUtc;
            commentEntity.CreatedBy = currentUserId.ToString();

            var commentResponse = await _applicationUnitOfWork.CommentRepository.AddAsync(commentEntity, cancellationToken, true);
            if (commentResponse == null || commentResponse.Id == Guid.Empty)
            {
                _logger.LogError("Create Comment fail");
                return new Response<Guid>(ErrorCodeEnum.COMM_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();
            return new Response<Guid>(commentResponse.Id);

        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteCommentAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (commentEntity == null)
            {
                _logger.LogError("Comment not found");
                return new Response<bool>(ErrorCodeEnum.COMM_ERR_001);
            }

            commentEntity.LastModified = _dateTimeService.NowUtc;
            commentEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.CommentRepository.SoftDeleteAsync(commentEntity, cancellationToken, true);
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

    public async Task<Response<CommentResponse>> GetCommentByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(id!.Value, cancellationToken);

            if (commentEntity == null)
            {
                _logger.LogError("Comment not found");
                return new Response<CommentResponse>(ErrorCodeEnum.COMM_ERR_001);
            }

            var commentResponse = _mapper.Map<CommentResponse>(commentEntity);
            return new Response<CommentResponse>(commentResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<PagedResponse<IReadOnlyList<CommentResponse>>> GetCommentsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var totalItems = await _applicationUnitOfWork.CommentRepository.CountAsync(x => !x.IsDeleted, cancellationToken);

        var comments = await _applicationUnitOfWork.CommentRepository.GetPagedReponseAsync(pageNumber, pageSize, cancellationToken);

        var commentsResponse = _mapper.Map<IReadOnlyList<CommentResponse>>(comments);

        return new PagedResponse<IReadOnlyList<CommentResponse>>(commentsResponse, pageNumber, pageSize, totalItems);
    }

    public async Task<Response<Guid>> UpdateCommentAsync(CommentRequest commentRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;
            var commentEntity = await _applicationUnitOfWork.CommentRepository.GetByIdAsync(commentRequest.Id!.Value, cancellationToken);

            if (commentEntity == null)
            {
                _logger.LogError("Comment not found");
                return new Response<Guid>(ErrorCodeEnum.COMM_ERR_001);
            }

            commentEntity.Content = commentRequest.Content;
            commentEntity.LastModified = _dateTimeService.NowUtc;
            commentEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.CommentRepository.UpdateAsync(commentEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<Guid>(commentRequest.Id.Value);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }
}
