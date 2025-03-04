using System.Collections;
using System.Reflection;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Shared.Documentation;

internal static class OpenApiExampleMapping
{
    internal static IOpenApiAny ToOpenApiAny<T>(this T subject)
        where T : class
    {
        OpenApiObject result = new();

        foreach (PropertyInfo propertyInfo in subject.GetType().GetProperties())
        {
            result[propertyInfo.Name.ToCamelCase()] = propertyInfo.GetValue(subject).MapToOpenApiAny();
        }

        return result;
    }

    private static string ToCamelCase(this string name) => name[..1].ToLower() + name[1..];

    private static IOpenApiAny MapToOpenApiAny(this object? value)
    {
        if (value is null)
        {
            return new OpenApiNull();
        }

        if (value is string s)
        {
            return new OpenApiString(s);
        }

        if (value is IEnumerable enumerable)
        {
            OpenApiArray array = [];
            array.AddRange(from object item in enumerable select item.MapToOpenApiAny());

            return array;
        }

        if (value is int i)
        {
            return new OpenApiInteger(i);
        }

        if (value is Guid g)
        {
            return new OpenApiString(g.ToString());
        }

        if (value is Enum e)
        {
            return new OpenApiString(e.ToString());
        }

        if (value is bool b)
        {
            return new OpenApiBoolean(b);
        }

        if (value.GetType().IsClass)
        {
            return value.ToOpenApiAny();
        }


        throw new InvalidOperationException($"Unsupported type {value.GetType()}.");
    }
}
