using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Adds an example for any schema type that implements <see cref="ISchemaExampleProvider{T}" />.
/// </summary>
/// <param name="jsonOptions">Contains JSON serialization options.</param>
public abstract class SchemaExampleTransformerBase(IOptions<JsonOptions> jsonOptions) : IOpenApiSchemaTransformer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonOptions.Value.SerializerOptions;

    protected abstract string RootNamespace { get; }

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken _)
    {
        if (schema.Example is null && TryGetExample(context.JsonTypeInfo.Type, out object? example))
        {
            schema.Example = JsonSerializer.SerializeToNode(example, _jsonSerializerOptions);
        }

        return Task.CompletedTask;
    }

    private bool TryGetExample(Type type, [NotNullWhen(true)] out object? example)
    {
        if (
            type is { IsArray: false, IsValueType: false }
            && type.Namespace?.StartsWith(RootNamespace) is true
            && type.IsAssignableTo(typeof(ISchemaExampleProvider<>).MakeGenericType(type))
            && type.GetMethod(nameof(ISchemaExampleProvider<>.CreateExample), BindingFlags.Public | BindingFlags.Static)
                is { } method
            && method.Invoke(null, null) is { } instance
        )
        {
            example = instance;

            return true;
        }

        example = null;

        return false;
    }
}
