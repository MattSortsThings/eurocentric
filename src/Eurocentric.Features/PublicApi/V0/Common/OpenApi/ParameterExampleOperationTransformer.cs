using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V0.Common.OpenApi;

internal sealed class ParameterExampleOperationTransformer : BaseParameterExampleOperationTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["minPoints"] = QueryParameterExamples.MinPoints.ToOpenApiAny(),
            ["maxPoints"] = QueryParameterExamples.MaxPoints.ToOpenApiAny()
        };
}
