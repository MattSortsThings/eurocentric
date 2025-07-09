using System.Collections;
using System.Reflection;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.Shared.Documentation;

internal static class OpenApiAnyMapping
{
    internal static IOpenApiAny ToOpenApiAny(this object? obj)
    {
        switch (obj)
        {
            case null:
                return new OpenApiNull();
            case string s:
                return new OpenApiString(s);
            case Guid g:
                return new OpenApiString(g.ToString());
            case bool b:
                return new OpenApiBoolean(b);
            case int i:
                return new OpenApiInteger(i);
            case double d:
                return new OpenApiDouble(Convert.ToDouble(d));
            case long l:
                return new OpenApiLong(l);
            case DateOnly d:
                return new OpenApiDate(d.ToDateTime(TimeOnly.MinValue));
        }

        if (obj.GetType().IsEnum)
        {
            return new OpenApiString(obj.ToString());
        }

        if (obj is IEnumerable enumerable)
        {
            return CreateOpenApiArray(enumerable);
        }

        if (obj.GetType().IsClass)
        {
            return CreateOpenApiObject(obj);
        }

        throw new NotSupportedException($"Type {obj.GetType()} not supported for IOpenApiAny mapping.");
    }

    private static OpenApiArray CreateOpenApiArray(IEnumerable enumerable)
    {
        OpenApiArray result = [];
        result.AddRange(from object item in enumerable select item.ToOpenApiAny());

        return result;
    }

    private static OpenApiObject CreateOpenApiObject(object obj)
    {
        OpenApiObject result = new();

        foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties().Where(property => property.CanRead))
        {
            string propertyName = propertyInfo.Name[..1].ToLower() + propertyInfo.Name[1..];

            result.Add(propertyName, propertyInfo.GetValue(obj).ToOpenApiAny());
        }

        return result;
    }
}
