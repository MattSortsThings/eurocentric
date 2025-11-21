using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Adds an example for a schema type.
/// </summary>
/// <param name="jsonOptions">Contains JSON serialization options.</param>
public abstract class SchemaExampleTransformerBase(IOptions<JsonOptions> jsonOptions) : IOpenApiSchemaTransformer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonOptions.Value.SerializerOptions;

    protected abstract string RootNamespace { get; }

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken _) =>
        Task.CompletedTask;
}
