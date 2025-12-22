using System;
using Blog.Utilities.Extensions;
using FluentValidation;

namespace Blog.Presentation.Shared.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithErrorEnum<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Enum errorCode, params string[] stringFormatValue)
    {
        rule.WithErrorCode(string.Format($"VAL_{errorCode.ToString()}"));
        try
        {
            rule.WithMessage(string.Format(errorCode.GetEnumDescription(), stringFormatValue) + "|{PropertyName}");
        }
        catch (Exception)
        {
            rule.WithMessage(errorCode.GetEnumDescription() + "|{PropertyName}");
        }
        return rule;
    }

    public static IRuleBuilderOptions<T, TProperty> WithErrorEnum<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string fileName, Enum errorCode, params string[] stringFormatValue)
    {
        rule.WithErrorCode(string.Format($"VAL_{errorCode.ToString()}"));
        try
        {
            rule.WithMessage(string.Format(errorCode.GetEnumDescription(), stringFormatValue) + $"|{fileName}");
        }
        catch (Exception)
        {
            rule.WithMessage(errorCode.GetEnumDescription() + $"|{fileName}");
        }
        return rule;
    }
}
