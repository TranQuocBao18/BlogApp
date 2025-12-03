using System;
using System.ComponentModel;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Utilities.Extensions;

public static class CommonExtensions
{
    public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);
        return model;
    }

    public static T ConvertTo<T>(this string input)
    {
        try
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(input);
        }
        catch (NotSupportedException)
        {
            return default;
        }
    }

    public static string GetGenericTypeName(this Type type)
    {
        string typeName;
        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }

    public static string ToSnakeCase(this string text)
    {
        return string.Concat(text.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }

    public static IServiceCollection AddValidatorsExt(this IServiceCollection services, params Assembly[] validatorAssemblies)
    {
        return services.Scan(scan => scan
            .FromAssemblies(validatorAssemblies)
            .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    public static TModel GetOptionsExt<TModel>(this IConfiguration configuration, string section) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);
        return model;
    }

    public static int ToInt(this Enum value)
    {
        return Convert.ToInt32(value);
    }

    public static string GetEnumDescription<T>(this T e) where T : IConvertible
    {
        if (!(e is Enum))
        {
            return string.Empty;
        }

        var field = e.GetType().GetField(e.ToString());
        var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        var description = customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : string.Empty;
        return description;
    }

    public static T ToEnum<T>(this int enumInt)
    {
        //return (T)enumInt;
        return (T)Enum.ToObject(typeof(T), enumInt);
    }

    public static double MathRound(this double value)
    {
        return Math.Round(value, 2);
    }
}
