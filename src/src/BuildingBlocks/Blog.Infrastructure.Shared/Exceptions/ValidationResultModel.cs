using System;
using System.Net;
using System.Text.Json;
using FluentValidation.Results;

namespace Blog.Infrastructure.Shared.Exceptions;

public class ValidationResultModel
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; } = "Validation Failed";
    public List<ValidationError> Errors { get; }

    public ValidationResultModel(ValidationResult result = null)
    {
        Errors = result.Errors
            .Select(err => new ValidationError(err.PropertyName, err.ErrorMessage))
            .ToList();
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
