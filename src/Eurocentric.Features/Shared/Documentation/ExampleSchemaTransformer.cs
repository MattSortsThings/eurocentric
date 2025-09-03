using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal abstract class ExampleSchemaTransformer : IOpenApiSchemaTransformer
{
    private protected abstract IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; }

    public Task TransformAsync(OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (SchemaExamples.TryGetValue(context.JsonTypeInfo.Type, out IOpenApiAny? example))
        {
            schema.Example = example;
        }

        return Task.CompletedTask;
    }
}
