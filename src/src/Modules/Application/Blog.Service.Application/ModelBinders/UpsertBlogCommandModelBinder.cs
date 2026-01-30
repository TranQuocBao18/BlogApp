using System.Text.Json;
using Blog.Domain.Application.Requests;
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
                var jsonString = payloadValue.ToString()?.Trim();
                _logger.LogInformation($"Payload JSON length: {jsonString?.Length ?? 0}");
                _logger.LogInformation($"Payload JSON (first 500 chars): {jsonString?.Substring(0, Math.Min(500, jsonString?.Length ?? 0))}");

                if (!string.IsNullOrEmpty(jsonString))
                {
                    // Validate JSON format before deserialization
                    try
                    {
                        using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
                        _logger.LogInformation("Payload JSON validation passed");
                    }
                    catch (JsonException parseEx)
                    {
                        _logger.LogError($"Payload JSON validation error at position {parseEx.BytePositionInLine}: {parseEx.Message}. Raw payload: {jsonString}");
                        bindingContext.ModelState.AddModelError("Payload", $"Invalid JSON format: {parseEx.GetType().Name} - {parseEx.Message}");
                        bindingContext.Result = ModelBindingResult.Success(command);
                        return;
                    }

                    command.Payload = JsonSerializer.Deserialize<BlogRequest>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Payload deserialization error: {ex.Message}");
                bindingContext.ModelState.AddModelError("Payload", $"Invalid JSON format: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error parsing payload: {ex.Message}");
                bindingContext.ModelState.AddModelError("Payload", $"Error parsing payload: {ex.Message}");
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
