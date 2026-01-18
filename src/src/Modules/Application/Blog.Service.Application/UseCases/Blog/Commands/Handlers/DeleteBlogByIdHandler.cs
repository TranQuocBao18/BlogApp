using System;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Service.Application.Interfaces;
using MediatR;

namespace Blog.Service.Application.UseCases.Blog.Commands.Handlers;

public class DeleteBlogByIdHandler : IRequestHandler<DeleteBlogByIdCommand, Response<bool>>
{
    private readonly IBlogService _blogService;
    private readonly IBannerService _bannerService;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;

    public DeleteBlogByIdHandler(
        IBlogService blogService,
        IBannerService bannerService,
        IApplicationUnitOfWork applicationUnitOfWork
        )
    {
        _blogService = blogService;
        _bannerService = bannerService;
        _applicationUnitOfWork = applicationUnitOfWork;
    }

    public async Task<Response<bool>> Handle(DeleteBlogByIdCommand request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        await _applicationUnitOfWork.BeginTransactionAsync();
        string uploadedPublicId;

        try
        {
            var blog = await _blogService.GetBlogByIdAsync(request.Id, cancellationToken);
            var banner = await _bannerService.GetBannerByIdAsync(blog.Data.BannerId, cancellationToken);
            uploadedPublicId = banner.Data.PublicId;
            await _bannerService.DeleteBannerByIdWithoutTransactionAsync(blog.Data.BannerId, cancellationToken);

            var result = await _blogService.DeleteBlogAsync(request.Id, cancellationToken);
            await _bannerService.DeleteRemoteByPublicIdAsync(uploadedPublicId, cancellationToken);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<bool>(result.Data);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            throw;
        }
    }
}
