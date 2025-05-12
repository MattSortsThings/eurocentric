using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.Documentation;

internal sealed class V1ParameterExampleTransformer : ParameterExampleTransformer
{
    private protected override IReadOnlyDictionary<string, IOpenApiAny> ParameterExamples { get; } =
        new Dictionary<string, IOpenApiAny>
        {
            ["countryId"] = new OpenApiString("13008a45-7363-4065-bbdb-59643f975903"),
            ["contestId"] = new OpenApiString("ff0c1d46-8031-42e8-8b7d-d33552623957")
        };
}
