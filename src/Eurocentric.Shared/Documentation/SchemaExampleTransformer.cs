using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.Documentation;

internal sealed class SchemaExampleTransformer(Dictionary<Type, IOpenApiAny> examples) : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (examples.TryGetValue(context.JsonTypeInfo.Type, out IOpenApiAny? example))
        {
            schema.Example = example;
        }

        return Task.CompletedTask;
    }
}
