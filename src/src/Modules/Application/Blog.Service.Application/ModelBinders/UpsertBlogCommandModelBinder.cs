using System.Text.Json;
using Blog.Model.Dto.Application.Requests;
using Blog.Service.Application.UseCases.Blog.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Blog.Service.Application.ModelBinders;

public class UpsertBlogCommandModelBinder : IModelBinder
{
    private readonly ILogger<UpsertBlogCommandModelBinder> _logger;

    public UpsertBlogCommandModelBinder(ILogger<UpsertBlogCommandModelBinder> logger)
    {
        _logger = logger;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var httpContext = bindingContext.HttpContext;
        var form = await httpContext.Request.ReadFormAsync();

        var command = new UpsertBlogCommand();

        _logger.LogInformation($"Form fields count: {form.Count}");
        _logger.LogInformation($"Form files count: {form.Files.Count}");

        // Parse Payload từ JSON string
        if (form.TryGetValue("Payload", out var payloadValue))
        {
            try
            {
                var jsonString = payloadValue.ToString();
                _logger.LogInformation($"Payload JSON: {jsonString}");
                if (!string.IsNullOrEmpty(jsonString))
                {
                    command.Payload = JsonSerializer.Deserialize<BlogRequest>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Payload JSON parse error: {ex.Message}");
                bindingContext.ModelState.AddModelError("Payload", $"Invalid JSON format: {ex.Message}");
            }
        }

        // Lấy BannerImage
        if (form.Files.Count > 0)
        {
            var bannerImage = form.Files.GetFile("BannerImage");
            if (bannerImage != null && bannerImage.Length > 0)
            {
                _logger.LogInformation($"BannerImage found: {bannerImage.FileName}, Size: {bannerImage.Length}");
                command.BannerImage = bannerImage;
            }
            else
            {
                _logger.LogWarning("BannerImage not found or is empty");
            }
        }

        bindingContext.Result = ModelBindingResult.Success(command);
    }
}
