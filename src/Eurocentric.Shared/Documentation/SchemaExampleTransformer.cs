using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.Documentation;

internal sealed class SchemaExampleTransformer : IOpenApiSchemaTransformer
{
    private readonly IDictionary<Type, IOpenApiAny> _examples;

    public SchemaExampleTransformer(IDictionary<Type, IOpenApiAny> examples)
    {
        _examples = examples ?? throw new ArgumentNullException(nameof(examples));
    }

    public Task TransformAsync(OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (_examples.TryGetValue(context.JsonTypeInfo.Type, out IOpenApiAny? example))
        {
            schema.Example = example;
        }

        return Task.CompletedTask;
    }
}
