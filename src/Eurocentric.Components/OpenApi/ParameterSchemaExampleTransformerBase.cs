using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Adds OpenAPI examples for configured parameters.
/// </summary>
public abstract class ParameterSchemaExampleTransformerBase : IOpenApiSchemaTransformer
{
    /// <summary>
    ///     Contains route parameter names and their examples.
    /// </summary>
    protected abstract IReadOnlyDictionary<string, JsonNode?> Examples { get; }

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken _)
    {
        if (TryGetExample(context.ParameterDescription, out JsonNode? example))
        {
            schema.Example = example;
        }

        return Task.CompletedTask;
    }

    private bool TryGetExample(ApiParameterDescription? parameterDescription, out JsonNode? example)
    {
        if (parameterDescription is { Name: var name } && Examples.TryGetValue(name, out JsonNode? node))
        {
            example = node;

            return true;
        }

        example = null;

        return false;
    }
}
