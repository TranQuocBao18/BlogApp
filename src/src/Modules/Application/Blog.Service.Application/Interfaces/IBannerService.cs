using System;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Application.Interfaces;

public interface IBannerService
{
    Task<Response<BannerResponse>> GetBannerByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<Response<BannerResponse>> UploadBannerAsync(IFormFile imageFile, CancellationToken cancellationToken);
    Task<Response<BannerResponse>> UploadBannerWithoutTransactionAsync(IFormFile imageFile, CancellationToken cancellationToken);
    Task DeleteRemoteByPublicIdAsync(string publicId, CancellationToken cancellationToken);
    Task<Response<Guid>> UpdateBannerAsync(BannerRequest bannerRequest, CancellationToken cancellationToken);
    Task<Response<bool>> DeleteBannerByIdAsync(Guid? id, CancellationToken cancellationToken);
    Task<string> CalculateFileHashAsync(IFormFile file);
}
