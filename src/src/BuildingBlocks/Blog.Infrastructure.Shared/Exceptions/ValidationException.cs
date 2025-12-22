using System;
using System.Runtime.Serialization;
using FluentValidation.Results;

namespace Blog.Infrastructure.Shared.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public List<string> Errors { get; }
    public ValidationResultModel ValidationResultModel { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }


    public ValidationException(ValidationResultModel validationResultModel)
    {
        ValidationResultModel = validationResultModel;
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        foreach (var failure in failures)
        {
            Errors.Add(failure.ErrorMessage);
        }
    }

    protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
