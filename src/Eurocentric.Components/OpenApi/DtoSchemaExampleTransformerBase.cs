using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Adds OpenAPI examples for configured request/response DTO schema types.
/// </summary>
/// <param name="jsonOptions">Contains JSON serialization options.</param>
public abstract class DtoSchemaExampleTransformerBase(IOptions<JsonOptions> jsonOptions) : IOpenApiSchemaTransformer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonOptions.Value.SerializerOptions;

    /// <summary>
    ///     The root namespace to be scanned for DTO schema examples.
    /// </summary>
    protected abstract string RootNamespace { get; }

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken _)
    {
        if (schema.Example is null && TryGetExample(context.JsonTypeInfo, out JsonNode? example))
        {
            schema.Example = example;
        }

        return Task.CompletedTask;
    }

    private bool TryGetExample(JsonTypeInfo typeInfo, out JsonNode? example)
    {
        if (
            typeInfo.Type is { IsArray: false, IsValueType: false } type
            && type.Namespace?.StartsWith(RootNamespace) is true
            && type.IsAssignableTo(typeof(IDtoSchemaExampleProvider<>).MakeGenericType(type))
            && type.GetMethod(
                nameof(IDtoSchemaExampleProvider<>.CreateExample),
                BindingFlags.Public | BindingFlags.Static
            )
                is { } method
            && method.Invoke(null, null) is { } instance
        )
        {
            example = JsonSerializer.SerializeToNode(instance, _jsonSerializerOptions);

            return true;
        }

        example = null;

        return false;
    }
}
