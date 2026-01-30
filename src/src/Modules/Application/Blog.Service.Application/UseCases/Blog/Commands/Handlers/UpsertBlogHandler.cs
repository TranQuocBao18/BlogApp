using System;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Domain.Application.Requests;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using Blog.Domain.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Commands.Handlers;

public class UpsertBlogHandler : IRequestHandler<UpsertBlogCommand, Response<Guid>>
{
    private readonly IBlogService _blogService;
    private readonly IBannerService _bannerService;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public UpsertBlogHandler(
        IBlogService blogService,
        IBannerService bannerService,
        IApplicationUnitOfWork applicationUnitOfWork
    )
    {
        _blogService = blogService;
        _bannerService = bannerService;
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<Response<Guid>> Handle(UpsertBlogCommand request, CancellationToken cancellationToken)
    {
        if (request.Payload!.Id.HasValue)
        {
            return await HandleUpdate(request, cancellationToken);
        }
        return await HandleInsert(request, cancellationToken);
    }

    public async Task<Response<Guid>> HandleInsert(UpsertBlogCommand request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        await _applicationUnitOfWork.BeginTransactionAsync();
        string uploadedPublicId = null;
        try
        {
            // upload remote image (service variant that does not control transaction)
            if (request.BannerImage != null)
            {
                var fileHash = await _bannerService.CalculateFileHashAsync(request.BannerImage);

                if (await _applicationUnitOfWork.BannerRepository
                    .AnyAsync(c => c.ETag == fileHash, cancellationToken))
                {
                    var banner = _applicationUnitOfWork.BannerRepository.GetAsync(x => x.ETag == fileHash, cancellationToken);
                    uploadedPublicId = banner.Result.PublicId;
                    request.Payload.BannerId = banner.Result.Id;
                }
                else
                {
                    var bannerResult = await _bannerService.UploadBannerWithoutTransactionAsync(request.BannerImage, cancellationToken);
                    if (bannerResult == null || !bannerResult.Succeeded)
                    {
                        await _applicationUnitOfWork.RollbackAsync();
                        return new Response<Guid>(bannerResult?.ErrorCode ?? ErrorCodeEnum.BAN_ERR_003.ToString(), bannerResult?.Message ?? "Upload banner failed");
                    }

                    uploadedPublicId = bannerResult.Data?.PublicId;
                    request.Payload.BannerId = bannerResult.Data.Id;
                }

            }

            var blogResult = await _blogService.CreateBlogWithoutTransactionAsync(request.Payload, cancellationToken);
            if (blogResult == null || !blogResult.Succeeded)
            {
                await _applicationUnitOfWork.RollbackAsync();
                if (!string.IsNullOrWhiteSpace(uploadedPublicId))
                {
                    try { await _bannerService.DeleteRemoteByPublicIdAsync(uploadedPublicId, cancellationToken); } catch { }
                }
                return new Response<Guid>(blogResult?.ErrorCode ?? ErrorCodeEnum.BLOG_ERR_003.ToString(), blogResult?.Message ?? "Create blog failed");
            }

            await _applicationUnitOfWork.CommitAsync();
            return new Response<Guid>(blogResult.Data);
        }
        catch
        {
            await _applicationUnitOfWork.RollbackAsync();
            if (!string.IsNullOrWhiteSpace(uploadedPublicId))
            {
                try { await _bannerService.DeleteRemoteByPublicIdAsync(uploadedPublicId, cancellationToken); } catch { }
            }
            throw;
        }
    }

    public async Task<Response<Guid>> HandleUpdate(UpsertBlogCommand request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        await _applicationUnitOfWork.BeginTransactionAsync();
        string uploadedPublicId = null;
        var blogResult = new Response<Guid>();

        try
        {
            // upload remote image (service variant that does not control transaction)
            if (request.BannerImage != null)
            {
                var fileHash = await _bannerService.CalculateFileHashAsync(request.BannerImage);

                if (await _applicationUnitOfWork.BannerRepository
                    .AnyAsync(c => c.ETag == fileHash, cancellationToken))
                {
                    var banner = _applicationUnitOfWork.BannerRepository.GetAsync(x => x.ETag == fileHash, cancellationToken);
                    uploadedPublicId = banner.Result.PublicId;
                    request.Payload.BannerId = banner.Result.Id;
                }
                else
                {
                    var bannerResult = await _bannerService.UploadBannerWithoutTransactionAsync(request.BannerImage, cancellationToken);
                    if (bannerResult == null || !bannerResult.Succeeded)
                    {
                        await _applicationUnitOfWork.RollbackAsync();
                        return new Response<Guid>(bannerResult?.ErrorCode ?? ErrorCodeEnum.BAN_ERR_003.ToString(), bannerResult?.Message ?? "Upload banner failed");
                    }

                    uploadedPublicId = bannerResult.Data?.PublicId;
                    request.Payload.BannerId = bannerResult.Data.Id;

                    var blogEntity = await _blogService.GetBlogByIdAsync(request.Payload.Id, cancellationToken);
                    await _bannerService.DeleteBannerByIdWithoutTransactionAsync(blogEntity.Data.Banner!.Id, cancellationToken);
                    await _bannerService.DeleteRemoteByPublicIdAsync(blogEntity.Data.Banner.PublicId, cancellationToken);
                }


                blogResult = await _blogService.UpdateBlogAsync(request.Payload, cancellationToken);
                if (blogResult == null || !blogResult.Succeeded)
                {
                    await _applicationUnitOfWork.RollbackAsync();
                    if (!string.IsNullOrWhiteSpace(uploadedPublicId))
                    {
                        try { await _bannerService.DeleteRemoteByPublicIdAsync(uploadedPublicId, cancellationToken); } catch { }
                    }
                    return new Response<Guid>(blogResult?.ErrorCode ?? ErrorCodeEnum.BLOG_ERR_003.ToString(), blogResult?.Message ?? "Create blog failed");
                }
                await _applicationUnitOfWork.CommitAsync();
            }
            return new Response<Guid>(blogResult.Data);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            if (!string.IsNullOrWhiteSpace(uploadedPublicId))
            {
                try { await _bannerService.DeleteRemoteByPublicIdAsync(uploadedPublicId, cancellationToken); } catch { }
            }
            throw;
        }
    }
}
