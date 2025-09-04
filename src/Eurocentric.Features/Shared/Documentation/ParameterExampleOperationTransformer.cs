using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal abstract class ParameterExampleOperationTransformer : IOpenApiOperationTransformer
{
    private protected abstract IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; }

    public Task TransformAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        foreach (OpenApiParameter parameter in operation.Parameters ?? Enumerable.Empty<OpenApiParameter>())
        {
            if (ParameterExamples.TryGetValue(parameter.Name, out IOpenApiAny? parameterExample))
            {
                parameter.Example = parameterExample;
            }
        }

        return Task.CompletedTask;
    }
}
