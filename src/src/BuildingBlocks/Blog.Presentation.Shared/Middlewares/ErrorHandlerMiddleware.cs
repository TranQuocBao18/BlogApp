using System;
using System.Net;
using System.Text.Json;
using Blog.Infrastructure.Shared.Exceptions;
using Blog.Infrastructure.Shared.Wrappers;
using Blog.Presentation.Shared.Options;
using Microsoft.AspNetCore.Http;

namespace Blog.Presentation.Shared.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var isFromUI = context.Session?.GetInt32("IsFromUI");
            if (isFromUI == null || isFromUI != 1)
            {
                // return exception to api with json
                await this.ReturnTemplateAPI(context, error);
            }
            else
            {
                // return exception to web with html
                await this.ReturnTemplateUI(context, error);
            }

        }
    }

    private async Task ReturnTemplateAPI(HttpContext context, Exception error)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };
        switch (error)
        {
            case ApiException e:
                // custom application error
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case ValidationException e:
                // custom application error
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Errors = e.Errors;
                break;
            case KeyNotFoundException e:
                // not found error
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            default:
                // unhandled error
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        var result = JsonSerializer.Serialize(responseModel);

        await response.WriteAsync(result);
    }

    private async Task ReturnTemplateUI(HttpContext context, Exception error)
    {
        var response = context.Response;
        response.ContentType = "text/html";
        var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };
        var templatePage = string.Empty;
        switch (error)
        {
            case ApiException e:
                // custom application error
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                templatePage = this.GetTemplateError("template-error-500.html");
                break;
            case ValidationException e:
                // custom application error
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Errors = e.Errors;
                templatePage = this.GetTemplateError("template-error-500.html");
                break;
            case KeyNotFoundException e:
                // not found error
                response.StatusCode = (int)HttpStatusCode.NotFound;
                templatePage = this.GetTemplateError("template-error-404.html");
                break;
            default:
                // unhandled error
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                templatePage = this.GetTemplateError("template-error-500.html");
                break;
        }

        // Validate environment to displat error detail
        var resultErrorDetail = string.Empty;

        if (AppSettingDataContext.Instance.EnableErrorDetail)
        {
            var errorDetail = JsonSerializer.Serialize(responseModel);
            var templateErrorDetail = @"<div class='padding'>
	                            <div class='col-sm-12'>
	                              <div class='form-group'>
		                            <label>Error Detail</label>
		                            <textarea class='form-control' rows='10' disabled=''>
			                            {0}
		                            </textarea>
	                              </div>
	                            </div>
                            </div>";
            resultErrorDetail = string.Format(templateErrorDetail, errorDetail);
        }

        var resultPage = string.Format(templatePage, resultErrorDetail);

        await response.WriteAsync(resultPage);
    }

    private string GetTemplateError(string pathFile)
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "template", pathFile);
        var result = File.ReadAllText(fullPath);
        return result;
    }
}
