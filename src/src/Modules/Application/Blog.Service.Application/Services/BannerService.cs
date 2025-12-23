using System;
using System.Text;
using AutoMapper;
using Blog.Domain.Application.Entities;
using Blog.Files.Constants;
using Blog.Files.Interfaces;
using Blog.Infrastructure.Application.Interfaces;
using Blog.Infrastructure.Shared.ErrorCodes;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Model.Dto.Application.Requests;
using Blog.Model.Dto.Application.Responses;
using Blog.Service.Application.Constants;
using Blog.Service.Application.Interfaces;
using Blog.Shared.Auth;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.Services;

public class BannerService : IBannerService
{
    private readonly IMapper _mapper;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<BannerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;
    private readonly IFileScannerService _fileScannerService;
    private readonly Cloudinary _cloudinary;

    public BannerService(
        IMapper mapper,
        ISecurityContextAccessor securityContextAccessor,
        IApplicationUnitOfWork applicationUnitOfWork,
        IDateTimeService dateTimeService,
        ILogger<BannerService> logger,
        IFileService fileService,
        IFileScannerService fileScannerService,
        Cloudinary cloudinary,
        IConfiguration configuration
    )
    {
        _mapper = mapper;
        _applicationUnitOfWork = applicationUnitOfWork;
        _securityContextAccessor = securityContextAccessor;
        _dateTimeService = dateTimeService;
        _logger = logger;
        _fileService = fileService;
        _fileScannerService = fileScannerService;
        _configuration = configuration;

        _cloudinary = cloudinary;
    }

    public async Task<Response<BannerResponse>> GetBannerByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var bannerEntity = await _applicationUnitOfWork.BannerRepository.GetByIdAsync(id!.Value, cancellationToken);

            if (bannerEntity == null)
            {
                _logger.LogError("Khong tim thay Banner");
                return new Response<BannerResponse>(ErrorCodeEnum.BAN_ERR_001);
            }

            var bannerResponse = _mapper.Map<BannerResponse>(bannerEntity);

            return new Response<BannerResponse>(bannerResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<BannerResponse>> UploadBannerAsync(IFormFile imageFile, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {

            var contentType = System.IO.Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var extension = contentType?.TrimStart('.') ?? string.Empty;
            if (!Blog.Files.MimeDetector.MimeTypes.IsAllowedExtension(extension))
            {
                _logger.LogError("Unsupported file extension: {ext}", extension);
                throw new ApiException("File extension is not allowed");
            }
            if (imageFile.Length > FileSize.MAX_FILE_SIZE)
            {
                _logger.LogError("Image size exceeds 5MB limit");
                throw new ApiException("Image size exceeds 5MB limit");
            }
            if (imageFile.Length > 0)
            {
                var fileName = imageFile.FileName;
                // remove the uri reserved characters from file name
                foreach (var reserved in UriReserveCharacters.URI_RESERVED_CHARACTERS)
                {
                    fileName = fileName.Replace(reserved, "");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);

                    // validate the file to prevent user upload virus file
                    var fileArray = memoryStream.ToArray();
                    var result = await _fileScannerService.ScanAsync(fileName, fileArray);
                    if (result)
                    {
                        _logger.LogError("File upload has virus");
                        // found virus
                        throw new ApiException("File upload has virus");
                    }

                    // trying to open the file.
                    var openResult = _fileService.TryOpen(fileName, fileArray);
                    if (openResult == false)
                    {
                        _logger.LogError("File cannot open");
                        // invalid file, system can not open the file
                        throw new ApiException("File cannot open");
                    }
                }
            }

            // Upload to Cloudinary
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                Folder = "blog-banners",
                Transformation = new Transformation()
                    .Width(1200)
                    .Height(630)
                    .Crop("fill")
                    .Quality("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                _logger.LogError($"Cloudinary upload failed: {uploadResult.Error.Message}");
                throw new ApiException($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }

            // Create Banner
            var currentUserId = _securityContextAccessor.UserId;
            var bannerEntity = new Banner
            {
                Url = uploadResult.SecureUrl.ToString(),
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                PublicId = uploadResult.PublicId,
                Created = _dateTimeService.NowUtc,
                CreatedBy = currentUserId.ToString()
            };

            var bannerResponse = await _applicationUnitOfWork.BannerRepository.AddAsync(bannerEntity, cancellationToken, true);
            if (bannerResponse == null || bannerResponse.Id == Guid.Empty)
            {
                _logger.LogError("Failed to create banner: {error}", ErrorCodeEnum.BAN_ERR_003);
                return new Response<BannerResponse>(ErrorCodeEnum.BAN_ERR_003);
            }

            await _applicationUnitOfWork.CommitAsync();

            var bannerDto = _mapper.Map<BannerResponse>(bannerResponse);
            return new Response<BannerResponse>(bannerDto);
        }
        catch (Exception ex)
        {
            await _applicationUnitOfWork.RollbackAsync();
            _logger.LogError(ex.Message);
            throw new ApiException(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteBannerByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var currentUserId = _securityContextAccessor.UserId;

            var bannerEntity = await _applicationUnitOfWork.BannerRepository.GetByIdAsync(id!.Value, cancellationToken);
            if (bannerEntity == null)
            {
                _logger.LogError("Banner not found");
                return new Response<bool>(ErrorCodeEnum.BAN_ERR_001);
            }

            bannerEntity.LastModified = _dateTimeService.NowUtc;
            bannerEntity.LastModifiedBy = currentUserId.ToString();

            await _applicationUnitOfWork.BannerRepository.SoftDeleteAsync(bannerEntity, cancellationToken, true);
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

    public async Task<Response<Guid>> UpdateBannerAsync(BannerRequest bannerRequest, CancellationToken cancellationToken)
    {
        await _applicationUnitOfWork.BeginTransactionAsync();
        try
        {
            var isDulicateWidthOrHeight = await _applicationUnitOfWork.BannerRepository.AnyAsync(x => x.Height == bannerRequest.Height || x.Width == bannerRequest.Width, cancellationToken);
            if (isDulicateWidthOrHeight)
            {
                _logger.LogError("Duplicate", ErrorCodeEnum.BAN_ERR_006);
                return new Response<Guid>(ErrorCodeEnum.BAN_ERR_006);
            }

            var currentUserId = _securityContextAccessor.UserId;
            var bannerEntity = await _applicationUnitOfWork.BannerRepository.GetByIdAsync(bannerRequest.Id, cancellationToken);

            if (bannerEntity == null)
            {
                _logger.LogError("Not found", ErrorCodeEnum.BAN_ERR_001);
                return new Response<Guid>(ErrorCodeEnum.BAN_ERR_001);
            }

            bannerEntity.PublicId = bannerRequest.PublicId;
            bannerEntity.Url = bannerRequest.Url;
            bannerEntity.Width = bannerRequest.Width;
            bannerEntity.Height = bannerRequest.Height;
            bannerEntity.LastModified = _dateTimeService.NowUtc;
            bannerEntity.LastModifiedBy = currentUserId.ToString();
            await _applicationUnitOfWork.BannerRepository.UpdateAsync(bannerEntity, cancellationToken, true);
            await _applicationUnitOfWork.CommitAsync();

            return new Response<Guid>(bannerEntity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await _applicationUnitOfWork.RollbackAsync();
            throw new ApiException(ex.Message);
        }
    }
}
